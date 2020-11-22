using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CREDIT FOR THIS BASIC CODE TO BRACKEYS
//https://www.youtube.com/watch?v=aLpixrPvlB8
public class BossCamera : MonoBehaviour
{
    public Transform playerTransform;
    public List<GameObject> targets;
    public float max_distance = 30f;
    private Vector3 velocity;
    // Start is called before the first frame update
    public Vector2 offset = new Vector2(0, 2);
    public float smoothTime = 0.3f;
    private Vector2 currOffset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate() //Changed from lateupdate to remove stuttering
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        currOffset = offset;
        Vector2 centerPoint = GetCenterPoint();
        Vector2 newPosition = centerPoint + currOffset;
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
                if (!targets[i].activeSelf)
                {
                    continue;
                }
                float target_dist = Vector2.Distance(new Vector2(playerTransform.position.x, playerTransform.position.y)
                                                    , new Vector2(targets[i].transform.position.x, targets[i].transform.position.y));
                if (target_dist < max_distance)
                {
                    bounds.Encapsulate(targets[i].transform.position);
                    currOffset = Vector2.zero;
                }
            }
        }
        return bounds.center;
    }

}
