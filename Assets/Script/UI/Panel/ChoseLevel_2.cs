using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseLevel_2 : BasePopUP
{
    public TittleUI tittle;
    public static ChoseLevel_2 instance;
    public override void Awake()
    {
        base.Awake();
        instance = this;
    }
    
    public ButonLevel btnLevelPrefab;
    public List<ButonLevel> btnLevels;
    public GameObject content;
    public List<Level> levels;
    public void RemoveGarbage()
    {
        for(int  i=content.transform.childCount-1;i>=0; i--)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
    public override void Hide(int dir = 1)
    {
        base.Hide(dir);
        
        btnLevels = new List<ButonLevel>();
    }
    public override void Show(object data = null, int dir =1)
    {
        base.Show(data, dir);
        levels = (List<Level>)data;
        RemoveGarbage();
        ChangeTheme();

        for (int i = 0; i < levels.Count; i++)
        {
            btnLevels.Add(Instantiate(btnLevelPrefab, content.transform));
            NameStateLevel nameStateLevel = NameStateLevel.Nothing;
            if (levels[i].datalevel.isfinished) nameStateLevel = NameStateLevel.Finished;
            else if (!levels[i].datalevel.isfinished && levels[i].datalevel.timeFinish >0)
            {
                nameStateLevel = NameStateLevel.Process;
            }
            btnLevels[i].SetLevel(levels[i].nameLevel, levels[i].nameLevel, nameStateLevel); 
            if (levels[i].datalevel != null)
            { 
                btnLevels[i].SetTime(levels[i].datalevel.timeFinish);
            }
            else
            {
                //btnLevels[i].SetTime(0);
            }
        }
    }
   public void Back()
    {
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.ChoseLevel1, 1, -1);
        GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel2, -1);
    }

    public Sprite bgrDark;
    public Sprite bgrLight;
    public override void ChangeTheme()
    {
        base.ChangeTheme();
        NameTheme theme = GameConfig.instance.nameTheme;
        if (theme == NameTheme.Dark)
        {
            BgrMain.sprite = bgrDark;
        }
        else
        {
            BgrMain.sprite = bgrLight;

        }


    }

}
