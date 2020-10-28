using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//I don't know what the actual name of the component is for the require lol
[RequireComponent(typeof(CharacterController),typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public float speedScalar = 5.0f;

    CharacterController controller;
    InputAction movementAction;

    // Start is called before the first frame update
    void Start()
    {
        controller = this.gameObject.GetComponent<CharacterController>();
        //having to keep a reference to the action being referenced defeats the purpose
        //of using Send Messages, but this seems to be the only way to do this right now
        movementAction = this.gameObject.GetComponent<PlayerInput>().actions["Movement"];
        //Debug.Log(Keyboard.keyboardLayout);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMovement(InputValue input){
        //controller.Move(input.Get<Vector2>() * speedScalar * Time.deltaTime);
        //Debug.Log(input.isPressed);
        StartCoroutine(DoMovement(input));
    }

    IEnumerator DoMovement(InputValue input)
    {
        Debug.Log("movement has been registered");

        Vector2 movementInput = input.Get<Vector2>();
        //InputAction inputAction = input.Get<InputAction>();
        Debug.Log("Input retreieved");

        //we scale by 1: speed and 2: time, to normalize the speed with the current flow of Unity Time  
        /*while(movementAction.triggered)
        {
            Debug.Log("smoovin...");
            controller.Move(movementInput * speedScalar * Time.deltaTime);
        }*/

        yield return null;
    }

    void OnAttack(){
        Debug.Log("attack has been registered");
    }
}
