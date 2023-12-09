using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBuyPuzzle : MonoBehaviour
{
    public void OpenShop()
    {
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.Shop);
        GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel2);
    }
}
