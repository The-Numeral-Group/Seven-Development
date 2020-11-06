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
        if (other.CompareTag("SceneChange"))
        {
            Debug.Log("Scene Changed to BossScene");
            SceneManager.LoadScene(sceneToLoad);
        }
        if (other.CompareTag("Gluttony"))
        {
            Debug.Log("Collided");
            PlayerMovement playerMov = player.GetComponent<PlayerMovement>();
            TestBoss gluttonyMov = gluttony.GetComponent<TestBoss>();
            playerMov.velocity += Vector2.Scale(gluttonyMov.movementDirection, 10.0f * 
                new Vector2((Mathf.Log(1f / (Time.deltaTime * playerMov.Drag.x + 1)) / -Time.deltaTime),(Mathf.Log(1f / (Time.deltaTime * playerMov.Drag.y + 1)) / -Time.deltaTime)));
        }
    }
}
