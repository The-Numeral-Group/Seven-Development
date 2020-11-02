using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera), typeof(Transform))]
public class CameraDirector : MonoBehaviour
{
    public Transform player;
    //private Transform localTransform;

    // Start is called before the first frame update
    void Start()
    {
        //localTransform = this.gameObject.transform;
        //localTransform.SetParent(originalTarget);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    /*public void SwitchTarget(Transform newTarget){
        localTransform.SetParent(newTarget);
    }*/

    //one day I'm going to add methods to switch primaryTarget with methods
    //and maybe a "slide to here, look at it, then go back" method.
}
