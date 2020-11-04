using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoss : MonoBehaviour
{
    public float inverse_speed = 2;
    GameObject player;
    Rigidbody2D rb;
    BoxCollider2D bc;
    float heightOffset = 20;
    bool canMove = true;
    bool canAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate() 
    {
        if (canMove) {
            canMove = false;
            StartCoroutine(MoveToTarget());
        }
    }

    Vector3 GetPathToPlayerPos()
    {
        Vector2 playerPos = new Vector3(player.transform.position.x, player.transform.position.y);
        playerPos.y += heightOffset;
        return playerPos;
    }

    //https://answers.unity.com/questions/14279/make-an-object-move-from-point-a-to-point-b-then-b.html
    //answer by: philjhale
    IEnumerator MoveToTarget()
    {
        canMove = false;
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
        canMove = true;
        Debug.Log("Reset Finished");
    }

    void DoActorDeath() {
        this.gameObject.SetActive(false);
    }
}
