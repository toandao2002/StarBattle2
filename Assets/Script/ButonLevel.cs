using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public enum NameStateLevel
{
    Finished,
    Process,
    Nothing,
}
public class ButonLevel : MonoBehaviour
{

    public TMP_Text txtLevel;
    public TMP_Text TimeTxt;
    int timeFinish;
    public int level;
    public Image bgr;
    public Sprite spriteDone;
    public Sprite spriteDoneDark;
    public Sprite spriteProcess;
    public Sprite spriteProcessDark;
    public Sprite spriteNothing; 
    public Sprite spriteNothingDark;
    private void OnEnable()
    {
        ChangeTheme();
        MyEvent.ChangeTheme += ChangeTheme;
    }
    private void OnDisable()
    {
        MyEvent.ChangeTheme -= ChangeTheme;
    }
    public void ChangeTheme()
    {
        bool darkMode = GameConfig.instance.nameTheme == NameTheme.Dark;
        switch (nameState)
        {
            case NameStateLevel.Finished:
                bgr.sprite = spriteDone;
                if (darkMode)
                {
                    bgr.sprite = spriteDoneDark;

                }
                break;
            case NameStateLevel.Process:
                bgr.sprite = spriteProcess;
                if (darkMode)
                {
                    bgr.sprite = spriteProcessDark;

                }
                break;
            case NameStateLevel.Nothing:
                bgr.sprite = spriteNothing;
                if (darkMode)
                {
                    bgr.sprite = spriteNothingDark;

                }
                break;
        }
        if (darkMode)
        {
            txtLevel.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
        }else
        {
            txtLevel.color = GameConfig.instance.lightMode.colorText[(int)NameThemeText.Back];

        }
    }
    NameStateLevel nameState;
    public void SetLevel(int id, int nameLevel,NameStateLevel nameState)
    {
        this.nameState = nameState;
        txtLevel.text = nameLevel.ToString();
        TimeTxt.gameObject.SetActive(false);
        level = id;
        bool darkMode = GameConfig.instance.nameTheme == NameTheme.Dark;
        switch (nameState)
        {
            case NameStateLevel.Finished:
                bgr.sprite = spriteDone;
                if (darkMode)
                {
                    bgr.sprite = spriteDoneDark;

                }
                break;
            case NameStateLevel.Process:
                bgr.sprite = spriteProcess;
                if (darkMode)
                {
                    bgr.sprite = spriteProcessDark;

                }
                break;
            case NameStateLevel.Nothing:
                bgr.sprite = spriteNothing;
                if (darkMode)
                {
                    bgr.sprite = spriteNothingDark;

                }
                break;
        }
        if (darkMode)
        {
            txtLevel.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
        }
        else
        {
            txtLevel.color = GameConfig.instance.lightMode.colorText[(int)NameThemeText.Back];

        }
    
    }
    public void SetTime(int  time)
    {
        if(time>0)
            TimeTxt.gameObject.SetActive(true);
        TimeTxt.text =Util. SecondToTimeString(time);
        timeFinish = time;
    }
    public void ChoseLevel()
    {
        DataLevelUser dataLevelUser = GameConfig.instance.GetDataLevelCommon();
        if (!dataLevelUser.CheckLevelFinish(GameConfig.instance.typeGame, level))
        {
            PopUpContinuePlay.instance.Show();
            return;
        }


        GameConfig.instance.SetLevelCurrent(level);
        GameConfig.instance.SetTimeFiishCurrent(timeFinish);
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.GamePlay);
        GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel2);
    }
}
