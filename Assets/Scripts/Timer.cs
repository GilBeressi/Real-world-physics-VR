﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public TextMesh timerText;
    public float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time - startTime;

        string minutes = ((int) t / 60).ToString("f0");
        string seconds = (t % 60).ToString("f0");

        timerText.text = minutes + ":" + seconds;
    }
}
