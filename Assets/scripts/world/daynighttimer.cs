using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class daynighttimer : MonoBehaviour
{
    public float dayDuration = 900f;
    public float nightDuration = 600f;
    private float totalDayDuration;
    private float currenttime;

    public TMP_Text timertext;
    public bool isDaytime = true;
    void Start()
    {
        totalDayDuration = dayDuration + nightDuration;
        currenttime = 0;
    }

    // Update is called once per frame

    void Update()
    {
        updateTime();
    }

    private void DisplayTime()
    {
        float hour = isDaytime ? (currenttime/dayDuration) * 12f : 12f + (currenttime / nightDuration) * 12f;

        int displayHour = Mathf.FloorToInt(hour);
        int displayMinute = Mathf.FloorToInt((hour - displayHour) * 60);

        // Display time in text here tmp_settext

        timertext.text = $"{displayHour:00}:{displayMinute:00}";
    }


    private void updateTime()
    {
        currenttime += Time.deltaTime;
        
        if (isDaytime && currenttime >= dayDuration)
        {
            currenttime -= dayDuration;
            isDaytime = false;
            nightStart();

    
       
        }
        else if (!isDaytime && currenttime >= nightDuration)
        {
            currenttime -= nightDuration;
            isDaytime = true;
            dayStart();
        }

        DisplayTime();
    }

    private void dayStart()
    {
        
    }

    private void nightStart()
    {

    }

}
