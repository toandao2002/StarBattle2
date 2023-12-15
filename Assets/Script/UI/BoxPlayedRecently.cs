using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BoxPlayedRecently : MonoBehaviour
{
    public Image bgr;
    public Sprite bgrLight;
    public Sprite bgrDark;
    public TMP_Text nameLevel;
    public TMP_Text dayPlay;
    public TMP_Text state;
    public TMP_Text timeFinish;
    int level; 
    int timeFinishInt;
    TypeGame typeGame;
    private void OnEnable()
    {
        ChangeTheme();
        MyEvent.ChangeTheme += ChangeTheme;
        MyEvent.UpdataLocalize += UpdateLocalize;
    }
    private void OnDisable()
    {
        MyEvent.UpdataLocalize -= UpdateLocalize;
        MyEvent.ChangeTheme -= ChangeTheme;
    }
    private void ChangeTheme()
    {
        if(GameConfig.instance.nameTheme == NameTheme.Dark)
        {
            nameLevel.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
            dayPlay.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.Gray];
            state.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.Gray];
            timeFinish.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
            bgr.sprite = bgrDark;
        }
        else
        {
            nameLevel.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.Back];
            dayPlay.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.Gray];
            state.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.Gray];
            timeFinish.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.Gray];
            bgr.sprite = bgrLight;

        }


    }

    public void UpdateLocalize()
    {
        nameLevel.text = Util.GetLocalizeRealString(Util.GetIdLocalLizeTypeGame(this.typeGame)) + "-" + nameStringLevel;
    }
    string nameStringLevel;
    public void SetData( string nameLevel, string dayplay, string state, int timeFinish, int level,  TypeGame typeGame)
    {

        this.nameStringLevel = nameLevel;
        this.typeGame = typeGame; 
        UpdateLocalize();
        this.dayPlay.text = dayplay ;
        this. state.text = state ;
        timeFinishInt = timeFinish;
        this.timeFinish.text = Util.SecondToTimeString(timeFinish);
        this.level = level;
    }
    public void PlayGame()
    {
        GameConfig.instance.nameModePlay = nameLevel.text.Substring(0,nameLevel.text.Length -2);
        GameConfig.instance.typeGame = typeGame;
        GameConfig.instance.SetLevelCurrent(level);
        GameConfig.instance.SetTimeFiishCurrent(timeFinishInt);
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.GamePlay);
        GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel2);

    }
      
}
