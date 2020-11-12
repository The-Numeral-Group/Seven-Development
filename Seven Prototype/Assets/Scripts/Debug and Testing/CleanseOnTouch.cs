using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CleanseOnTouch : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider){
        PlayerEffectHandler collidedEffectHandler 
            = collider.gameObject.transform.parent.gameObject.GetComponent<PlayerEffectHandler>();

        //if the thing we collided with has a PlayerEffectHandler component,
        //apply ourselves to it
        if(collidedEffectHandler != null){
            collidedEffectHandler.RemoveAllEffects();
        }
    }
}
