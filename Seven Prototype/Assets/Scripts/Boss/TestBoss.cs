using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoss : MonoBehaviour
{
    public float inverse_speed = 1;
    public float walk_speed;
    public float drag_speed = 5f;
    public float crush_range = 60f;
    public float bite_range = 25f;

    public Vector3 movementDirection;
    public Vector3 currentPos;

    public GameObject projectile;
    public GameObject bite;
    public GameObject PhaseOne_attackRange;
    public GameObject crush_shadow;
    public GameObject sin_item;

    GameObject player;
    GameObject camera;

    Rigidbody2D rb;
    PolygonCollider2D pc;
    ActorHealth health;

    float riseHeightOffset = 120;
    float fallHeightOffset = 120;
    float lastX;
    float lastY;
    float ph1_attack_counter = 0;

    bool projectileOnCooldown = false;
    bool biteOnCooldown = false;

    public State state;
    public enum State
    {
        Walk,
        Physical_Crush,
        Crushed,
        Physical_Bite,
        Bited,
        PH1_SA,
        PH1_SA_activated,
        Fire_Projectile,
        Null,
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        camera = GameObject.Find("ShakeCam");
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PolygonCollider2D>();
        health = GetComponent<ActorHealth>();
        pc.enabled = true;
        lastX = transform.position.x;
        lastY = transform.position.y;

        state = State.Walk;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        switch (state)
        {
            //Moved the functions in update to walk to avoid the stuttering that occurs on ground pound.
            case State.Walk:
                checkHealth();
                GetMovementDirection();
                // check for special attack counter.
                // if it is 7, activate special attack. 
                if (ph1_attack_counter == 7)
                {
                    ph1_attack_counter = 0;
                    state = State.PH1_SA;
                }
                else
                {
                    CheckDistance();
                    StartCoroutine(WalkToTarget());
                }
                break;
            case State.Physical_Crush:
                StartCoroutine(MoveToTarget());
                break;
            case State.Crushed:
                break;
            case State.Physical_Bite:
                StartCoroutine(PhysicalBite());
                break;
            case State.Bited:
                break;
            case State.PH1_SA:
                StartCoroutine(PhaseOne_SpecialA());
                break;
            case State.PH1_SA_activated:
                StartCoroutine(PhaseOne_SpecialA_Activated());
                break;
            case State.Fire_Projectile:
                StartCoroutine(GenerateProjectiles());
                break;
            case State.Null:
                break;
        }
    }


    //Checking on gluttonys health
    void checkHealth()
    {
        if (health.currentHealth <= 5)
        {
            if (!projectileOnCooldown)
            {
                state = State.Fire_Projectile;
            }
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
        if ((Vector2.Distance(player.transform.position, this.gameObject.transform.position) <= bite_range) && (!biteOnCooldown))
        {
            state = State.Physical_Bite;
        }
    }

    Vector3 GetPathToPlayerPos()
    {
        Vector2 playerPos = new Vector3(player.transform.position.x, player.transform.position.y);
        playerPos.y += riseHeightOffset;
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
        // Increase Attack Counter for special attack
        ph1_attack_counter++;

        state = State.Crushed;
        pc.enabled = false;
        Vector2 playerPos = GetPathToPlayerPos();
        Vector2 currentPos = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        float rate = 1.0f / inverse_speed;
        for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime * rate)
        {
            transform.position = Vector2.Lerp(currentPos, playerPos, i);
            yield return null;
        }
        //Debug.Log("Move Finished");
        StartCoroutine(CrushShadow());
    }

    // Create a black shadow that will follow the player for few seconds. 
    // After few seconds, gluttony will try to crush at the shadow's position.
    IEnumerator CrushShadow()
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        playerPos.y += 5.0f;
        Instantiate(crush_shadow, playerPos, Quaternion.identity);
        yield return new WaitForSeconds(3);
        StartCoroutine(SlamDown());
    }

    IEnumerator SlamDown()
    {
        pc.enabled = true;
        BossCameraShake bossCamera = camera.GetComponent<BossCameraShake>();
        bossCamera.cameraShake(2.0f, 0.2f);
        Vector2 currentPos = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        Vector2 desiredPos = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        desiredPos.y -= fallHeightOffset;
        float rate = 1.0f/(inverse_speed / 10);
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime * rate) {
            transform.position = Vector2.Lerp(currentPos, desiredPos, i);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        //Debug.Log("Slam Finished");
        Instantiate(sin_item, this.gameObject.transform.position, Quaternion.identity);
        state = State.Walk;
    }

    // Mingun: This function is not going to be called.
    // Explaination: When glutton finishes the slam/crush,
    // It should go straight back to "Walk" state and follow the player.
    // However, this function makes the gluttony move vertically up for few seconds, then go back to the "Walk" state. 
    // I will leave this function for now for any future references. 
    /*IEnumerator MoveBackUp()
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
    }*/

    IEnumerator PhysicalBite()
    {
        // Increase Attack Counter for special attack
        ph1_attack_counter++;

        state = State.Bited;
        biteOnCooldown = true;
        //Debug.Log("Bite");
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);

        yield return new WaitForSeconds(1);
        Instantiate(bite, playerPos, Quaternion.identity);
        yield return null;

        yield return new WaitForSeconds(1);
        state = State.Walk;
        yield return new WaitForSeconds(3);
        biteOnCooldown = false;
    }

    IEnumerator PhaseOne_SpecialA()
    {
        //Debug.Log("SPECIAL ATTACK");
        state = State.PH1_SA_activated;
        currentPos = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        currentPos.y -= 6.0f;
        Instantiate(PhaseOne_attackRange, currentPos, Quaternion.identity);

        // Set back to walk state
        yield return new WaitForSeconds(5);
        state = State.Walk;
    }

    IEnumerator PhaseOne_SpecialA_Activated()
    {
        // Drag player to gluttony
        currentPos = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        PlayerMovement playerMov = player.GetComponent<PlayerMovement>();
        playerMov.DragPlayer(currentPos);
   
        yield return null;
    }

    IEnumerator GenerateProjectiles()
    {
        projectileOnCooldown = true;
        state = State.Null;
        //Move to center
        Vector2 arenaCenter = new Vector2(0, 0);
        Vector2 currentPos = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        float rate = 1.0f / inverse_speed;
        for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime * rate)
        {
            transform.position = Vector2.Lerp(currentPos, arenaCenter, i);
            yield return null;
        }
        //Spray Projectiles
        float dtheta = 45 * Mathf.PI / 180;
        for (int i = 0; i < 8; i++)
        {
            GameObject proj = Instantiate(projectile, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            Vector2 direction = new Vector2(Mathf.Cos(i * dtheta), Mathf.Sin(i * dtheta));
            proj.GetComponent<GluttonyProjectile>().initializeProjectile(direction);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1);
        state = State.Walk;
        //yield return null;
    }

    void DoActorDeath()
    {
        this.gameObject.SetActive(false);
    }
}
