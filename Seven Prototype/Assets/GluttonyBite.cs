using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GluttonyBite : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(1);
        this.gameObject.SetActive(false);
    }

}
