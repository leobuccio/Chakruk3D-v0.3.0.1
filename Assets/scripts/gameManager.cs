using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            QuitGame();
        }
		
	}
    public void resetLevel() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
    public void QuitGame() {
        Application.Quit();
    }

}
