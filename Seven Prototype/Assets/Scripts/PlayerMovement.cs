using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.CallbackContext;

//I don't know what the actual name of the component is for the require lol
[RequireComponent(typeof(CharacterController),typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public float speedScalar = 5.0f;
    //Variables used during dodge: Drag, and dodge Distance
    public float dodgeDistance = 15.0f;
    public Vector2 Drag = new Vector2(10, 10);

    CharacterController controller;
    Vector2 movementDirection;
    Animator animator;
    //Variables for Dodging: isDodging,
    bool isDodging;
    Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = this.gameObject.GetComponent<CharacterController>();
        movementDirection = Vector2.zero;
        animator = this.gameObject.GetComponent<Animator>();

        //Dodge control variables
        isDodging = false;
        velocity = new Vector2(0, 0);
        //having to keep a reference to the action being referenced defeats the purpose
        //of using Send Messages, but this seems to be the only way to do this right now
        //Debug.Log(Keyboard.keyboardLayout);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void OnMovement(InputValue input){
        //controller.Move(input.Get<Vector2>() * speedScalar * Time.deltaTime);
        //Debug.Log(input.isPressed);
        //StartCoroutine(DoMovement(input));
        movementDirection = input.Get<Vector2>();
    }

    void MovePlayer()
    {
        if (movementDirection != Vector2.zero)
        {
            animator.SetBool("player_moving", true);
            animator.SetFloat("player_H", movementDirection.x);
            animator.SetFloat("player_V", movementDirection.y);
        }
        else
        {
            animator.SetBool("player_moving",false);
        }
        controller.Move(movementDirection * speedScalar * Time.deltaTime);
        //Dodge Controller
        controller.Move(velocity * Time.deltaTime);
        velocity.x /= 1 + Drag.x * Time.deltaTime;
        velocity.y /= 1 + Drag.y * Time.deltaTime;
    }

    /*IEnumerator DoMovement(InputAction.CallbackContext input)
    {
        Debug.Log("movement has been registered");

        Vector2 movementInput = input.ReadValue<Vector2>();
        //InputAction inputAction = input.Get<InputAction>();
        Debug.Log("Input retreieved. Is button pressed? " + input.action.isPressed);

        //we scale by 1: speed and 2: time, to normalize the speed with the current flow of Unity Time  
        /*while(input.isPressed)
        {
            Debug.Log("smoovin...");
            controller.Move(movementInput * speedScalar * Time.deltaTime);
        }*

        yield return null;
    }*/

    void OnAttack(){
        Debug.Log("attacking...");
        this.gameObject.SendMessage("DoActorAttack");
    }
    void OnDodge()
    {
        if (!isDodging) {
            isDodging = true;
            //https://medium.com/ironequal/unity-character-controller-vs-rigidbody-a1e243591483
            //credit to above link for equation used.
            Debug.Log("Dodge Start");
            velocity += Vector2.Scale(movementDirection, dodgeDistance * new Vector2((Mathf.Log(1f/ (Time.deltaTime * Drag.x + 1))/-Time.deltaTime),
                                                                                                (Mathf.Log(1f/ (Time.deltaTime * Drag.y + 1))/-Time.deltaTime)));
            StartCoroutine(Dodge());
        }
    }

    IEnumerator Dodge() 
    {
        yield return new WaitForSeconds(1);
        isDodging = false;
        Debug.Log("Dodge over");
    }
}
