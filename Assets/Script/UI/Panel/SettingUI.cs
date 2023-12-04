using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using DG.Tweening;
public class SettingUI : BasePopUP
{
    public static SettingUI instance;
    public override void Show(object data = null, int dir = 1)
    {
        main.SetActive(true);
        if (rec == null)
        {
            rec = main.GetComponent<RectTransform>();
        }
        float posY = Screen.width;
        if (dir == -1) // down
        {
            posY = 120;
        }
        rec.DOAnchorPos3DX(0, durationEffect).From(posY * dir).SetEase(ease);
    }
    public override void Hide(int dir = 1)
    {
        if (rec == null)
        {
            rec = main.GetComponent<RectTransform>();
        }

        float posY = -Screen.width;
        if (dir == 1) // up
        {
            posY = -120;
        }

        rec.DOAnchorPos3DX(posY * dir, durationEffect).SetEase(ease).From(0).OnComplete(() => {
            main.SetActive(false);
        });

    }
    public void Back()
    {
        Hide(-1);
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
        
    }

    public Switch myswitch;
     
    
    public void TurnSound()
    {

    }
}
