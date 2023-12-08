using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxSetting : MonoBehaviour
{
    public Switch mySwitch;
    public Image Bgr;
    public List<Sprite> Bgrs;
    public TMP_Text nametxt;
    private void OnEnable()
    {
        ChangeTheme();
        MyEvent.ChangeTheme += ChangeTheme;
    }
    private void OnDisable()
    {
        MyEvent.ChangeTheme -= ChangeTheme;
    }
    public virtual void ChangeTheme()
    {
        NameTheme theme = GameConfig.instance.nameTheme;
        if (theme == NameTheme.Dark)
        {
            Bgr.sprite = Bgrs[1];
            nametxt.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
        }
        else
        {
            Bgr.sprite = Bgrs[0];
            nametxt.color = GameConfig.instance.lightMode.colorText[(int)NameThemeText.Back];
        }
    }
}
