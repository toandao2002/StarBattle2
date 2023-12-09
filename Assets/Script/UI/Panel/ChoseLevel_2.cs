using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseLevel_2 : BasePopUP
{
    public TittleUI tittle;
    public static ChoseLevel_2 instance;
    public override void Awake()
    {
        base.Awake();
        instance = this;
        MyEvent.BuyPack = () =>
        {
            Show();
        };
    }
    
    public ButonLevel btnLevelPrefab;
    public List<ButonLevel> btnLevels;
    public GameObject content;
    public List<Level> levels;
    public GameObject BoxShopPref;
    public void RemoveGarbage()
    {
        for(int  i=content.transform.childCount-1;i>=0; i--)
        {
            DestroyImmediate(content.transform.GetChild(i).gameObject);
        }
    }
    public override void Hide(int dir = 1)
    {
        base.Hide(dir);
        
        btnLevels = new List<ButonLevel>();
        MyEvent.ClickBack -= Back;
    }
    public override void Show(object data = null, int dir = 1)
    {
        base.Show(data, dir);
        try
        {
            /*levels = (List<Level>)data;*/
            content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            RemoveGarbage();
            GetData();
            ChangeTheme();

            for (int i = 0; i < levels.Count; i++)
            {
                btnLevels.Add(Instantiate(btnLevelPrefab, content.transform));
                NameStateLevel nameStateLevel = NameStateLevel.Nothing;
                if (levels[i].datalevel.isfinished) nameStateLevel = NameStateLevel.Finished;
                else if (!levels[i].datalevel.isfinished && levels[i].datalevel.timeFinish > 0)
                {
                    nameStateLevel = NameStateLevel.Process;
                }
                btnLevels[i].SetLevel(levels[i].nameLevel, levels[i].nameLevel, nameStateLevel);
                Debug.Log(btnLevels[i].txtLevel);
                if (levels[i].datalevel != null)
                {
                    btnLevels[i].SetTime(levels[i].datalevel.timeFinish);
                }
                else
                {
                    //btnLevels[i].SetTime(0);
                }
            }
            Instantiate(BoxShopPref, content.transform);
        }
        catch (Exception e)
        {
            Debug.LogError("Dont Get Data");
            Debug.LogError(e);
        }


        MyEvent.ClickBack = Back;
        
    }
    public void GetData()
    {
        List<SubLevel> subLevels = GameConfig.instance.GetCurrentSubs();
        tittle.ShowTittle(subLevels[0].nameSubLevel.Substring(0, subLevels[0].nameSubLevel.Length - 2));
        levels = new List<Level>();
        for(int i = 0; i< subLevels.Count; i++)
        {
            if (!subLevels[i].isNeedBuy)
            {
                levels.AddRange(subLevels[i].levels);
            }
            else if(subLevels[i].isNeedBuy && GameConfig.instance.GetDataPack().CheckPackBeBought(GameConfig.instance.typeGame, i))
            {
                levels.AddRange(subLevels[i].levels);
            }
        }
        
        string nameL = subLevels[0].nameSubLevel.Substring(0, subLevels[0].nameSubLevel.Length - 2);
       // ChoseLevel_2.instance.tittle.ShowTittle(nameL);
        GameConfig.instance.nameModePlay = nameL;
        for (int i = 0; i <  levels.Count; i++)
        {
            DataLevel datalevel = DataGame.GetDataLevel(GameConfig.instance.typeGame, levels[i].nameLevel);
            if (datalevel != null)
            {
                levels[i].datalevel = datalevel;
            }
            else
            {
                levels[i].datalevel = new DataLevel(DateTime.Now.ToString(), false, 0);
            }
        }
    }
    public void Back()
    {
        //  GameManger.instance.manageUi.ShowPopUp(NamePopUp.ChoseLevel1, 1, -1);
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.Lobby, 1, -1);
        GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel2, -1);
    }

    public Sprite bgrDark;
    public Sprite bgrLight;
    public override void ChangeTheme()
    {
        base.ChangeTheme();
        NameTheme theme = GameConfig.instance.nameTheme;
        if (theme == NameTheme.Dark)
        {
            BgrMain.sprite = bgrDark;
        }
        else
        {
            BgrMain.sprite = bgrLight;

        }
        if (BgrMain2 != null)
            BgrMain2.sprite = BgrMain.sprite;


    }

}
