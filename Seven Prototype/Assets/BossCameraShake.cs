using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// CREDIT: https://www.youtube.com/watch?v=Y8nOgEpnnXo&t=171s&ab_channel=Brackeys
// The following codes are from this video.
public class BossCameraShake : MonoBehaviour
{
    public Camera mainCam;
    float shakeAmount = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cameraShake(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void BeginShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = mainCam.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            camPos.x += offsetX;
            camPos.y += offsetY;

            mainCam.transform.position = camPos;

        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
        mainCam.transform.localPosition = Vector3.zero;
    }


}
