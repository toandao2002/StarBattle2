using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseLevel_2 : BasePopUP
{
    public ButonLevel btnLevelPrefab;
    public List<ButonLevel> btnLevels;
    public GameObject content;
    public List<Level> levels;

    public override void Show(object data = null)
    {
        base.Show(data);
        levels = (List<Level>)data;
        for (int i = 0; i < levels.Count; i++)
        {
            btnLevels.Add(Instantiate(btnLevelPrefab, content.transform));
            btnLevels[i].SetLevel(levels[i].nameLevel, levels[i].nameLevel);
            DataLevel datalevel = DataGame.GetDataLevel(GameConfig.instance.typeGame, levels[i].nameLevel);
            if (datalevel != null)
            {
                levels[i].datalevel = datalevel;
                btnLevels[i].SetTime(datalevel.timeFinish);
            }
            else
            {
                //btnLevels[i].SetTime(0);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
