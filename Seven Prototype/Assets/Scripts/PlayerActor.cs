using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorHealth))]
public class PlayerActor : MonoBehaviour
{

    //Public Fields (Inspector Accessable)
    public GameObject startingWeaponObject;   //the player's weapon
    
    //Public Properties (Publicly Accessable)

    //Private Fields (fuck you, and your accesability)
    private GameObject weaponObject;
    private Vector2 facingDirection = new Vector2(-1.0f, 0);

    //temp variable, details on weapon's anchor point should be in weapon
    private Vector2 weaponPositionScale = new Vector2(0.1f, 0.1f);

    // Start is called before the first frame update
    void Start()
    {
        //create the weapon object but deactivate it for now
        weaponObject = Instantiate(startingWeaponObject, this.gameObject.transform);
        //the weapon isn't following the game object, I've heard this helps?
        weaponObject.transform.localPosition = weaponPositionScale;
        //weapon = weaponObject.GetComponent<Weapon>();
        weaponObject.SetActive(false);
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
    
    //make the actor attack
    public void DoActorAttack(){
        weaponObject.SetActive(true);
        weaponObject.transform.localPosition = facingDirection * weaponPositionScale;
        weaponObject.SendMessage("DoWeaponStrike");
    }

    //make the actor die
    public void DoActorDeath(){
        this.gameObject.SetActive(false);
    }

    public void DoActorUpdateFacing(Vector2 newDirection){
        facingDirection = newDirection;
    }
}
