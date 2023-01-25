using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenPrefabs : MonoBehaviour
{
    public static QueenPrefabs instance;

    [SerializeField] GameObject[] queens;

    private void Awake()
    {
        instance = this;    
    }

    public GameObject[] getQueens()
    {
        return queens;
    }

}
