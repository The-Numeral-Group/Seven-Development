using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorHealth : MonoBehaviour
{
    //Public Fields (Inspector Accessable)
    public int startingMaxHealth = 100;
    //this value determines how resistant to damage this thing is
    [SerializeField]
    [Range(0.0f, 1.0f)]
    public float damageResistance = 0.0f;

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
    void Update()
    {
        Debug.Log(this.gameObject.name + " Hp: " + currentHealth);
    }

    public virtual void takeDamage(int damageTaken){
        var damage = (int)Mathf.Floor(damageTaken * (1.0f - damageResistance));
        Debug.Log("taking " + damage + " damage");

        //take the damage
        this.currentHealth -= damage;

        //if the attack killed the thing
        if(this.currentHealth <= 0){
            /*I'd like to use SendMessageOptions.RequireReciever to make it so
            that the game vomits if we try to kill something that cannot die,
            but I just don't know how*/
            this.gameObject.SendMessage("DoActorDeath");//, null, RequireReciever);
        }
    }
}
