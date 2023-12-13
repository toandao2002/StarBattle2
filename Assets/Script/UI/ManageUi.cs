using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageUi : MonoBehaviour
{
    private void Awake()
    {
  /*      float sx = (Screen.width / 720f);
        float sy = (Screen.height / 1280f);
        this.GetComponent<CanvasScaler>().matchWidthOrHeight = (sx < sy ? 0 : 1);*/
    }
    public List<BasePopUP> popUps;
    public void ShowPopUp(NamePopUp namePopUp, object data = null, int dir = 1)
    {
        foreach(BasePopUP i in popUps)
        {
            if(i.namePopUp == namePopUp)
            {
                i.Show(data, dir);
                break;
            }
        }
    }
    public BasePopUP GetPopUP(NamePopUp namePopUp)
    {

        foreach (BasePopUP i in popUps)
        {
            if (i.namePopUp == namePopUp)
            {
                return i; ;
            }
        }
        return null;
    }
    public void HidePopUP(NamePopUp namePopUp, int dir= 1 )
    {
        foreach (BasePopUP i in popUps)
        {
            if (i.namePopUp == namePopUp)
            {
                i.Hide(dir);
                break;
            }
        }
    }
}
