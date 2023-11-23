using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxChoseBigLevel : MonoBehaviour
{
    public string nameLevel;
    public int NumLevelIn;
    public List<Level> levels;
    public void OpenChoseLevel2()
    {
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.ChoseLevel2,levels );

        GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel1);
    }
}
