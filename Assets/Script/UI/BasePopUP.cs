using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum NamePopUp
{
    GameWin,
    Lobby,
    ChoseLevel1,
    ChoseLevel2,


}
public class BasePopUP : MonoBehaviour
{
   
    public NamePopUp namePopUp;
    public GameObject main; 
    public bool isPopUp;
    private float durationEffect = 0.5f;
    RectTransform rec;
    public virtual void Hide(int dir = 1) {

        if (rec == null)
        {
            rec = main.GetComponent<RectTransform>();
        }
        rec.DOAnchorPos3DY(Screen.height* dir* 0.8f, 0.2f).From(0).OnComplete(()=> {
            main.SetActive(false); 
        }); 
    }
    public virtual void Show(object data = null,int dir =1)
    {
        
        main.SetActive(true);
        if(rec== null)
        {
            rec = main.GetComponent<RectTransform>();
        }
        rec.DOAnchorPos3DY(0, 0.2f).From(-Screen.height* dir*0.8f);
        
    }
}
