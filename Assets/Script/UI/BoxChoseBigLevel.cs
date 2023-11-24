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
                //btnLevels[i].SetTime(0);
            }
        }
        percent.text = cnt+"/"+ subLevel.levels.Count;
        bar.fillAmount = (float)cnt / subLevel.levels.Count;
    }
    public void OpenChoseLevel2()
    {
       
        TittleUI.instacne.ShowTittle(NamePopUp.ChoseLevel2, nameLevel);
        SetActionForBtnTitle();
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.ChoseLevel2,levels );

        GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel1);
    }
    public void SetActionForBtnTitle()
    {
        TittleUI.instacne.SetActionIconLeft(() => {
            TittleUI.instacne.ShowTittle(NamePopUp.ChoseLevel1, nameLevel.Substring(0,nameLevel.Length-2));
            GameManger.instance.manageUi.ShowPopUp(NamePopUp.ChoseLevel1,1,-1);
            GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel2,-1);
            TittleUI.instacne.SetActionIconLeft(() => {
                TittleUI.instacne.ShowTittle(NamePopUp.Lobby, "Home");
                GameManger.instance.manageUi.ShowPopUp(NamePopUp.Lobby,1,-1);
                GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel1,-1);

            });
        });
    }
    

}