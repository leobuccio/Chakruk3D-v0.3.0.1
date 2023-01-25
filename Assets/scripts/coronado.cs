using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class coronado : MonoBehaviour {

    public Sprite myImageCoronada;

    public void cambiar() {
        GetComponent<Image>().sprite = myImageCoronada;
    }
}
