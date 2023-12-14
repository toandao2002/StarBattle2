using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BoxLanguage : BoxSetting
{
    // Start is called before the first frame update
    public Image popUP;
    public static BoxLanguage instance;
    public bool isOpenPopUp;
    public GameObject bgrHide;
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        popUP.gameObject.SetActive(false);
        bgrHide.SetActive(false);
    }
    public void ShowPopUpChoseLanguage()
    {
        isOpenPopUp = !isOpenPopUp;
        if (isOpenPopUp)
        {
            popUP.gameObject.SetActive(true);
            popUP.DOFillAmount(1, 0.3f).From(0).SetEase(Ease.InOutBack).SetUpdate(true);
            popUP.GetComponent<RectTransform>().DOAnchorPos3DY(0, 0.6f).SetEase(Ease.OutElastic).From(50).SetUpdate(true);
        } 
        else
        {
           
            popUP.DOFillAmount(0, 0.3f).From(1).SetEase(Ease.InOutQuart).OnComplete(()=> {
                popUP.gameObject.SetActive(false);
            }).SetUpdate(true);
            popUP.GetComponent<RectTransform>().DOAnchorPos3DY(50, 0.3f).SetEase(Ease.InElastic).SetUpdate(true);
        }
        bgrHide.SetActive(isOpenPopUp);

    }

}
