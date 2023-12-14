using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GamePlayPanel : BasePopUP
{
   
    public List<Color> Bgr;
    public Image bgrBottom;
    public List<Color> bgrBottomColor;
    public List<TMP_Text> txt;

    public GameObject iconHint;
    private void OnEnable()
    {
        MyEvent.UpdateDataAmountHint += SetTextAmountHint;
    }
    private void OnDisable()
    {
        MyEvent.UpdateDataAmountHint -= SetTextAmountHint;
    }
    
    public override void Show(object data = null, int dir = 1)
    {
        base.Show(data, dir);
        ChangeTheme();
        SetTextAmountHint();
        MyEvent.ClickBack = Back;
        MyEvent.DontTouch += SuggestHint;
        MyEvent.ClickCell+= StopSuggestHInt;
    }
    public override void Hide(int dir = 1)
    {
        base.Hide(dir);
        MyEvent.ClickBack -= Back;
    }
    public override void ChangeTheme()
    {
        base.ChangeTheme();
        NameTheme theme = GameConfig.instance.nameTheme;
        if (theme == NameTheme.Dark)
        {
            BgrMain.color = Bgr[1];
            for(int i = 0; i < txt.Count; i++)
            {
                txt[i].color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
            }
            bgrBottom.color = bgrBottomColor[1];
        }
        else
        {
            BgrMain.color= Bgr[0];
            for (int i = 0; i < txt.Count; i++)
            {
                txt[i].color = GameConfig.instance.lightMode.colorText[(int)NameThemeText.Gray];
            }
            bgrBottom.color = bgrBottomColor[0];
        }
         
    }
    public void SuggestHint()
    {
        iconHint.transform.DOScale(0.7f, 0.6f).From(0.5f).SetEase(Ease.InOutQuart).SetLoops(-1,LoopType.Yoyo);
        
    }
    public void StopSuggestHInt()
    {
        iconHint.transform.DOKill();
        iconHint.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
    }
    public TMP_Text amountHint;
    int numHint = 0;
    public void SetTextAmountHint()
    {
        
        if (DataGame.CheckContain(DataGame.AmountHint))
        {
            numHint = DataGame.GetInt(DataGame.AmountHint);
        }
        else
        {
            numHint = 3;
            DataGame.SetInt(DataGame.AmountHint, 3);
            DataGame.Save();
        }
        amountHint.text = numHint + "";
    }
    public void CheckHint()
    {
        
        if (numHint == 0)
        {
            PopUpHint.instance.Show();
        }
        else
        {
            numHint -= 1;
            GameContrler.instance.Hint();
            DataGame.SetInt(DataGame.AmountHint, numHint  );
            DataGame.Save();
            
        }
        SetTextAmountHint();
    }
    public void Back()
    {
        GameManger.instance.manageUi.HidePopUP(NamePopUp.GamePlay,-1);
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.ChoseLevel2,1,-1);
    }
    public void Win()
    {
        MyEvent.GameWin?.Invoke(null);
    }
}
