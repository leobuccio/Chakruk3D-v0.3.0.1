using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panZoom : MonoBehaviour {
    public Camera MainCamera;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
    public Vector3 CameraStartingPosition;
    Touch touchZero, touchOne;

    private void Awake()
    {
        CameraStartingPosition = MainCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
        {
           touchZero = Input.GetTouch(0);
           touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.01f);
        }
        //else if (Input.GetMouseButton(0))
        //{
        //    Vector3 direction = touchStart - MainCamera.ScreenToWorldPoint(Input.mousePosition);
        //    Camera.main.transform.position += direction;
        //}
        //zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void zoom(float increment)
    {
        MainCamera.orthographicSize = Mathf.Clamp(MainCamera.orthographicSize - increment, zoomOutMin, zoomOutMax);
        MainCamera.transform.localPosition = new Vector3(MainCamera.transform.localPosition.x+(touchOne.position.x - touchZero.position.x)/2, MainCamera.transform.localPosition.y+(touchOne.position.y - touchZero.position.y)/2,MainCamera.transform.localPosition.z);
    }
}
