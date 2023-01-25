using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blitzMode : MonoBehaviour
{
    private Text myText;
    public bool BlitzMode = true;

    // Use this for initialization
    void Start()
    {
        myText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {


    }
    public void ChangeBlitzMode()
    {
        BlitzMode = !BlitzMode;
        if (BlitzMode)
        {
            myText.text = "Blitz Mode On";
            myText.color = new Color(1, 1, 1, 1);
        }
        else
        {
            myText.text = "Blitz Mode Off";
            myText.color = new Color(1, 1, 1, 0.5f);
        }
    }
}
