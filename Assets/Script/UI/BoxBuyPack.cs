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
    private TypeGame typeGame;
    private DataPack dataPack;
    private int idPack;
    public void init(bool canBuy, SubLevel subLevel, TypeGame typeGame, DataPack datapack, int idPack)
    {
        this.idPack = idPack;
        this.dataPack = datapack;
        this.typeGame = typeGame;
        this.canBuy = canBuy;
        this.sublevel = subLevel;
        namePack.text = typeGame.ToString() +" "+ (idPack + 1);
        contecnPack.text = sublevel.nameSubLevel;
        if (canBuy)
        {
            textInBtnBuy.text = "$100";
            iconLocked.gameObject.SetActive(false);
        }
        else
        {
            iconLocked.gameObject.SetActive(true);

            textInBtnBuy.text = "Locked";
        }
    }
    public void Buy()
    {
        if(canBuy)
        {
            Debug.Log("Buy ");
           dataPack.BuyBack(typeGame, idPack);
            MyEvent.BuyPack?.Invoke();
        }
        else
        {
            Debug.Log("cant buy this pack");
        }
    }
}
