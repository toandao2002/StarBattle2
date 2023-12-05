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

    public Image Bgr;
    public List<Color> bgrColors;
    public void OpenSeting()
    {
        SettingUI.instance.Show();
    }
    
    private void OnEnable()
    {
        MyEvent.ChangeTheme += changeTheme;
        changeTheme();
    }
    private void OnDisable()
    {
        MyEvent.ChangeTheme -= changeTheme;
    }
    public void changeTheme()
    {
        NameTheme theme = GameConfig.instance.nameTheme;
        if (theme == NameTheme.Dark)
        {
            Bgr.color = bgrColors[1];
            nameTittle.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
        }
        else
        {
            Bgr.color = bgrColors[0];
            nameTittle.color = GameConfig.instance.lightMode.colorText[(int)NameThemeText.Back];


        }
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
