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
        if (Input.GetMouseButtonDown(1))
        {

            movingCamera = !movingCamera;
        }

        if (movingCamera == true)
        {
            Quaternion canTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * speedRotation, Vector3.up);
            cameraOffset = canTurnAngle * cameraOffset;
        }
        if (movingCamera == true)
        {
            newPosition = center.transform.position + cameraOffset;
            transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);
        }
        else
        {
            transform.DOMove(new Vector3(-8.22f, 9.41f, -8.22f), 1f);
            //transform.DORotate(new Vector3(35f,45f,0f),1f);
            cameraOffset = transform.position - center.transform.position;
        }

        if (movingCamera == true)
        {
            //transform.LookAt(center.transform);
        }

    }

}