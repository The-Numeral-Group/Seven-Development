using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluttonyCrushShadow : MonoBehaviour
{
    GameObject player;
    GameObject gluttony;
    bool freezePos = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gluttony = GameObject.Find("Gluttony");
        StartCoroutine(freezePosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (!freezePos)
        {
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 5.0f);
            gluttony.transform.position = new Vector2(player.transform.position.x, this.gameObject.transform.position.y + 120.0f);
        }
    }

    IEnumerator freezePosition()
    {
        yield return new WaitForSeconds(2);
        freezePos = true;
        StartCoroutine(DestorySelf());
    }

    IEnumerator DestorySelf()
    {
        yield return new WaitForSeconds(1);
        this.gameObject.SetActive(false);
    }
}
