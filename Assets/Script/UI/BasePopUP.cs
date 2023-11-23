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
    public GameObject Bgr;
    public bool isPopUp;
    private float durationEffect = 0.5f;
    public void Hide() {

        main.transform.DOScale(0.4f, durationEffect).From(1).OnComplete(()=> {
            main.SetActive(false); 
        }); 
    }
    public virtual void Show(object data = null)
    {
        
        main.SetActive(true);
        main.transform.DOScale(1f, durationEffect).From(0.4f); 
    }
}
