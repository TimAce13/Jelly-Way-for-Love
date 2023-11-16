using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f0");

        if (minutes.Length < 2 && seconds.Length < 2)
            timerText.text = "0" + minutes + ":0" + seconds;
        else if (minutes.Length < 2)
            timerText.text = "0" + minutes + ":" + seconds;
        else
            timerText.text = minutes + ":" + seconds;
    }
}
