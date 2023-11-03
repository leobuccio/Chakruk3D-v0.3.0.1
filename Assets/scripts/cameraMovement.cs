using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class cameraMovement : MonoBehaviour
{
    Animator myAnim;
    public GameObject camera2D, cameraIso;
    public GameObject center;
    [SerializeField] private panZoom _panZoom;
    public bool movingCamera;
    public float speedRotation = 5.0f;
    private Vector3 cameraOffset;
    [Range(0.01f, 1f)]
    public float smoothFactor = 0.5f;
    private bool goingBack;
    private Vector3 newPosition;
    
    bool setOneShootRotate = false;

    void Start()
    {
        myAnim = GetComponent<Animator>();
        cameraOffset = transform.position - center.transform.position;
        _panZoom = GetComponent<panZoom>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            cameraTo2d();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            cameraToISO();
        }

    }

    public void cameraTo2d()
    {
        myAnim.SetBool("ISO", false);
    }
    public void cameraToISO()
    {
        myAnim.SetBool("ISO", true);
    }
    public void cameraSwitchMode()
    {
        myAnim.SetBool("ISO", !myAnim.GetBool("ISO"));
    }

    public void camera2DOff()
    {
        camera2D.SetActive(false);
    }
    public void camera2DOn()
    {
        camera2D.SetActive(true);
    }
    public void cameraIsoDOff()
    {
        cameraIso.SetActive(false);
    }
    public void cameraIsoDOn()
    {
        cameraIso.SetActive(true);
    }

    private void LateUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            HandleAndroidInput();
        }
        else
        {
            HandlePCInput();
        }
        transform.LookAt(center.transform);
    }

    private void HandleAndroidInput()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            float touchDeltaX = Input.GetTouch(0).deltaPosition.x;
            Quaternion canTurnAngle = Quaternion.AngleAxis(touchDeltaX * speedRotation, Vector3.up);
            cameraOffset = canTurnAngle * cameraOffset;
            setOneShootRotate = true;

            newPosition = center.transform.position + cameraOffset;
            transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);
        }
        else
        {
            if (setOneShootRotate)
            {
                transform.DOMove(new Vector3(-8.22f, 9.41f, -8.22f), 1f);

                setOneShootRotate = false;
            }

            cameraOffset = transform.position - center.transform.position;
        }
    }
    

    private void HandlePCInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            movingCamera = !movingCamera;
        }

        if (movingCamera == true)
        {
            float mouseX = Input.GetAxis("Mouse X");
            Quaternion canTurnAngle = Quaternion.AngleAxis(mouseX * speedRotation, Vector3.up);
            cameraOffset = canTurnAngle * cameraOffset;
            setOneShootRotate = true;
            
           
            newPosition = center.transform.position + cameraOffset;
            transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);
        }
        else
        {
            if(setOneShootRotate)
            {
                transform.DOMove(new Vector3(-8.22f, 9.41f, -8.22f), 1f);
                
                setOneShootRotate = false;
            }
            
            cameraOffset = transform.position - center.transform.position;
        }
    }
}