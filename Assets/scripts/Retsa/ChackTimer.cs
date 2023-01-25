using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChackTimer : MonoBehaviour
{
    [SerializeField] Text timerLabel;
    
    private float timePlayer = 0;
    private float minutes, seconds, fraction;
    private bool isRunning = false;

    void Update()
    {
        if (isRunning)
            Run();
    }

    void Run()
    {
        timePlayer += Time.deltaTime;
        minutes = (int)(timePlayer / 60);
        seconds = (int)timePlayer % 60;
        fraction = (int)(timePlayer * 100) % 100;
        timerLabel.text = string.Format("{0:00}\n{1:00}\n{2:00}", minutes, seconds, fraction);
    }

    public void SetIsRunning(bool value) {
        isRunning = value;
    }
    
}
