using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluttonyProjectile : MonoBehaviour
{
    Rigidbody2D rb;
    bool canMove = false;
    Vector2 movementDirection;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroySelf());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.AddForce(movementDirection * 50);
        }
    }

    public void initializeProjectile(Vector2 direction)
    {
        movementDirection = direction;
        canMove = true;
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(3);
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
