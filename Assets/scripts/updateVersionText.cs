using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateVersionText : MonoBehaviour {
    private Text myText;

	// Use this for initialization
	void Start () {
        myText = GetComponent<Text>();
        myText.text = Application.version;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
