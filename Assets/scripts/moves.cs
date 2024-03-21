﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moves : MonoBehaviour {
    public Sprite[] movesSprites;
    public Sprite[] subLoreSprites;
    private Sprite currentSprite = null;
    private bool spriteSetted = false;
    public GameObject Lore;
    public GameObject SubLore;

    public void changeSprite(int a)
    {
        GetComponent<Image>().sprite  = movesSprites[a];
        SubLore.GetComponent<Image>().sprite = subLoreSprites[a];
        currentSprite = movesSprites[a];
        spriteSetted = true;
    }

    public void ActivateLore()
    {
        Lore.GetComponent<SC_UIHandler>().ButtonClicked();
        if(spriteSetted)
        {
            Lore.GetComponent<Image>().sprite = currentSprite;
        }
    }
}
