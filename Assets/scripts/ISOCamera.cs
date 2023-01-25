using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISOCamera : MonoBehaviour {
    public float myOriginalSize, mySize;
    public Vector3 myOriginalPosition, myDestinationPosition;
    public bool zooming;
    private float t;
	// Use this for initialization
	void Start () {
        myOriginalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        myDestinationPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        myOriginalSize = GetComponent<Camera>().orthographicSize;
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!zooming)
        {

        }
        else
        {
            ResetZoom();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            //ResetZoom();
        }
            t += Time.deltaTime;
            t = Mathf.Clamp01(t);
            transform.position = new Vector3(myOriginalPosition.x + t * (myDestinationPosition.x - myOriginalPosition.x), myOriginalPosition.y + t * (myDestinationPosition.y - myOriginalPosition.y), myOriginalPosition.z + t * (myDestinationPosition.z - myOriginalPosition.z));
            GetComponent<Camera>().orthographicSize += t * (mySize - GetComponent<Camera>().orthographicSize);
		
	}
    public void ResetZoom() {
        mySize = myOriginalSize;
        myDestinationPosition = myOriginalPosition;
        t = 0;
    }
}
