using UnityEngine;
using System.Collections;

public interface ActorAbility{

    bool getUsable();

    IEnumerator coolDown(float period);

    void Invoke(ref MonoBehaviour player);
}

/*Because this is a full class, it's the only thing Abilities
can inherit from. I've made it a MonoBehaviour to make */
public abstract class ActorAbilityFunctions<T1, T2> : MonoBehaviour, ActorAbility
{
    public float cooldownPeriod;

    private bool usable = true;

    public bool getUsable(){return usable;}

    //overridable coroutine for handling ability cooldowns.
    public virtual IEnumerator coolDown(float period)
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
    //should also look for an Actor, but that type doesn't exist yet
    public virtual void Invoke(ref MonoBehaviour user)
    {
        if(usable)
        {
            InternInvoke(null);
            StartCoroutine(coolDown(cooldownPeriod));
        }
        
    }

    /*this is only accessable to derivative classes, as it's protected
    it's also the method that does the ability, feel free to make the
    types whatever you want*/
    protected abstract T2 InternInvoke(params T1[] args);
}
