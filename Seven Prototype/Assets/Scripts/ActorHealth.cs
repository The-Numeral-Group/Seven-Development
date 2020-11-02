using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorHealth : MonoBehaviour
{
    //Public Fields (Inspector Accessable)
    public int startingMaxHealth = 100;

    //Public Properties (Publicly Accessable)
    public int maxHealth { get; set; }
    public int currentHealth { get; set; }

    void Awake(){
        this.maxHealth = startingMaxHealth;
        this.currentHealth = this.maxHealth;
    }
    
    // Start is called before the first frame update
    /*void Start()
    {
        
    }*/

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    public void takeDamage(int damageTaken){
        //take the damage
        this.currentHealth -= damageTaken;

        //if the attack killed the thing
        if(this.currentHealth <= 0){
            /*I'd like to use SendMessageOptions.RequireReciever to make it so
            that the game vomits if we try to kill something that cannot die,
            but I just don't know how*/
            this.gameObject.SendMessage("DoActorDeath");//, null, RequireReciever);
        }
    }
}
