using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance; 

    public GameObject localPlayer;
    public GameObject remotePlayer;

    void Awake()
    {
        instance = this;
    }
}
