using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{

    public Text timerLabel1, timerLabel2;
    public float StartTime;
    public bool playerOneTurn;
    private float timePlayer01,timePlayer02;
    private float minutes, seconds, fraction;

    private void Start()
    {
        playerOneTurn = true;
    }
    void Update()
    {
        if (playerOneTurn)
        {
            timePlayer01 += Time.deltaTime;
            minutes = (int)(timePlayer01 / 60);
            seconds = (int)timePlayer01 % 60;
            fraction = (int)(timePlayer01 * 100) % 100;
            timerLabel1.text = string.Format("{0:00}\n{1:00}\n{2:00}", minutes, seconds, fraction);
        }
        else {
            timePlayer02 += Time.deltaTime;
            minutes = (int)(timePlayer02 / 60);
            seconds = (int)timePlayer02 % 60;
            fraction = (int)(timePlayer02 * 100) % 100;
            timerLabel2.text = string.Format("{0:00}\n{1:00}\n{2:00}", minutes, seconds, fraction);

        }

    }
}
