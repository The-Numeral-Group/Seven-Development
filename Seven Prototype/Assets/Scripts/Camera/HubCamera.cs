using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubCamera : MonoBehaviour
{
    public Transform playerTransform;
    public List<Transform> POI;
    public Vector2 offset;
    public float minDistanceToPOI = 20f;
    public float maxZoom = 40;
    public float minZoom  = 17f;
    public float zoomSpeed = 5.0f;
    public float smoothTime = 0.5f;
    bool m_closeToPOI = false;
    private Vector3 velocity;
    private Camera cam;
    Vector3 m_closestPOI = Vector3.zero; //should only be referenced when closetoPOI is true
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveCamera();
        ZoomCamera();
    }

    void MoveCamera()
    {
        Vector3 cameraPosition = GetCenterPos();
        if (!m_closeToPOI) {
            cameraPosition = cameraPosition + new Vector3(offset.x, offset.y, cameraPosition.z);
        }
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(cameraPosition.x, cameraPosition.y, transform.position.z), ref velocity, smoothTime);
    }

    void ZoomCamera()
    {
        float newZoom;
        if (m_closeToPOI)
        {
            //float dist = Vector2.Distance(new Vector2(playerTransform.position.x, playerTransform.position.y), new Vector2(m_closestPOI.x, m_closestPOI.y));
            newZoom = Mathf.Lerp(cam.orthographicSize, minZoom, (zoomSpeed * Time.deltaTime));
            Debug.Log("In zoom");
        }
        else
        {
            newZoom = Mathf.Lerp(cam.orthographicSize, maxZoom, (zoomSpeed * Time.deltaTime));
        }
        cam.orthographicSize = newZoom;
    }

    Vector3 GetCenterPos()
    {
        //Get the position where the camera should be. If points of interest are nearby, adjust camera accordingly
        var bounds = new Bounds(playerTransform.position, Vector3.zero);
        bounds.Encapsulate(playerTransform.position);
        m_closeToPOI = false;
        float closestVal = minDistanceToPOI;
        for (int i = 0; i < POI.Count; i++)
        {
            float distToPoi = Vector2.Distance(new Vector2(playerTransform.position.x, playerTransform.position.y)
                                                    , new Vector2(POI[i].position.x, POI[i].position.y));
            //Prioritize framing the closest point of interest to the player
            if (distToPoi <= minDistanceToPOI && distToPoi < closestVal)
            {
                m_closeToPOI = true;
                closestVal = distToPoi;
                m_closestPOI = POI[i].position;
            }
        }
        if (m_closeToPOI)
        {
            bounds.Encapsulate(m_closestPOI);
        }
        return bounds.center;
    }
}
