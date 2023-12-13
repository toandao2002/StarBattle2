using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : BasePopUP
{
    public static Lobby instance;
    private TypeGame typeGame;

    public BoxPlayedRecently BoxPlayedRecentlyPref;
    public GameObject contennt;
    public List<BoxPlayedRecently> boxPlayedRecentlies;
    public List<BoxBigLevelHome> boxBigLevelHomes;
    public GameObject content;
    public override void Awake()
    {
        base.Awake();
        ChangeTheme();
        instance = this;
    }
    private void Start()
    {
        HandelData();
    }
    public void RemoveGarbage()
    {
        for (int i = content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
    public override void Show(object data = null, int dir = 1)
    {
        base.Show(data, dir);
        HandelData();
        ChangeTheme();

    }
    private void OnEnable()
    {
        ChangeTheme();
    }
    public void HandelData()
    {
        RemoveGarbage();
        DataLevelUser dataLevelComon = GameConfig.instance.GetDataLevelCommon();

        foreach (BoxBigLevelHome i in boxBigLevelHomes)
        {
            List<SubLevel> subLevels = GameConfig.instance.GetSubsLevelByTypeGame(i.typeGame);
            int numLevel = 0;
            foreach (var j in subLevels)
            {
                numLevel += j.GetAmountLevel();
            } 
            i.SetRateBarLevel((float)dataLevelComon.GetLevelPassByTypeGame(i.typeGame) / numLevel);
        }


        HistoryPlayed historyPlayed = null;
        try
        {
            historyPlayed = JsonUtility.FromJson<HistoryPlayed>(DataGame.GetDataJson(DataGame.History));

        }
        catch
        {
            Debug.Log("Dont find history");
        }
        if (historyPlayed!= null &&historyPlayed.historys != null)
        {
            for (int i = historyPlayed.historys.Count - 1; i >= 0; i--)
            {
                var obj = Instantiate(BoxPlayedRecentlyPref, contennt.transform);
                 
                try
                { 
                    obj.SetData(historyPlayed.historys[i].typeGame.ToString().ToString(historyPlayed.historys[i].typeGame) + "-" + historyPlayed.historys[i].nameLevel, historyPlayed.historys[i].dayPlay,
                    historyPlayed.historys[i].isfinished == true ? "Finished" : "Process", historyPlayed.historys[i].timeFinish
                     , historyPlayed.historys[i].nameLevel, historyPlayed.historys[i].typeGame
                    );
                    boxPlayedRecentlies.Add(obj);
                }
                catch
                {
                    Debug.LogError("Loi game gan day");
                }


            }
        }
    }
    public void OpenLevelEasy()
    {
        typeGame = TypeGame.Easy;  
        HandleChangePanel();
    }
    public void OpenLevelMedium()
    {
        typeGame = TypeGame.Medium;  
        HandleChangePanel();


    }
    public void OpenLevelDifficut()
    {

        typeGame = TypeGame.Difficult;  
        HandleChangePanel();

    }
    public void OpenLevelGenius()
    { 
        typeGame = TypeGame.Genius; 
        HandleChangePanel();

    }
    public void HandleChangePanel()
    {
        GameConfig.instance.SetTypeGame(typeGame);
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.ChoseLevel2);
        Hide();
    }
 

    #region change theme

    [Header("Bgr------------------")]
    public Sprite bgligt;
    public Sprite bgdark;
    [Header("Recently------------")]
    public Image bgrRecentlyPlayed;
    public Sprite bgrRecentlyPlayedWhite;
    public Sprite bgrRecentlyPlayedDark;

    public Image bar;
    public Sprite barDark;
    public Sprite barLight;
 
    public TMP_Text titleRecently;
    public override void ChangeTheme()
    {
        base.ChangeTheme();
        NameTheme theme = GameConfig.instance.nameTheme;
        if(theme == NameTheme.Dark)
        {
            bgrRecentlyPlayed.sprite = bgrRecentlyPlayedDark;
            titleRecently.color = GameConfig.instance.darkMode.colorText[(int)TextColor.White];
            BgrMain.sprite = bgdark;
            bar.sprite = barDark;
        }
        else
        {
            bgrRecentlyPlayed.sprite = bgrRecentlyPlayedWhite;
            titleRecently.color = GameConfig.instance.darkMode.colorText[(int)TextColor.Black];
            BgrMain.sprite = bgligt;
            bar.sprite = barLight;
        }
        if (BgrMain2 != null)
            BgrMain2.sprite = BgrMain.sprite;

    }
    #endregion  


}
