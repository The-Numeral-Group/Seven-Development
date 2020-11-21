using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowtime : ActorAbilityFunctions<PlayerEffectHandler, int>
    /*technically, we want a void return type, but that's not allowed
    for generics in C#, so I'm just using int*/
{
    public float cooldownPeriod = 5.0f;
    public float userSlow;
    public float targetSlow;
    public float userSlowDuration;
    public float targetSlowDuration;

    private bool usable = true;
    
    //this should be actor, but that type doesn't exist yet
    MonoBehaviour user;
    //these need to be abstracted to regular ActorEffectHandlers
    //  that type just doesn't exist yet
    PlayerEffectHandler playerEffecter;
    PlayerEffectHandler bossEffecter;
    Slow playerEffect;
    Slow otherEffect;

    // Start is called before the first frame update
    void Start()
    {
        //make new effect objects based on desired durations
        playerEffect = new Slow(userSlow);
        otherEffect = new Slow(targetSlow);

        //locate targets but in a more generic way
        //playerEffecter = user.gameObject.GetComponent<PlayerEffectHandler>();
        bossEffecter = GameObject.FindWithTag("Gluttony").GetComponent<PlayerEffectHandler>();
    }

    //comes from parent
    //public bool getUsable(){return usable;}

    //overridable coroutine for handling ability cooldowns.
    public override IEnumerator coolDown(float period)
    {
        usable = false;
        yield return new WaitForSeconds(period);
        usable = true;
    }

    /*overridable method for activating the ability. While nothing is
    making you call InternInvoke or start coolDown, you should still do
    those manually.
    
    and, fyi, Invoke and InternInvoke are seperated so things using abilities
    don't need to worry about conflicting argument and return types*/
    public override void Invoke(ref MonoBehaviour user)
    {
        if(this.user != user){
            this.user = user;
            playerEffecter = this.user.gameObject.GetComponent<PlayerEffectHandler>();
        }

        if(usable)
        {
            InternInvoke(playerEffecter, bossEffecter);
            StartCoroutine(coolDown(cooldownPeriod));
        }
        
    }

    /*this is only accessable to derivative classes, as it's protected
    it's also the method that does the ability, feel free to make the
    types whatever you want*/
    //args #0 is the player, args #1 is the boss.
    //is using override and generic notation like this legal...?
    //one day I will figure out a way to declare a method as genetic in
    //an interface and then implement the types in the realizations.
    //  but that day is not today
    protected override int InternInvoke(params PlayerEffectHandler[] args)
    {
        //AddTimedEffect and PlayerEffectHandler don't technically exist yet...
        //so let's do some funky coroutines!
        //args[0].AddTimedEffect(userSlow, userSlowDuration);
        StartCoroutine(TimedEffect(args[0], playerEffect, userSlowDuration));
        //It's possible that there will be no enemy to target with this
        if(args[1] != null)
        {
            StartCoroutine(TimedEffect(args[1], otherEffect, targetSlowDuration));
        }

        /*technically, we want a void return type, but that's not allowed
        for generics in C#, so I'm just using int*/
        return 0;
    }

    //this functionality will be integrated into ActorEffectHandler later
    IEnumerator TimedEffect(PlayerEffectHandler recipient, ActorEffect effect, float duration){
        recipient.AddEffect(effect);
        yield return new WaitForSeconds(duration);
        recipient.RemoveEffect(effect);
    }
}



class Slow : ActorEffect
{
    float effect;

    public Slow(float effect){
        this.effect = effect;
    }

    //these should look for regular actors, but that type
    //doesn't exist yet
    public bool ApplyEffect(ref MonoBehaviour actor)
    {
        //locate whatever controls speed and slow it
        return true;
    }

    public void RemoveEffect(ref MonoBehaviour actor)
    {
        //locate whatever controls speed and hasten it
    }
}
