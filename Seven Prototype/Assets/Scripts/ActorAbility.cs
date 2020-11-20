using UnityEngine;
using System.Collections;

/*Because this is a full class, it's the only thing Abilities
can inherit from. I've made it a MonoBehaviour to make */
public abstract class ActorAbility : MonoBehaviour
{
    public float cooldownPeriod = 5.0f;

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
    public virtual void Invoke()
    {
        if(usable)
        {
            InternInvoke<int, int>(new int[0]);
            StartCoroutine(coolDown(cooldownPeriod));
        }
        
    }

    /*this is only accessable to derivative classes, as it's protected
    it's also the method that does the ability, feel free to make the
    types whatever you want*/
    protected abstract T2 InternInvoke<T1, T2>(params T1[] args);
}
