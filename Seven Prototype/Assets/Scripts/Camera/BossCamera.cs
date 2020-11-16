using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CREDIT FOR THIS BASIC CODE TO BRACKEYS
//https://www.youtube.com/watch?v=aLpixrPvlB8
public class BossCamera : MonoBehaviour
{
    public Transform playerTransform;
    public List<Transform> targets;
    public float max_distance = 80f;
    private Vector3 velocity;
    // Start is called before the first frame update
    public Vector2 offset;
    public float smoothTime = 0.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate() //Changed from lateupdate to remove stuttering
    {
        Vector2 centerPoint = GetCenterPoint();
        Vector2 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(newPosition.x, newPosition.y, transform.position.z), ref velocity, smoothTime);
    }

    Vector2 GetCenterPoint()
    {
        var bounds = new Bounds(playerTransform.position, Vector3.zero);
        bounds.Encapsulate(playerTransform.position);
        if (targets.Count > 0)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                float target_dist = Vector2.Distance(new Vector2(playerTransform.position.x, playerTransform.position.y)
                                                    , new Vector2(targets[i].position.x, targets[i].position.y));
                if (target_dist < max_distance)
                {
                    bounds.Encapsulate(targets[i].position);
                }
            }
        }
        return bounds.center;
    }

}
