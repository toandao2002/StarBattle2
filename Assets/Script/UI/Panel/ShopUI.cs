using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : BasePopUP
{
    public BoxBuyPack boxBuyPackPref;
    public Transform content;
    DataPack dataPack;
    public void RemoveGarbage()
    {
        for (int i = content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
    Action preAction;
    public override void Show(object data = null, int dir = 1)
    {
        base.Show(data, dir);
        RemoveGarbage();
        SetData();
        preAction = (Action)Delegate.Combine(MyEvent.ClickBack);
        MyEvent.ClickBack = Back;
    }
    public void Back()
    {
        Hide(-1);
        MyEvent.ClickBack = preAction;
    }
    private void OnEnable()
    {
        
    }
    public void SetData()
    {
        LevelCommon levelCommon = GameConfig.instance.levelCommon;
        dataPack = GameConfig.instance.GetDataPack();
        if (dataPack == null)
            dataPack = new DataPack();
        Debug.Log(DataGame.GetDataJson(DataGame.Datapack));
        int cnt = 0;
        bool canBuy = true;
        foreach (SubLevel s in levelCommon.levelEasy)
        {
            BoxBuyPack boxBuyPack = null;
            if (s.isNeedBuy && dataPack.CheckPackBeBought(TypeGame.Easy, cnt))
            {

            }
            else if (s.isNeedBuy && !dataPack.CheckPackBeBought(TypeGame.Easy, cnt) && canBuy)
            {
                canBuy = false;
                boxBuyPack = Instantiate(boxBuyPackPref, content);
                boxBuyPack.init(true, s, TypeGame.Easy, dataPack, cnt);
            }
            else if(s.isNeedBuy)
            {
                boxBuyPack = Instantiate(boxBuyPackPref, content);
                boxBuyPack.init(false, s, TypeGame.Easy, dataPack, cnt);
            }
            cnt++;

        }
        cnt = 0;
        canBuy = true;
        foreach (SubLevel s in levelCommon.levelMedium)
        {
            BoxBuyPack boxBuyPack = null;
            if (s.isNeedBuy && dataPack.CheckPackBeBought(TypeGame.Medium, cnt))
            {

            }
            else if (s.isNeedBuy && !dataPack.CheckPackBeBought(TypeGame.Medium, cnt) && canBuy)
            {
                canBuy = false;
                boxBuyPack = Instantiate(boxBuyPackPref, content);
                boxBuyPack.init(true, s, TypeGame.Medium, dataPack, cnt);
            }
            else if (s.isNeedBuy)
            {
                boxBuyPack = Instantiate(boxBuyPackPref, content);
                boxBuyPack.init(false, s, TypeGame.Medium, dataPack, cnt);
            }
            cnt++;

        }
        cnt = 0;
        canBuy = true;
        foreach (SubLevel s in levelCommon.levelDifficult)
        {
            BoxBuyPack boxBuyPack = null;
            if (s.isNeedBuy && dataPack.CheckPackBeBought(TypeGame.Difficult, cnt))
            {

            }
            else if (s.isNeedBuy && !dataPack.CheckPackBeBought(TypeGame.Difficult, cnt) && canBuy)
            {
                canBuy = false;
                boxBuyPack = Instantiate(boxBuyPackPref, content);
                boxBuyPack.init(true, s, TypeGame.Difficult, dataPack, cnt);
            }
            else if (s.isNeedBuy)
            {
                boxBuyPack = Instantiate(boxBuyPackPref, content);
                boxBuyPack.init(false, s, TypeGame.Difficult, dataPack, cnt);
            }
            cnt++;

        }
    }
}
