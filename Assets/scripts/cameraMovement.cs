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
    public bool movingCamera;
    public float speedRotation = 5.0f;
    private Vector3 cameraOffset;
    [Range(0.01f, 1f)]
    public float smoothFactor = 0.5f;
    private bool goingBack;
    private Vector3 newPosition;
    private bool isMultiTouching = false;
    private Vector2 touchStartPos1;
    private Vector2 touchStartPos2;
    private Vector2 touchEndPos1;
    private Vector2 touchEndPos2;

    void Start()
    {
        myAnim = GetComponent<Animator>();
        cameraOffset = transform.position - center.transform.position;
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
            HandleTouchInput();
        }
        else
        {
            HandlePCInput();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                isMultiTouching = true;
                touchStartPos1 = touch1.position;
                touchStartPos2 = touch2.position;
            }
            else if ((touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved) && isMultiTouching)
            {
                touchEndPos1 = touch1.position;
                touchEndPos2 = touch2.position;

                Vector2 delta1 = touchEndPos1 - touchStartPos1;
                Vector2 delta2 = touchEndPos2 - touchStartPos2;

                float horizontalInput = (delta1.x + delta2.x) * 0.5f * speedRotation * 0.01f;

                Quaternion canTurnAngle = Quaternion.AngleAxis(horizontalInput, Vector3.up);
                cameraOffset = canTurnAngle * cameraOffset;
                transform.LookAt(center.transform);

                newPosition = center.transform.position + cameraOffset;
                transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

                touchStartPos1 = touchEndPos1;
                touchStartPos2 = touchEndPos2;
            }
            else if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
            {
                isMultiTouching = false;
            }
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
            transform.LookAt(center.transform);

            newPosition = center.transform.position + cameraOffset;
            transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);
        }
        else
        {
            transform.DOMove(new Vector3(-8.22f, 9.41f, -8.22f), 1f);
            // transform.DORotate(new Vector3(35f, 45f, 0f), 1f);
            cameraOffset = transform.position - center.transform.position;
        }
    }
}