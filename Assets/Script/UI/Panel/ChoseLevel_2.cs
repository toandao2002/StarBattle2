using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseLevel_2 : BasePopUP
{
    public TittleUI tittle;
    
    public static ChoseLevel_2 instance;
    public PopUpSuggestBuyPack popUpSuggestBuyPack;
    public override void Awake()
    {
        base.Awake();
        instance = this;
    }
    
    public ButonLevel btnLevelPrefab;
    public List<ButonLevel> btnLevels; 
    public GameObject content;
    public List<Level> levels;
    public GameObject BoxShopPref;
    public ButtonPlayNow buttonPlayNow;
    public void RemoveGarbage()
    {
       /* for(int  i=content.transform.childCount-1;i>=0; i--)
        {
            
        }*/
        DestroyImmediate(content.transform.GetChild(content.transform.childCount - 1).gameObject);
    }
    public override void Hide(int dir = 1)
    {
        base.Hide(dir);
        
      
        MyEvent.ClickBack -= Back;
    }
    public void show2()
    {
        Show();
    }
    public ButonLevel InitButtonLevel(int id)
    {
        if(id< btnLevels.Count)
        {
            btnLevels[id].gameObject.SetActive(true);
            return btnLevels[id];
        }
         
        ButonLevel btn = Instantiate(btnLevelPrefab, content.transform);
        btnLevels.Add(btn);
        return btn;
    }
    public void HideReduntantItem(int id)
    {
        for (int i = id; i < btnLevels.Count; i++)
        {
            if(btnLevels[i].gameObject.active == true)
                btnLevels[i].gameObject.SetActive(false);
            else
            {
                break;
            }
        }
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
            int levelFinishLast = 0;
            int timeFinishLast = 0;
            for (int i = 0; i < levels.Count; i++)
            {
                //btnLevels.Add(Instantiate(btnLevelPrefab, content.transform));
                ButonLevel btnLevel = InitButtonLevel(i);
                NameStateLevel nameStateLevel = NameStateLevel.Nothing;
                if (levels[i].datalevel.isfinished)
                {
                    nameStateLevel = NameStateLevel.Finished;
                    levelFinishLast = levels[i].nameLevel;
                    if (levels[i].datalevel != null)
                    {
                        timeFinishLast =  levels[i].datalevel.timeFinish;
                    }
                    else
                    {
                        timeFinishLast = 0;
                    }
                }
                else if (!levels[i].datalevel.isfinished && levels[i].datalevel.timeFinish > 0)
                {
                    nameStateLevel = NameStateLevel.Process;
                }
                btnLevel.SetLevel(levels[i].nameLevel, levels[i].nameLevel, nameStateLevel);
               
                if (levels[i].datalevel != null)
                {
                    btnLevel.SetTime(levels[i].datalevel.timeFinish);
                }
                else
                {
                    //btnLevels[i].SetTime(0);
                }
            }
            buttonPlayNow.SetLevel(levelFinishLast+1, timeFinishLast);
            HideReduntantItem(levels.Count);
            Instantiate(BoxShopPref, content.transform);
        }
        catch (Exception e)
        {
            Debug.LogError("Dont Get Data");
            Debug.LogError(e);
        }

        
        MyEvent.ClickBack = Back;
        
    }

    public void SetDataForBtnPlayNow()
    {
        
    }
    public void OpenLasTLevelCanPlay()
    {
        DataLevelUser dataLevelUser = GameConfig.instance.GetDataLevelCommon();
        int id = dataLevelUser.GetLevelPassByTypeGame(GameConfig.instance.typeGame);

        GameConfig.instance.SetLevelCurrent(id + 1);
        GameConfig.instance.nameModePlay = Util.GetLocalizeRealString(Util.GetIdLocalLizeTypeGame(GameConfig.instance.typeGame));
        GameConfig.instance.SetTimeFiishCurrent(GameConfig.instance.GetLevelCurrent().datalevel.timeFinish);
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.GamePlay);
        GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel1);
    }


    public void ChangeLanguage()
    {
        tittle.ShowTittle(Util.GetLocalizeRealString(Util.GetIdLocalLizeTypeGame(GameConfig.instance.typeGame)));
    }
    public void GetData()
    {
        List<SubLevel> subLevels = GameConfig.instance.GetCurrentSubs();
        ChangeLanguage();
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

    public void ShowPopUPSuggestBuyPack()
    {
        popUpSuggestBuyPack.Show();
    }

}
