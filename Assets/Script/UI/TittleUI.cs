using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TittleUI : MonoBehaviour
{ 
     
     
    public Image spriteIconLeft;
    public MyButton LeftBtn;
    public TMP_Text nameTittle;
    public void OpenSeting()
    {
        SettingUI.instance.Show();
    }
    public  void ShowTittle( string nameT)
    {
        nameTittle.text = nameT;
        
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
    public void SetLevelTittle(string numlevel)
    {
        nameTittle.text =    numlevel;
    }
}
