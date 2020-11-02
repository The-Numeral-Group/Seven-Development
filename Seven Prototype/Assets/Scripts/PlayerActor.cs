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

    // Start is called before the first frame update
    void Start()
    {
        //create the weapon object but deactivate it for now
        weaponObject = Instantiate(startingWeaponObject, this.gameObject.transform);
        //the weapon isn't following the game object, I've heard this helps?
        weaponObject.transform.localPosition = new Vector2(0.1f, 0.0f);
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
        weaponObject.SendMessage("DoWeaponStrike");
    }

    //make the actor die
    public void DoActorDeath(){
        this.gameObject.SetActive(false);
    }
}
