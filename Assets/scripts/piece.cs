
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class piece : pieces {
    public int myType;
    public int myArmy;
    public float multiplicador;
    public bool played;
    public bool checkThisPiece=true;
    public bool move;
    public Vector3 destino;
    public float velocidad;
    public GameObject myCanvas;
     private void OnValidate()
    {
        transform.localScale = Vector3.one *multiplicador;
    }
    void Update()
    {
        if (move) {
           //transform.position += (destino - transform.position) * 0.3f;// la velocidad es 0.3
                       transform.position = transform.position + (destino - transform.position) * 0.2f;// la velocidad es 0.3

            if ((transform.position - destino).sqrMagnitude < 0.01) {
                transform.position = destino;
                move = false;
            }
        }
    }
    public void prenderOutline() {
        StartCoroutine("OutlineOn");
    }
    public void morir() {
        if (myType != 2)
        {
            //myCanvas.SetActive(true);
        }
    }
    IEnumerator OutlineOn() {
        GetComponent<Outline>().enabled = true;
        yield return new WaitForSeconds(1);
        GetComponent<Outline>().enabled = false;



    }
    private void OnDestroy()
    {
        
    }
}
