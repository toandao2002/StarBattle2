using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util 
{
    public static string SecondToTimeString(int time)
    {
        var timeSpan = TimeSpan.FromSeconds(time);

        return  timeSpan.ToString(@"hh\:mm\:ss");
        
    }
    public static string ConvertObjectToString<T>(T data)
    {
        string rs =  JsonUtility.ToJson(data);
        return rs;
    }
}
