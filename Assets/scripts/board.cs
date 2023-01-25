using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board : MonoBehaviour {
    public GameObject MiPieza;
    public GameObject tablero;
    public int posX, posY;
    public int miOrden;
    private GameObject selectVisualHelp;
    private GameObject selectVisualHelp2;

    //[HideInInspector]
    public bool visualHelp;
    public bool visualHelp2;

	// Use this for initialization
	void Start () {
        miOrden = 10 * posX + posY;
        tablero = this.gameObject.transform.parent.gameObject;
        selectVisualHelp= this.gameObject.transform.GetChild(0).gameObject;
        selectVisualHelp2 = this.gameObject.transform.GetChild(1).gameObject;


    }

    // Update is called once per frame
    void Update () {
		
	}
    
    void OnMouseDown() {
        if (tablero.GetComponent<tablero>().piezaSostenida)
        {
            Debug.Log(tablero.GetComponent<tablero>().piezaSostenida.GetComponent<piece>().myArmy);
            if (selectVisualHelp.activeInHierarchy)
            {
                tablero.GetComponent<tablero>().Selection(posX, posY);
            }
            else if (MiPieza&&(tablero.GetComponent<tablero>().piezaSostenida.GetComponent<piece>().myArmy == MiPieza.GetComponent<piece>().myArmy))
            {
                tablero.GetComponent<tablero>().changePiece(posX, posY);

            }
        }
        else
        {
            tablero.GetComponent<tablero>().Selection(posX, posY);
        }
    }

    public void VisualHelpON()
    {
        selectVisualHelp.SetActive(true);
        visualHelp = true;
    }
    public void VisualHelpOFF()
    {
        visualHelp = false;
        visualHelp2 = false;
        selectVisualHelp.SetActive(false);
        selectVisualHelp2.SetActive(false);
    }
    public void VisualHelp2ON()
    {
        if (visualHelp == false)
        {
            selectVisualHelp2.SetActive(true);
            visualHelp2 = true;
        }
    }
    private void OnValidate()
    {
        gameObject.name = "Cubo-" + posX + "-" + posY;
    }
}
