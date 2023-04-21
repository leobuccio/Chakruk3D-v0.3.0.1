using UnityEngine;

public class panZoom : MonoBehaviour
{
    public Camera MainCamera;
    [HideInInspector] public int zoomIncrement = 10;
    [HideInInspector] public float zoomOutMin = 1;
    [HideInInspector] public float zoomOutMax = 65;
    public Vector3 CameraStartingPosition;
    Touch touchZero, touchOne;
    [HideInInspector] public GameObject piece;
    [HideInInspector] public Transform pieceTransform;
    public int clicked;
    private float clicktime;
    private float clickdelay = 0.5f;

    private void Awake()
    {
        CameraStartingPosition = MainCamera.transform.position;
        zoomIncrement = 10;
        zoomOutMax = 65;
        zoomOutMin = 1;
    }
    // Update is called once per frame
    public void Update()
    {
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100);
        if (Input.touchCount == 2)
        {
            touchZero = Input.GetTouch(0);
            touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            //zoom(difference * 0.01f);
        }
        if (Input.GetMouseButtonDown(0))
        {

            clicked++;
            if (clicked == 1)
            {
                clicktime = Time.time;
            }
            if (clicked > 1 && Time.time - clicktime < clickdelay)
            {
                clicked = 0;
                clicktime = 0;
                Debug.Log("Double CLick");
                if (Physics.Raycast(ray, out hit) == true)
                {
                    Debug.Log("choco con " + hit.transform.name);
                    piece = hit.transform.gameObject;
                    pieceTransform = piece.transform;
                    transform.LookAt(pieceTransform);
                }
            }
            if (Time.time - clicktime > 1)
            {
                clicked = 0;
            }


        }
        zoom(Input.GetAxis("Mouse ScrollWheel") * zoomIncrement);
    }

    void zoom(float increment)
    {
        MainCamera.fieldOfView = Mathf.Clamp(MainCamera.fieldOfView - increment, zoomOutMin, zoomOutMax);
        MainCamera.transform.localPosition = new Vector3(MainCamera.transform.localPosition.x + (touchOne.position.x - touchZero.position.x) / 2, MainCamera.transform.localPosition.y + (touchOne.position.y - touchZero.position.y) / 2, MainCamera.transform.localPosition.z);
        // Debug.Log("Zoom" + increment);

    }
}

