using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coronar : MonoBehaviour
{
    private piece IAm;
    public Mesh myNewMesh;
    private GameObject blitzmode;

    // Use this for initialization
    void Start()
    {
        IAm = GetComponent<piece>();
        blitzmode = GameObject.Find("BlitzMode");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void checkCoronar()
    {
        if (IAm.myArmy == 1)
        {
            if ((IAm.posX == 0 && IAm.posY == 3) || (IAm.posX == 1 && IAm.posY == 5) || (IAm.posX == 2 && IAm.posY == 7) || (IAm.posX == 3 && IAm.posY == 8) || (IAm.posX == 5 && IAm.posY == 9) || (IAm.posX == 7 && IAm.posY == 10))
            {
                Coronado();
            }
            else if (blitzmode.GetComponent<blitzMode>().BlitzMode && (IAm.posX == 5 && IAm.posY == 5)) {
                Coronado();
            }

        }
        if (IAm.myArmy == 2)
        {
            if ((IAm.posX == 3 && IAm.posY == 0) || (IAm.posX == 5 && IAm.posY == 1) || (IAm.posX == 7 && IAm.posY == 2) || (IAm.posX == 8 && IAm.posY == 3) || (IAm.posX == 9 && IAm.posY == 5) || (IAm.posX == 10 && IAm.posY == 7))
            {
                Coronado();
            }
            else if (blitzmode.GetComponent<blitzMode>().BlitzMode && (IAm.posX == 5 && IAm.posY == 5))
            {
                Coronado();
            }

        }
    }
    void Coronado()
    {

        IAm.myType = 1;
        GetComponent<MeshFilter>().mesh = myNewMesh;
        //GetComponent<piece>().myCanvas.GetComponent<coronado>().cambiar(); //Esto es para cuando aparezcan las piezas comidas


    }
}
