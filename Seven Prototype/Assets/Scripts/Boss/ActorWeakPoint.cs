using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorWeakPoint : ActorHealth
{
    public ActorHealth ownerHealth;    //The object that this weakpoint effects
    public float damageMultiplier = 1.0f;
    public bool bypassDamageResistance = true;

    //ActorHealth ownerHealth;
    // Start is called before the first frame update
    void Start()
    {
        /*if(owner == null){
            owner = this.gameObject.transform.parent.gameObject;

            if(owner == null){Debug.LogError("Weakpoint created without owner!");}
        }
        ownerHealth = owner.GetComponent<ActorHealth>();
        if(ownerHealth == null){Debug.LogError("Weakpoint owner can't take damage!");}*/

        if(ownerHealth == null){
            ownerHealth = this.gameObject.transform.parent.gameObject.GetComponent<ActorHealth>();
            if(ownerHealth == null){Debug.LogError("Weakpoint owner can't take damage!");}
        }
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    //new in a method declaration means "use me rather than my superclass's version"
    public override void takeDamage(int damageTaken){

        //take the damage to the weakpoint
        this.currentHealth -= (int)Mathf.Floor(damageTaken * (1.0f - damageResistance));

        //then deal the damage to the owner
        if(bypassDamageResistance){
            //We divide by 100 - ownerHealth.damage resistance to cancel out the damage resistance
            //in the owner, with is * 100 - ownerHealth.damageResistance
            ownerHealth.takeDamage((int)Mathf.Floor(
                damageTaken * damageMultiplier / (1.0f - ownerHealth.damageResistance)));
        }
        else{
            ownerHealth.takeDamage((int)Mathf.Floor(damageTaken * damageMultiplier));
        }

        //if the attack killed the thing
        if(this.currentHealth <= 0){
            /*I'd like to use SendMessageOptions.RequireReciever to make it so
            that the game vomits if we try to kill something that cannot die,
            but I just don't know how*/
            this.gameObject.SendMessage("DoActorDeath");//, null, RequireReciever);
        }
    }
}
