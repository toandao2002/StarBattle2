using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class HintMesageUI : MonoBehaviour
{
    public static HintMesageUI instance;
    public CanvasGroup cg;
    private void Awake()
    {
        instance = this;
        MyEvent.ChangeTheme += ChangeTheme;
       
    }
     

    public Image main;
    public TMP_Text txt; 
    public TMP_Text title;  
    public List<Color> colorBgr;
    public void ShowHint(TypeHint typeHint)
    {
        txt.text = GetDescription(typeHint);
        title.text = Util.GetLocalizeRealString(Loc.ID.GamePlay.Hint);

        main.gameObject.SetActive(true);
        cg.DOFade(1, 1 / 2).From(0);
        ChangeTheme();
        MyEvent.ClickCell += Hide;
    }
    public void ShowNotice(string val)
    {
        txt.text = val;
        title.text = Util.GetLocalizeRealString(Loc.ID.Common.Notification);
        main.gameObject.SetActive(true);
        cg.DOFade(1, 1 / 2).From(0);
        ChangeTheme();
        MyEvent.ClickCell += Hide;
    }
    public void Hide()
    {
        MyEvent.ClickCell -= Hide;
        cg.DOFade(0, 0.4f).From(1).OnComplete(() => {
            main.gameObject.SetActive(false);
        });
        
    }
    public void ChangeTheme()
    {
        NameTheme theme = GameConfig.instance.nameTheme;
        if (theme == NameTheme.Dark)
        {
            main.color = colorBgr[1];
            txt.color = GameConfig.instance.darkMode.colorText[(int)TextColor.Gray];
            title.color = GameConfig.instance.darkMode.colorText[(int)TextColor.White];
         
        }
        else
        {
            main.color = colorBgr[0];
            txt.color = GameConfig.instance.darkMode.colorText[(int)TextColor.Gray];
            title.color = GameConfig.instance.darkMode.colorText[(int)TextColor.Black];
      
        }
    }

    public string GetDescription(TypeHint typeHint)
    {
        /*switch (typeHint)
        {
            case TypeHint.None:
                return "";
            case TypeHint.MoreTwoStar:
                return Util.GetLocalizeRealString("/Hint/" + typeHint);
            case TypeHint.MustTwoWstar:
                return Util.GetLocalizeRealString("/Hint/" + typeHint);
            case TypeHint.MustOneStar:
                return "This cell must be a star";
            case TypeHint.TwoStarCorrect:
                return "This row or column should be marked with dots because there are enough stars";
            case TypeHint.ManyDot:
                return "Many dot in row or column";
            case TypeHint.IncorrectPos:
                return "This cell is in the wrong position";
            case TypeHint.Arround:
                return "Mark the cells around the star as dots";
            case TypeHint.MustClickMoreOneStarInRegion:
                return "This cell must be marked as a star so that there are enough stars in the specified area";
            case TypeHint.ShouldOneStarInRegion:
                return "This location should be marked as a star because if this location were a dot there would not be enough cells to place the star in the designated area";
            case TypeHint.ShouldClickStarInRegion:
                return "Star should be placed in this position";
            case TypeHint.MarkDotUnlessHasCellEmpty:
                return "Nếu vị trí này là 1 ngôi sao thì không còn ô trống trong các vùng đước đánh dấu";
            case TypeHint.ShouldDotBecauseSquare:
                return "Nếu vị trí này là 1 ngôi sao thì vùng đanh được đánh dấu còn lại sẽ không đủ ô trống để đánh dấu sao";
             case TypeHint.MustMarkDot:
                return "Phải đánh dấu các ô này là dáu chấm";
            default:
                return "No Hint";
                  
        }*/
        try
        {
            return Util.GetLocalizeRealString("/Hint/" + typeHint);
        }
        catch {
            return Util.GetLocalizeRealString(Loc.ID.Hint.None);
        }
    }
}
