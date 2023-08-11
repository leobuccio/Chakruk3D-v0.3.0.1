using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_OtherFunctions : MonoBehaviour
{
    public void Exit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
