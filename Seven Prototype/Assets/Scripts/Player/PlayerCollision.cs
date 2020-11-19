using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public string sceneToLoad;
    GameObject player;
    GameObject gluttony;

    private void Awake()
    {
        player = GameObject.Find("Player");
        gluttony = GameObject.Find("Gluttony");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other);
        if (other.CompareTag("SceneChange"))
        {
            Debug.Log("Scene Changed to BossScene");
            SceneManager.LoadScene(sceneToLoad);
        }
        if (other.CompareTag("Gluttony"))
        {
            //Debug.Log("Collided");
            PlayerMovement playerMov = player.GetComponent<PlayerMovement>();
            ActorHealth playerHealth = player.GetComponent<ActorHealth>();
            TestBoss gluttonyMov = gluttony.GetComponent<TestBoss>();
            playerMov.velocity += Vector2.Scale(gluttonyMov.movementDirection, 10.0f * 
                new Vector2((Mathf.Log(1f / (Time.deltaTime * playerMov.Drag.x + 1)) / -Time.deltaTime),(Mathf.Log(1f / (Time.deltaTime * playerMov.Drag.y + 1)) / -Time.deltaTime)));

            if(gluttonyMov.state == TestBoss.State.Walk)
            {
                // reduces player health by 1 when gluttony is walking
                playerHealth.takeDamage(1);
            }
            else if (gluttonyMov.state == TestBoss.State.Crushed)
            {
                // reduces player health by 5 when gluttony is slamming/crushing
                playerHealth.takeDamage(3);
            }
            else if (gluttonyMov.state == TestBoss.State.PH1_SA_activated)
            {
                playerHealth.takeDamage(10);
            }
        }
        if (other.CompareTag("Gluttony Projectile"))
        {
            ActorHealth playerHealth = player.GetComponent<ActorHealth>();
            playerHealth.takeDamage(3);
        }
        if (other.CompareTag("Gluttony Bite"))
        {
            ActorHealth playerHealth = player.GetComponent<ActorHealth>();
            playerHealth.takeDamage(1);
        }
        
    }
}
