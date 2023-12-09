using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public enum NamePopUp
{
    GameWin,
    Lobby,
    ChoseLevel1,
    ChoseLevel2,
    GamePlay,
    Tutoria,
    Shop,
}
public class BasePopUP : MonoBehaviour
{
   
    public NamePopUp namePopUp;
    public GameObject main;
    public CanvasGroup canvasGroup;
    public Image BgrMain; 
    public Image BgrMain2; 
    public bool isPopUp;
    protected float durationEffect = 0.3f;
    protected RectTransform rec;
    protected Ease ease = Ease.InOutQuart;
    public virtual void Awake()
    {
        MyEvent.ChangeTheme += ChangeTheme;
    }
    public virtual void Hide(int dir = 1) {

        if(BgrMain!= null)
        {
            BgrMain.fillAmount = 1;
        }
        if (rec == null)
        {
            rec = main.GetComponent<RectTransform>();
        }

        float posY = 120;
        if (dir == 1) // up
        {
            posY = 120;
        }
        else
        {
            if (BgrMain != null)
            {
                BgrMain.DOFillAmount(0, durationEffect).From(1).SetEase(ease);
            }
        }
        if(canvasGroup!= null)
            canvasGroup.DOFade(0, durationEffect).From(1).SetDelay(0.02f);

        rec.DOAnchorPos3DY(posY * dir, durationEffect).SetEase(ease).From(0).OnComplete(()=> {
            main.SetActive(false); 
        }); 
    }
    public virtual void Show(object data = null,int dir =1)
    {
        
        main.SetActive(true);
        if(rec== null)
        {
            rec = main.GetComponent<RectTransform>();
        }
        float posY =-120;
        if (dir == -1) // down
        {
            posY = -120;
            
        }
        else
        {
            if (BgrMain != null)
            {
                BgrMain.DOFillAmount(1, durationEffect).From(0).SetEase(ease);
            }
        }
        if (canvasGroup != null)
            canvasGroup.DOFade(1, durationEffect).From(0);
        rec.DOAnchorPos3DY(0, durationEffect).From(posY * dir).SetEase(ease);
        
    }
   
    public virtual void  ChangeTheme()
    {
        
    }
}
