using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluttonyProjectile : MonoBehaviour
{
    Rigidbody2D rb;
    bool canMove = false;
    Vector3 movementDirection;

    public float force = 50f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(FreezeSelf());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        //Debug.Log(canMove);
        if (canMove)
        {
            rb.AddForce(movementDirection * force);
        }
    }

    public void initializeProjectile(Vector3 direction)
    {
        movementDirection = direction;
        canMove = true;
    }

    IEnumerator FreezeSelf()
    {
        float offset = Random.Range(1.5f, 2.0f);
        Debug.Log(offset);
        yield return new WaitForSeconds(offset);
        canMove = false;
        rb.velocity = Vector3.zero;
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(60);
        this.gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Gluttony" && other.gameObject.tag != "Gluttony Projectile")
        {
            this.gameObject.SetActive(false);
        }
    }
}
