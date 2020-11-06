using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoss : MonoBehaviour
{
    public float inverse_speed = 2;
    public float walk_speed;
    public float crush_range = 60f;
    public Vector3 movementDirection;
    public Vector3 currentPos;
    GameObject player;
    Rigidbody2D rb;
    BoxCollider2D bc;
    float heightOffset = 20;
    float lastX;
    float lastY;
    bool canAttack = false;

    private State state;
    private enum State
    {
        Walk,
        Physical_Crush,
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        bc.enabled = true;
        lastX = transform.position.x;
        lastY = transform.position.y;

        state = State.Walk;
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementDirection();
        CheckDistance();
    }

    void FixedUpdate() 
    {
        switch (state)
        {
            case State.Walk:
                StartCoroutine(WalkToTarget());
                break;
            case State.Physical_Crush:
                StartCoroutine(MoveToTarget());
                break;
        }
    }

    // This gets the movement direction of gluttony.
    // Vector movementDirection is being used when player collides with gluttony,
    // player will be pushed off to the direction gluttony is moving. 
    void GetMovementDirection()
    {
        if (transform.position.x > lastX)
        {
            movementDirection.x = 1.0f;
            lastX = transform.position.x;
        }
        else if (transform.position.x < lastX)
        {
            movementDirection.x = -1.0f;
            lastX = transform.position.x;
        }
        if (transform.position.y > lastY)
        {
            movementDirection.y = 1.0f;
            lastY = transform.position.y;
        }
        else if (transform.position.y < lastY)
        {
            movementDirection.y = -1.0f;
            lastY = transform.position.y;
        }
    }

    // Check distance between gluttony and player.
    // If distance goes over crush_range, 
    // gluttony uses the crush ability to get closer to player.
    void CheckDistance()
    {
        //Debug.Log(Vector2.Distance(player.transform.position, this.gameObject.transform.position));
        if (Vector2.Distance(player.transform.position, this.gameObject.transform.position) >= crush_range)
        {
            state = State.Physical_Crush;
        }
    }

    Vector3 GetPathToPlayerPos()
    {
        Vector2 playerPos = new Vector3(player.transform.position.x, player.transform.position.y);
        playerPos.y += heightOffset;
        return playerPos;
    }

    IEnumerator WalkToTarget()
    {
        Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        currentPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        transform.position = Vector3.MoveTowards(currentPos, playerPos, walk_speed * Time.fixedDeltaTime);
        yield return null;
    }

    //https://answers.unity.com/questions/14279/make-an-object-move-from-point-a-to-point-b-then-b.html
    //answer by: philjhale
    IEnumerator MoveToTarget()
    {
        Vector2 playerPos = GetPathToPlayerPos();
        Vector2 currentPos = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        float rate = 1.0f/inverse_speed;
        for (float i = 0.0f; i <= 1.0f; i+=Time.deltaTime * rate) {
            transform.position = Vector2.Lerp(currentPos, playerPos, i);
            yield return null;
        }
        canAttack = true;
        Debug.Log("Move Finished");
        StartCoroutine(SlamDown());
    }

    IEnumerator SlamDown()
    {
        Vector2 currentPos = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        Vector2 desiredPos = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        desiredPos.y -= heightOffset;
        float rate = 1.0f/(inverse_speed / 10);
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * rate) {
            transform.position = Vector2.Lerp(currentPos, desiredPos, i);
            yield return null;
        }
        bc.enabled = true;
        yield return new WaitForSeconds(1);
        bc.enabled = false;
        canAttack = false;
        Debug.Log("Slam Finished");
        state = State.Walk;
        StartCoroutine(MoveBackUp());
    }

    IEnumerator MoveBackUp()
    {
        Vector2 currentPos = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        Vector3 desiredPos = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        desiredPos.y += heightOffset;
        float rate = 1.0f/(inverse_speed * 2);
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * rate) {
            transform.position = Vector2.Lerp(currentPos, desiredPos, i);
            yield return null;
        }
        state = State.Walk;
        Debug.Log("Reset Finished");
    }

    void DoActorDeath() {
        this.gameObject.SetActive(false);
    }
}
