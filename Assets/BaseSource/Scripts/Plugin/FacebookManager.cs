using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
public class FacebookManager
{
    public void Init()
    {
        Debug.Log("->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>INITTING FACEBOOK");


        //Instance = this;
        if (FB.IsInitialized)
        {
            Debug.Log("->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>INIT FACEBOOK");
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("->>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>NOT INIT FACEBOOK");

            //Handle FB.Init
            FB.Init(() =>
            {
                FB.ActivateApp();
                //Debug.Log("DEEP LINKKKKKKKKKKKKKKKKKKKKKKK");
                //FB.Mobile.FetchDeferredAppLinkData(AppLinkCallback);
            });
        }
    }
    private void AppLinkCallback(IAppLinkResult result)
    {

    }
   
    public void Login()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }
    public void Login(FacebookDelegate<ILoginResult> callback)
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, callback);
    }
    private void AuthCallback(ILoginResult result)
    {

        if (FB.IsLoggedIn)
        {
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }
    public void FacebookGameRequest()
    {
        if (FB.IsLoggedIn)
        {
            FB.AppRequest("This game is real!!", title: "Hero Rescue", callback: AppRequestCallbaack);
        }
        else
        {
            Login();
        }
    }

    public void ShareLink()
    {
        if (FB.IsLoggedIn)
        {
            FB.ShareLink(new System.Uri("http://tiny.cc/HeroRescue"),"Hero Rescue", "OMG This game is real now! Download now: http://tiny.cc/HeroRescue",  callback: ShareCallback);
        }
        else
        {
            Login();
        }
    }
    private void AppRequestCallbaack(IAppRequestResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("request Error: " + result.Error);
        }
        else
        {
            Debug.Log("request success!");
        }
    }

    private void ShareCallback(IShareResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink Error: " + result.Error);
        }
        else
        {
            // Share succeeded without postID
            Debug.Log("ShareLink success!");
        }
    }
}
