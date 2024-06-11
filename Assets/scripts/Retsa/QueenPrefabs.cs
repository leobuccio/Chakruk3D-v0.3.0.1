using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenPrefabs : MonoBehaviour
{
    public static QueenPrefabs Instance;

    [SerializeField] GameObject[] queens;

    private void Awake()
    {
        Instance = this;    
    }

    public GameObject[] getQueens()
    {
        return queens;
    }

}
