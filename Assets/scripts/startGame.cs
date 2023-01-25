using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startGame : MonoBehaviour {
    private AudioSource MyAS;
    public GameObject Loading;

	// Use this for initialization
	void Start () {
        MyAS = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	//void Update () {
 //       if (Input.anyKeyDown)
 //       {
 //       UnityEngine.SceneManagement.SceneManager.LoadScene("9 nivel");
 //       }
		
	//}
    public void StartClassic()
    {
        MyAS.Play();
        Loading.SetActive(true);
        StartCoroutine(LoadYourAsyncScene());

    }
    
    IEnumerator LoadYourAsyncScene()
    {

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
