using UnityEngine;
using System.Collections;

public class CamShakeSimple : MonoBehaviour 
{

    Vector3 originalCameraPosition;

    float shakeAmt = 0;

    public Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void ShakeItBaby(float shakeStrength)
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        originalCameraPosition = mainCamera.transform.position;
        shakeAmt = shakeStrength * .0025f;
        InvokeRepeating("CameraShake", 0, .01f);
        Invoke("StopShaking", 0.1f);
    }

    void CameraShake()
    {
        
        if(shakeAmt>0) 
        {
            float quakeAmtX = Random.value*shakeAmt*2 - shakeAmt;
            float quakeAmtY = Random.value * shakeAmt * 2 - shakeAmt;
            Vector3 pp = mainCamera.transform.position;
            pp.y+= quakeAmtX;
            pp.x += quakeAmtY;// can also add to x and/or z
            mainCamera.transform.position = pp;
        }
    }

    void StopShaking()
    {
        CancelInvoke("CameraShake");
        mainCamera.transform.position = originalCameraPosition;
    }

}