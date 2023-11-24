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
    int level;
    public Image bgr;
    public Sprite spriteDone;
    public Sprite spriteProcess;
    public Sprite spriteNothing;
    public void SetLevel(int id, int nameLevel,NameStateLevel nameState)
    {
        txtLevel.text = nameLevel.ToString();
        level = id;
        switch (nameState)
        {
            case NameStateLevel.Finished:
                bgr.sprite = spriteDone;
                break;
            case NameStateLevel.Process:
                bgr.sprite = spriteProcess;
                break;
            case NameStateLevel.Nothing:
                bgr.sprite = spriteNothing;
                break;
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
        GameConfig.instance.SetLevelCurrent(level);
        GameConfig.instance.SetTimeFiishCurrent(timeFinish);
        GameManger.instance.LoadScene("GamePlay");
    }
}
