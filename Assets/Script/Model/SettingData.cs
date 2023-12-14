using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SettingData 
{
    public bool vibrate = true;
    public float soundVolume = 1;
    public float musicVolume = 1;
    public bool autoDot = true;
    public int theme;
    public static SettingData GetSetting()
    {
        SettingData settingData = Util.ConvertStringToObejct<SettingData>(DataGame.GetDataJson(DataGame.SettingData));
      
        if(settingData == null)
        {
            settingData = new SettingData();
        }
        return settingData;
    }
    public void SaveSetingData()
    {
        Debug.Log(theme);
        string json = Util.ConvertObjectToString<SettingData>(this);
        DataGame.SetDataJson(DataGame.SettingData, json);
        DataGame.Save();
        MyEvent.UpdateSetingData?.Invoke();
        
    }
    public void TurnAutoDot()
    {
        autoDot = !autoDot;
        SaveSetingData();
    }
    public void TurnSound()
    {
        soundVolume = Mathf.Abs(1 - soundVolume);
        SaveSetingData();
    }
    public void TurnMusic()
    {
        musicVolume = Mathf.Abs(1 - musicVolume);
        SaveSetingData();
    }
    public void ChangeVibration()
    {
        vibrate = !vibrate;
        SaveSetingData();
    }
    public NameTheme ChangeTheme()
    {
        if(theme == 1)
        {
            theme = 0;
           
        }
        else
        {
            theme = 1;
        }
        SaveSetingData();
        return (NameTheme) theme;
    }
}
