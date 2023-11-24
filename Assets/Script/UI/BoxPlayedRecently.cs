using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BoxPlayedRecently : MonoBehaviour
{
    public TMP_Text nameLevel;
    public TMP_Text dayPlay;
    public TMP_Text state;
    public TMP_Text timeFinish;
    int level;
    int timeFinishInt;
    TypeGame typeGame;
    public void SetData(string namelevel, string dayplay, string state, int timeFinish, int level, TypeGame typeGame)
    {
        this.nameLevel.text = namelevel ;
        this.dayPlay.text = dayplay ;
        this. state.text = state ;
        timeFinishInt = timeFinish;
        this.timeFinish.text = Util.SecondToTimeString(timeFinish);
        this.level = level;
        this.typeGame = typeGame;
    }
    public void PlayGame()
    {
         
        GameConfig.instance.SetLevelCurrent(level);
        GameConfig.instance.SetTimeFiishCurrent(timeFinishInt);
        GameConfig.instance.typeGame = typeGame;
        GameManger.instance.LoadScene("GamePlay");
       
    }
}
