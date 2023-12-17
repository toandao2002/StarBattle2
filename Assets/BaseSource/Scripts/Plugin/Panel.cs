using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    protected Animator ani;
    protected Transform _transform;
    protected bool overrideBack = false;
    public delegate void OnHide();
    public OnHide onClosePanel;

    //public delegate void OnHide();
    public virtual void Show()
    {
        Active();
    }
    public virtual void Hide()
    {
        //SoundManager.Instance.PlaySFX(SFX.popUpClose);
        ani.SetTrigger("Close");
    }
    public virtual void Deactive()
    {
        gameObject.SetActive(false);
    }
    public virtual void Active()
    {
        gameObject.SetActive(true);
        overrideBack = false;
    }
    public void Init()
    {
        ani = GetComponent<Animator>();
        _transform = transform;
        PostInit();
    }
    public virtual void ShowAfterAd() { }
    public virtual void Close()
    {
        Hide();
    }
    public virtual void PostInit()
    {
        
    }

    public virtual void OnBack()
    {
        Close();
    }
}

public class Panel<T> : Panel where T : Panel
{
    public static T Instance { get; set; }
    //public delegate void OnHide();
    public OnHide onHidePanel;
    public override void PostInit()
    {
        Instance = this.GetComponent<T>();
    }

    public override void Deactive()
    {
        base.Deactive();
        onHidePanel?.Invoke();

        Destroy();
    }

    private void Destroy()
    {
        Instance = null;

    }

    private void OnEnable()
    {
        //if(Camera.main.aspect < 0.56f)
        //{
        //    var content = this.transform.Find("Content");
        //    if (content != null)
        //    {
        //        var panel = content.Find("Panel");
        //        if(panel!=null)
        //            panel.localScale = Vector3.one * 0.9f;
        //    }

        //}
    }

    public override void Close()
    {
        onClosePanel?.Invoke();
        base.Close();
    }
}