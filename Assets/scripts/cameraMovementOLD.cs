using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovementOLD : MonoBehaviour {
    Animator myAnim;
    public GameObject camera2D, cameraIso;
	// Use this for initialization
	void Start () {
        myAnim = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.U))
        {
            cameraTo2d();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            cameraToISO();
        }

    }
		
    public void cameraTo2d() {
        myAnim.SetBool("ISO", false);
    }
    public void cameraToISO()
    {
        myAnim.SetBool("ISO", true);
    }
    public void cameraSwitchMode() {
        myAnim.SetBool("ISO", !myAnim.GetBool("ISO"));
    }

    public void camera2DOff() {
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


}
