using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseLevel_1 : BasePopUP
{
    
    public List<BoxChoseBigLevel> boxChoseBigLevels;
    public override void Show(object data = null, int dir =1)
    {
        base.Show(data,dir);
        List<SubLevel> subLevels = GameConfig.instance.GetSubsLevelByTypeGame();
        for (int i = 0; i < subLevels.Count; i++)
        {

            boxChoseBigLevels[i]. InitBox(subLevels[i]);
        }
    }

}
