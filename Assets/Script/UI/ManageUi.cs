using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageUi : MonoBehaviour
{
    
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
