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
    public static T ConvertStringToObejct<T>(string json)
    { 
        T obj = JsonUtility.FromJson<T>(json);
        return obj;
    }

    public static string GetIdLocalLizeTypeGame (TypeGame typeGame)
    {
        switch (typeGame)
        {
            case TypeGame.Difficult:
                return Loc.ID.Common.ExpertText;
            case TypeGame.Medium:
                return Loc.ID.Common.AdvancedText;
            case TypeGame.Easy:
                return Loc.ID.Common.BeginnerText;
            case TypeGame.Genius:
                return Loc.ID.Common.GeniusText;

        }
        // other ones, just use the base method
        return Loc.ID.Common.BeginnerText;
    }
    public static string GetLocalizeRealString(string s)
    {
        return Wugner.Localize.Localization.GetEntry(null, s, "").Content;
    }
}
