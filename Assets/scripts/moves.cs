using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moves : MonoBehaviour {
    public Sprite[] movesSprites;
    
    public void changeSprite(int a)
    {
        GetComponent<Image>().sprite = movesSprites[a];
    }
}
