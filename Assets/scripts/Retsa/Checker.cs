using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour {
    [Header("Properties")]
    [SerializeField]private int xPosition;
    [SerializeField]private int yPosition;
    [SerializeField]private GameObject greenHighlight;
    [SerializeField]private GameObject redHighlight;
    [SerializeField]private bool coronacion;

    public void turnOnGreenHighlight(){
        greenHighlight.SetActive(true);
    }

    public void turnOffGreenHighlight(){
        greenHighlight.SetActive(false);
    }

    public void turnOnRedHighlight(){
        redHighlight.SetActive(true);
    }

    public void turnOffRedHighlight(){
        redHighlight.SetActive(false);
    }

    public int getXPosition(){
        return xPosition;
    }

    public int getYPosition(){
        return yPosition;
    }

    void OnMouseDown(){
        Commander.instance.actOnChecker(this);
    }

    public bool getCoronacion()
    {
        return coronacion;
    }
}
