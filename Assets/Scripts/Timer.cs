using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public Text TimeCounter;

    private TimeSpan TimePlaying;
    private bool TimerGoing;
    private float ElapsedTime;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        TimeCounter.text = "00:00.00";
        TimerGoing = false;
    }

    public void BeginTimer()
    {
        TimerGoing = true;
        ElapsedTime = 0f;
        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        TimerGoing = false;
    }

    public void ResumeTimer()
    {
        TimerGoing = true;
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while (TimerGoing)
        {
            ElapsedTime += Time.deltaTime;
            TimePlaying = TimeSpan.FromSeconds(ElapsedTime);
            string TimePlayStr = TimePlaying.ToString("mm':'ss'.'ff");
            TimeCounter.text = TimePlayStr;

            yield return null;
        }
    }
}
