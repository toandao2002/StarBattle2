using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TittleUI : MonoBehaviour
{
    public static TittleUI instacne;
    private void Awake()
    {
        instacne = this;
    }
    
    public Sprite back;
    public Sprite home;
    public Image spriteIconLeft;
    public MyButton LeftBtn;
    public TMP_Text nameTittle;
    public  void ShowTittle(NamePopUp namePopup, string nameT)
    {
        nameTittle.text = nameT;
        spriteIconLeft.gameObject.SetActive(true);

        switch (namePopup)
        {
            case NamePopUp.ChoseLevel1:
                SetIconLeft(home);
                
                break;
            case NamePopUp.Lobby:
                spriteIconLeft.gameObject.SetActive(false);
                break;
            default:
                SetIconLeft(back);
                break;

        }
    }
    public void SetActionIconLeft(Action call)
    {
        LeftBtn.onClick.RemoveAllListeners();
        LeftBtn.onClick.AddListener(() =>
        {
            call?.Invoke();
        });
    }
    public void SetIconLeft(Sprite sprite)
    {
        spriteIconLeft.sprite = sprite;
    }
    public void SetLevelTittle(int numlevel)
    {
        nameTittle.text = "Level : " + numlevel;
    }
}
