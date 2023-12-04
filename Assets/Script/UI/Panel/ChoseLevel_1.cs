using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseLevel_1 : BasePopUP
{
    public TittleUI tittle;
    public List<BoxChoseBigLevel> boxChoseBigLevels;
    public override void Show(object data = null, int dir =1)
    {
        base.Show(data,dir);
        
        List<SubLevel> subLevels = GameConfig.instance.GetCurrentSubs();
        tittle.ShowTittle(subLevels[0].nameSubLevel.Substring(0, subLevels[0].nameSubLevel.Length-2));
        for (int i = 0; i < subLevels.Count; i++)
        {

            boxChoseBigLevels[i]. InitBox(subLevels[i]);
        }
    }
     public void  backHome()
    {
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.Lobby, 1, -1);
        GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel1, -1);
    }

}
