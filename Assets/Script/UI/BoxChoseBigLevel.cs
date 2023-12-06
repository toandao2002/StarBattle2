using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxChoseBigLevel : MonoBehaviour
{
    public TMP_Text txtLevel;
    public string nameLevel;
    public int NumLevelIn;
    public List<Level> levels;
    public Image bar;
    public TMP_Text percent;
     
    private void OnEnable()
    {
        ChangeTheme();
        MyEvent.ChangeTheme += ChangeTheme;
    }
    private void OnDisable()
    {
        MyEvent.ChangeTheme -= ChangeTheme;
    }
    public void InitBox(SubLevel subLevel)
    {
        nameLevel = subLevel.nameSubLevel;
        txtLevel.text = subLevel.nameSubLevel;
        NumLevelIn = subLevel.levels.Count;
        levels = subLevel.levels;
        int cnt = 0;
        for (int i = 0; i < levels.Count; i++)
        {
            DataLevel datalevel = DataGame.GetDataLevel(GameConfig.instance.typeGame, levels[i].nameLevel);
            if (datalevel != null)
            {
                levels[i].datalevel = datalevel;
                if (levels[i].datalevel.isfinished) cnt++;
            }
            else
            {
                levels[i].datalevel = new DataLevel(DateTime.Now.ToString(), false, 0);
                //btnLevels[i].SetTime(0);
            }
        }
        percent.text = cnt+"/"+ subLevel.levels.Count;
        bar.fillAmount = (float)cnt / subLevel.levels.Count;
    }
    public void OpenChoseLevel2()
    {
        
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.ChoseLevel2,levels );
        ChoseLevel_2.instance.tittle.ShowTittle(nameLevel);
        GameConfig.instance.nameModePlay = nameLevel.Substring(0,nameLevel.Length-2);
        GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel1);
    }

    public void OpenLasTLevelCanPlay()
    {
        DataLevelUser dataLevelUser = GameConfig.instance.GetDataLevelCommon();
        int id = dataLevelUser. GetLevelPassByTypeGame(GameConfig.instance.typeGame);

        GameConfig.instance.SetLevelCurrent(id+1);
        GameConfig.instance.nameModePlay = nameLevel.Substring(0, nameLevel.Length - 2);
        GameConfig.instance.SetTimeFiishCurrent(GameConfig.instance.GetLevelCurrent().datalevel.timeFinish);
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.GamePlay);
        GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel1);
    }
    #region change theme 

    public Image bgr;
    public List<Sprite> Bgrs;
     
    public void ChangeTheme()
    {
        NameTheme theme = GameConfig.instance.nameTheme;
        if (theme == NameTheme.Dark)
        {
            bgr.sprite = Bgrs[1];
            txtLevel.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
            percent.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
        }
        else
        { 
            bgr.sprite = Bgrs[0];
            txtLevel.color = GameConfig.instance.lightMode.colorText[(int)NameThemeText.Back];
            percent.color = GameConfig.instance.lightMode.colorText[(int)NameThemeText.Gray];
        }
    }

    #endregion
}
