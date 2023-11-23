using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseLevel_1 : BasePopUP
{
    public List<BoxChoseBigLevel> boxChoseBigLevels;
    public override void Show(object data = null)
    {
        base.Show(data);
        List<SubLevel> subLevels = GameConfig.instance.GetSubsLevelByTypeGame();
        for (int i = 0; i < subLevels.Count; i++)
        {
            boxChoseBigLevels[i].nameLevel = subLevels[i].nameSubLevel;
            boxChoseBigLevels[i].NumLevelIn = subLevels[i].levels.Count;
            boxChoseBigLevels[i].levels = subLevels[i].levels;
            
        }
           
            
        
    }

}
