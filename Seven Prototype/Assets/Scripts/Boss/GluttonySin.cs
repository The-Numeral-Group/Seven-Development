using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GluttonySin : MonoBehaviour, ActorEffect
{
    //static value so every GluttonySin knows how many have
    //already been activated
    public static int maxGluts = 3;
    public static int appliedGluts = 0;

    //Strength of Gluttony's sin
    [Tooltip("Percentage of the player's original max health that will be ADDED (1 is 100%)")]
    public float healthIncrease = 1.0f;
    [Tooltip("Percentage of the player's original speed that will be REMOVED (1 is 100%)")]
    public float speedDecrease = 0.3f;
    [Tooltip("Should this be deleted if the related effect stops?")]
    public bool deleteOnEffectEnd = true;

    //when this thing gets walked into...
    void OnTriggerEnter2D(Collider2D collider){
        Debug.Log("collision with food!");
        //currently gets parent because that's how player collision works right now
        PlayerEffectHandler collidedEffectHandler 
            = collider.gameObject.transform.parent.gameObject.GetComponent<PlayerEffectHandler>();

        //if the thing we collided with has a PlayerEffectHandler component,
        //apply ourselves to it
        if(collidedEffectHandler != null){
            collidedEffectHandler.AddEffect(this);
        }
        

    }

    
    public bool ApplyEffect(ref PlayerActor player){
        if(GluttonySin.appliedGluts < GluttonySin.maxGluts){
            var health = player.getHealthObject();
            var movement = player.getMovementObject();

            health.maxHealth += (int)(health.startingMaxHealth * this.healthIncrease);
            health.currentHealth += (int)(health.startingMaxHealth * this.healthIncrease);

            movement.speed -= movement.startingSpeed * this.speedDecrease;

            ++GluttonySin.appliedGluts;

            this.gameObject.SetActive(false);

            Debug.Log("Food eaten!");
            return true;
        }
        Debug.Log("Too much glut!");
        return false;
    }

    public void RemoveEffect(ref PlayerActor player){
        if(GluttonySin.appliedGluts > 0){
            var health = player.getHealthObject();
            var movement = player.getMovementObject();

            health.maxHealth -= (int)(health.startingMaxHealth * this.healthIncrease);
            health.currentHealth -= (int)(health.startingMaxHealth * this.healthIncrease);

            movement.speed += movement.startingSpeed * this.speedDecrease;

            --GluttonySin.appliedGluts;
        }

        if(this.deleteOnEffectEnd){
            Destroy(this.gameObject);
        }
    }
}
