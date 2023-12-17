using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCoroutine : MonoBehaviour
{
    TimeSpan timeSpan;
    long timeRun;
    public long er;
    public void DownTime()
    {

    }
    IEnumerator IeDownTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeRun++;
        }
    }
    public void GetTime()
    {
        timeSpan = TimeSpan.FromSeconds(timeRun);
        timeSpan.ToString(@"hh\:mm\:ss");

    }
    [Button]
    public void GetCurentDurationFromOneDate()
    {
        DateTime date;
        date = new DateTime(2023, 1, 12, 3, 3, 3);
        DateTime now = DateTime.Now;
        var x = now - date;
        Debug.Log(x. ToString());
    }
    public Convert 
}
