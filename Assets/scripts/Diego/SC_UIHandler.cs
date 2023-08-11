using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_UIHandler : MonoBehaviour
{
    public GameObject rightFrame;
    public bool isEnabled = true;
    
    public void ButtonClicked()
    {
        isEnabled = !isEnabled;
        rightFrame.SetActive(isEnabled);
    }
    public void ButtonPointed ()
    {
        isEnabled = !isEnabled;
        rightFrame.SetActive(isEnabled);
    }

    public void ButtonUnpointed()
    {
        isEnabled = !isEnabled;
        rightFrame.SetActive(isEnabled);
    }
}
