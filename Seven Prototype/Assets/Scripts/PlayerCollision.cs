using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public string sceneToLoad;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SceneChange"))
        {
            Debug.Log("Scene Changed to BossScene");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
