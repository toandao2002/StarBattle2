using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageUi : MonoBehaviour
{
    
    public List<BasePopUP> popUps;
    public void ShowPopUp(NamePopUp namePopUp, object data = null)
    {
        foreach(BasePopUP i in popUps)
        {
            if(i.namePopUp == namePopUp)
            {
                i.Show(data);
                break;
            }
        }
    }
    public void HidePopUP(NamePopUp namePopUp )
    {
        foreach (BasePopUP i in popUps)
        {
            if (i.namePopUp == namePopUp)
            {
                i.Hide();
                break;
            }
        }
    }
}
