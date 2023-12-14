using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BoxChoseLanguage : MonoBehaviour
{
    public string val;
    public TextMeshProUGUI text;
    public Image bgr;
    public void Init(string val , string nameLanguage)
    {
        this.val = val;
        this.text.text = nameLanguage;
    }
    public void UpdataLanguage()
    {
        MyLocalize.Instance.Select(val);
        BoxLanguage.instance.ShowPopUpChoseLanguage();
    }
}
