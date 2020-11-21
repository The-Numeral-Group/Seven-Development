using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectHandler : MonoBehaviour
{
    //should be actor, but that doesn't exist yet
    MonoBehaviour player;
    List<ActorEffect> activeEffects;

    void Awake(){
        activeEffects = new List<ActorEffect>();
    }

    void Start(){
        player = this.gameObject.GetComponent<PlayerActor>();
    }

    //Add any effect!
    public void AddEffect(ActorEffect effect){
        if(effect.ApplyEffect(ref player)){ activeEffects.Add(effect); };
    }

    //Remove an effect you have a reference to
    public void RemoveEffect(ActorEffect effect){
        if(activeEffects.Contains(effect)) {activeEffects.Remove(effect);}
        effect.RemoveEffect(ref player);
    }

    //Remove the first effect of type T
    public void RemoveEffectByType<T>(){
        //find the first element of type T
        var foundEffect = activeEffects.Find(i => {return i.GetType() == typeof(T);});
        
        RemoveEffect(foundEffect);
    }

    //Remove all the effects of type T
    public void RemoveAllEffectsByType<T>(){
        //get a List<ActorEffect> of all the elements of type T
        var foundEffects = activeEffects.FindAll(i => {return i.GetType() == typeof(T);});

        foreach(ActorEffect effect in foundEffects){
            RemoveEffect(effect);
        }
    }

    //Remove all effects
    public void RemoveAllEffects(){
        //we need to use a while loop here because, unlike the above,
        //  we're making changes to the original list. C# does not allow that
        //  in a foreach loop (it gives an Invalid Operation Exception)
        while(activeEffects.Count != 0){
            RemoveEffect(activeEffects[0]);
        }
        Debug.Log("Total cleanse completed!");
    }
}
