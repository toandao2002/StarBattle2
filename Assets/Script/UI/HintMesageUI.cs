using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class HintMesageUI : MonoBehaviour
{
    public static HintMesageUI instance;
    private void Awake()
    {
        instance = this;    
    }
    public Image main;
    public TMP_Text txt; 
    public List<TMP_Text> textFade;
    public void ShowHint(TypeHint typeHint)
    {
        txt.text = GetDescription(typeHint);
        StopAllCoroutines();
        StartCoroutine(show());
    }
    IEnumerator show()
    {
        
        main.gameObject.SetActive(true);
        float duration = 1f;
        main.DOFade(1, duration/2).From(0);
        foreach(TMP_Text i in textFade)
        {
            i.DOFade(1, duration/2).From(0);
        } 
        yield return new WaitForSeconds(duration*2+1);
        main.DOFade(0, duration/2).From(1);
        foreach(TMP_Text i in textFade)
        {
            i.DOFade(0, duration/2).From(1);
        }
        yield return new WaitForSeconds(duration/2);
        main.gameObject.SetActive(false);
    }
    public string GetDescription(TypeHint typeHint)
    {
        switch (typeHint)
        {
            case TypeHint.None:
                return "";
            case TypeHint.MoreTwoStar:
                return "Has more star in row or column";
            case TypeHint.MustTwoWstar:
                return "These two cells must be stars";
            case TypeHint.MustOneStar:
                return "This cell must be a star";
            case TypeHint.TwoStarCorrect:
                return "This row or column should be marked with a dot";
            case TypeHint.ManyDot:
                return "Many dot in row or column";
            case TypeHint.IncorrectPos:
                return "This box is in the wrong position";
            case TypeHint.Arround:
                return "Mark the dots around the star cells";
            case TypeHint.MustClickMoreOneStarInRegion:
                return "Must mark more than 1 star in 1 area";
            case TypeHint.ShouldOneStarInRegion:
                return "A star should be placed in this position";
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
                  
        }
    }
}
