using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MyTime : MonoBehaviour
{
  

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

    IEnumerator IECountdown()
    {
      
        while (true)
        {
            var timeSpan = TimeSpan.FromSeconds(timeRun);

            timeTxt.text = timeSpan.ToString(@"hh\:mm\:ss"); 
            yield return new WaitForSeconds(1);
            timeRun++;
             
        }
    }

}
