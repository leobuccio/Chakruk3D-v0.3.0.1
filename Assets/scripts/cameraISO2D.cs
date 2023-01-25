using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraISO2D : MonoBehaviour {
    Vector3 originalPosition, originalRotation;
    Vector3 to2dPosition, to2dRotation;
    float originalCameraSize;
    float to2dCameraSize;
    float movementTime = 1;
    float t = 0, r;
    public bool ISO;
    bool lastISO;

    // Use this for initialization
    void Start() {
        originalPosition = new Vector3(-8.2f, 8.8f, -8.2f);
        originalRotation = new Vector3(35.35f, 45, 0);
        originalCameraSize = 6;
        to2dPosition = new Vector3(0f, 8.8f, 0f);
        to2dRotation = new Vector3(90, 45, 0);
        to2dCameraSize = 4;


        transform.position = originalPosition;
        transform.localEulerAngles = originalRotation;


    }

    // Update is called once per frame
    void Update() {
        if (lastISO != ISO) {
            t = 0;
        }

        if (!ISO)
        {
            cameraTo2D();
        }
        else {
            cameraToISO();
        }
        lastISO = ISO;

    }
    public void cameraTo2D()
    {
        t += Time.deltaTime / movementTime;
        r = Mathf.Log(t)*2;
        transform.position = Vector3.Lerp(originalPosition, to2dPosition, r);
        transform.localEulerAngles = Vector3.Lerp(originalRotation, to2dRotation, r);
        GetComponent<Camera>().orthographicSize = Mathf.Lerp(originalCameraSize, to2dCameraSize, r);
    }

    public void cameraToISO() {
        t += Time.deltaTime / movementTime;
        r = Mathf.Log(t)*2;
        transform.position = Vector3.Lerp(to2dPosition, originalPosition, r);
        transform.localEulerAngles = Vector3.Lerp(to2dRotation, originalRotation, r);
        GetComponent<Camera>().orthographicSize = Mathf.Lerp(to2dCameraSize, originalCameraSize, r);
    }
}
