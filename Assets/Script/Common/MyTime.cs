using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MyTime : MonoBehaviour
{
    public int timeUserDontTouch;
    private void OnEnable()
    {
        timeUserDontTouch = 0;
        MyEvent.ClickCell += RessetTimeUserDontTouch;
    }
    private void OnDisable()
    {
        MyEvent.ClickCell -= RessetTimeUserDontTouch;
    }
    public TMP_Text timeTxt;
    public int timeRun;
    public void CountTime(int fromTime )
    {
        timeRun = fromTime;
        StartCoroutine(IECountdown());
    }
    public void SetTime(int fromTime)
    {
        timeRun = fromTime;
        var timeSpan = TimeSpan.FromSeconds(timeRun);

        timeTxt.text = timeSpan.ToString(@"hh\:mm\:ss");
    }
    public void RessetTimeUserDontTouch()
    {
        timeUserDontTouch = 0;
    }
    IEnumerator IECountdown()
    {
      
        while (true)
        {
            var timeSpan = TimeSpan.FromSeconds(timeRun);

            timeTxt.text = timeSpan.ToString(@"hh\:mm\:ss"); 
            yield return new WaitForSeconds(1);
            timeRun++;
            timeUserDontTouch++; 
            if(timeUserDontTouch > 10)
            {
                MyEvent.DontTouch?.Invoke();
            }
        }
    }

}
