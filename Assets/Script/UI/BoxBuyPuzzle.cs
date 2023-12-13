using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBuyPuzzle : MonoBehaviour
{
    public NamePopUp namePopUp;
    public void OpenShop()
    {
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.Shop);
        GameManger.instance.manageUi.HidePopUP(namePopUp);
        ShopUI.instance.PrePopUP = namePopUp;
    }
}
