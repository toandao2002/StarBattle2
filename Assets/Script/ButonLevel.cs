using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ButonLevel : MonoBehaviour
{
    public TMP_Text txtLevel;
    public TMP_Text TimeTxt;
    int timeFinish;
    int level;
    public void SetLevel(int id, int nameLevel)
    {
        txtLevel.text = nameLevel.ToString();
        level = id;
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
