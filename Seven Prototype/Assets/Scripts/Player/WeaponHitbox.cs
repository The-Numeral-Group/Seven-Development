using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class WeaponHitbox: MonoBehaviour
{
    //ints start with a value of 0 in C#
    public int damage { get; set; }

    // Start is called before the first frame update
    /*void Start()
    {
        
    }*/

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    void OnCollisionEnter2D(Collision2D collider){
        //try to get the enemy's health object
        var enemyHealth = collider.gameObject.GetComponent<ActorHealth>();
        //or a weakpoint if there's no regular health
        if(enemyHealth == null){collider.gameObject.GetComponent<ActorWeakPoint>();}

        //if the enemy can take damage (if it has an ActorHealth component),
        //hurt them. Do nothing if they can't take damage.
        if(enemyHealth != null){
            enemyHealth.takeDamage(this.damage);
        }
    }
}
