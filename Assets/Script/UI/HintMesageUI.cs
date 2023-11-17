using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class HintMesageUI : MonoBehaviour
{
    public static HintMesageUI instance;
    private void Awake()
    {
        instance = this;    
    }
    public GameObject main;
    public TMP_Text txt;
    public void ShowHint(TypeHint typeHint)
    {
        txt.text = GetDescription(typeHint);
        StopAllCoroutines();
        StartCoroutine(show());
    }
    IEnumerator show()
    {
        main.SetActive(true);
        float duration = 0.7f;
        this.GetComponent<RectTransform>().DOAnchorPosY( +230, duration);
        yield return new WaitForSeconds(2);
        this.GetComponent<RectTransform>().DOAnchorPosY(-100, duration);
        yield return new WaitForSeconds(duration);

        main.SetActive(false);
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
            default:
                return "No Hint";
                  
        }
    }
}
