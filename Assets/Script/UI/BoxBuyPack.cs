using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BoxBuyPack : MonoBehaviour
{
    public SubLevel sublevel;
    public bool canBuy;
    public TMP_Text textInBtnBuy;
    public TMP_Text namePack;
    public TMP_Text contecnPack;
    public Image iconLocked;
    public Image bgrBtn;
    public Image bgr;
    public Sprite bgrBtnBlue;
    public Sprite bgrBtnGray;
    public List<Sprite> colorBgr;

    private TypeGame typeGame;
    private DataPack dataPack;
    private int idPack;
    private void OnEnable()
    {
        MyEvent.ChangeTheme += ChangeTheme;
        ChangeTheme();
    }
    private void OnDisable()
    {
        MyEvent.ChangeTheme -= ChangeTheme;
    }
    public string GetIdContentShop(TypeGame typeGame)
    {
        switch (typeGame)
        {
            case TypeGame.Difficult:
                return Loc.ID.Shop.GetLevelInModeExpert;
            case TypeGame.Medium:
                return Loc.ID.Shop.GetLevelInModeAdvanced;
            case TypeGame.Easy:
                return Loc.ID.Shop.GetLevelInModeBegginer;
            case TypeGame.Genius:
                return Loc.ID.Shop.GetLevelInModeGenius;

        }
        return "404";
    }
    public void init(bool canBuy, SubLevel subLevel, TypeGame typeGame, DataPack datapack, int idPack, int levelFrom, int levelEnd)
    {
        this.idPack = idPack;
        this.dataPack = datapack;
        this.typeGame = typeGame;
        this.canBuy = canBuy;
        this.sublevel = subLevel;
        namePack.text = Util.GetLocalizeRealString(Util.GetIdLocalLizeTypeGame(typeGame)) +" "+ (idPack + 1);
        string buf = Util.GetLocalizeRealString(GetIdContentShop(typeGame));
        
        int indexS = buf.LastIndexOf("0");
        buf = buf.Replace("0",  " ");
        buf = buf.Insert(indexS, levelFrom+"");
        int indexE = buf.LastIndexOf("1");

        buf = buf.Substring(0, indexE) + buf.Substring(indexE + 1);
        buf = buf.Insert(indexE, levelEnd +"");

        contecnPack.text = buf;
        if (canBuy)
        {
            textInBtnBuy.text = "$ 100";
            iconLocked.gameObject.SetActive(false);
            bgrBtn.sprite = bgrBtnBlue;
        }
        else
        {
            iconLocked.gameObject.SetActive(true);
            bgrBtn.sprite = bgrBtnGray;
            textInBtnBuy.text = Util.GetLocalizeRealString(Loc.ID.Shop.LockedText);
        }
    }
    public void Buy()
    {
        if(canBuy)
        {
            Debug.Log("Buy ");
            dataPack.BuyBack(typeGame, idPack);
            MyEvent.BuyPack?.Invoke();
            ShopUI.instance.popUpConfirm.Show();
        }
        else
        {
            Debug.Log("cant buy this pack");
        }
    }
    public  void ChangeTheme()
    {
        NameTheme theme = GameConfig.instance.nameTheme;
        if (theme == NameTheme.Dark)
        {
            bgr.sprite = colorBgr[1];
            contecnPack.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.Gray];

            namePack.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
        }
        else
        {
            contecnPack.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.Gray];

            bgr.sprite = colorBgr[0];
            namePack.color = GameConfig.instance.lightMode.colorText[(int)NameThemeText.Back];

        }


    }
}
