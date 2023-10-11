using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_UIHandler : MonoBehaviour
{
    public GameObject thisObject;
    public bool isEnabled = true;

    public void ButtonClicked()
    {
        isEnabled = !isEnabled;
        thisObject.SetActive(isEnabled);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void GoToMainMenu ()
    {
        
    }
}
