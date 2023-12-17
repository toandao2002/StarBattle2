using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class MasterAdsController : MonoBehaviour
{
    [ReadOnly]
    public MasterControl masterControl;
    [SerializeField]
    private List<IAdsController> adsControllers;
    [ReadOnly]
    [SerializeField]
    float interstitialTimer = 0;
    [ReadOnly]
    public bool isInterNull = true;
    public IAdsController currentLoadedBannerCtr, currentLoadedInterstitialCtr, currentLoadedRewardCtr;
    #region init
    public void Init()
    {
        PlayerPrefs.DeleteKey("TimeToShowAds");
        PlayerPrefs.SetString("TimeToShowAds", System.DateTime.Now.ToString());
        Debug.Log("INIT ADS CONTROLLER");
        adsControllers = new List<IAdsController>();
        foreach (Transform ac in transform)
        {
            if (!ac.gameObject.activeInHierarchy) continue;
            IAdsController ia = ac.GetComponent<IAdsController>();
            if (adsControllers.Count > 0)
            {
                adsControllers[adsControllers.Count - 1].SetNext(ia);
            }
            adsControllers.Add(ia);
            ia.Init(this,OnInitComnpleted);
        }


        //adsControllers[0].LoadBanner();
        

        try
        {
            LoadRule();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }
    public void OnInitComnpleted()
    {
        isInit = true;
        adInited = true;
    }
    
    public void SetOnTop(string adNetworkName)
    {
        List<IAdsController> list = new List<IAdsController>();
        list.AddRange(adsControllers);
        Debug.Log("TOTAL LIST: " + list.Count);
        adsControllers.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].ToString().Contains(adNetworkName))
            {
                adsControllers.Insert(0, list[i]);
                list.RemoveAt(i);
                break;
            }
        }
        adsControllers.AddRange(list);
        for (int i = 0; i < adsControllers.Count - 1; i++)
        {
            adsControllers[i].SetNext(adsControllers[i + 1]);
            Debug.Log("NEW" + i + " " + adsControllers[i].ToString());
        }

    }
    public bool CanPlayInterstitialAd()
    {
        return interstitialTimer <= 0;
    }
    public float GetInterstitialTimer()
    {
        return this.interstitialTimer;
    }
    public void SetInterstitialTimer(float time)
    {
        Debug.Log("SET TIMER:" + time);
        this.interstitialTimer = time;
    }
    [ReadOnly]
    public float rewardedAdWaitTime = 0f, bannerAdWaitTime = 0, delayRewardTimer = 0f;
    bool canShow = false;
    bool isInit = false, adInited;
    void Update()
    {
        if (isInit)
        {
            Debug.Log("ADMOB ON COMPLETED");
            LoadInterstitial();
            LoadRewardedVideo();

            if (PrefInfo.IsUsingAd())
            {
                ShowBanner();
            }
            else
            {
                HideBanner();
            }
            isInit = false;
        }
        if (canShow)
        {
            canShow = false;
            if (currentPanel != null)
            {
                (currentPanel).ShowAfterAd();
            }
            currentPanel = null;
        }

        //banner
        UpdateBanner();

        // interstitial
        UpdateInter();

        //rewarded ad
        UpdateReward();
    }
    #endregion

    [ReadOnly]
    public bool showBanner, showInterstitial, showReward;

    #region banner
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (!adInited) return;
            ShowBanner();
        }
    }
    public void LoadBanner()
    {
        Debug.Log("LOAD BANNER MAC");
        if(currentLoadedBannerCtr == null)
        {
            adsControllers[0].LoadBanner();
            return;
        }

    }
    public void ReLoadBanner()
    {
        Debug.Log("RELOAD BANNER MAC");
        if (currentLoadedBannerCtr == null) return;
        currentLoadedBannerCtr.ReloadBanner();

    }
    public void HideBanner()
    {
        Debug.Log("HIDE BANNER MAC");
        try
        {
            currentLoadedBannerCtr.HideBanner();
        }
        catch (System.Exception) { }
/*
        if(AdPanel.Instance!=null)
        AdPanel.Instance.gameObject.SetActive(false);*/

    }
    public bool ShowBanner()
    {
        if (masterControl.adsState == AdsState.UnlockAll) return false;
        if (PrefInfo.IsUsingAd())
        {
            Debug.Log("SHOW BANNER MAC");
            showBanner = true;
            if (currentLoadedBannerCtr == null)
            {
                LoadBanner();
            }
            else if (failedBanner)
            {
                ReLoadBanner();
            }
            return true;
        }
        return false;
    }
    private void UpdateBanner()
    {
        if (!showBanner) return;

        //Wait 4 second for banner
        if (bannerAdWaitTime < 4f)
        {
            bannerAdWaitTime += Time.deltaTime;
        }
        else
        {
            showBanner = false;
            bannerAdWaitTime = 0;
            if (currentLoadedBannerCtr != null)
            {
                currentLoadedBannerCtr.ShowBanner();
                if (currentLoadedBannerCtr.ToString().Equals("Admob"))
                {
                    adsControllers[1].HideBanner();
                    adsControllers[adsControllers.Count - 1].HideBanner();
                }
            }
            else
            {
                //LoadBanner();
                adsControllers[adsControllers.Count - 1].ShowBanner();
            }
        }
    }
    #endregion
    //
    #region interstitial
    Panel currentPanel;
    bool chk = false, chk2 = false;
    public void LoadInterstitial()
    {
        Debug.Log("LOAD INTER MAC");
        if (currentLoadedInterstitialCtr == null)
        {
            adsControllers[0].LoadInterstitial();
            //return;
        }
        //currentLoadedInterstitialCtr.LoadInterstitial();
    }
    public bool IsInterstitialReady()
    {
        if (currentLoadedInterstitialCtr == null) return false;
        return currentLoadedInterstitialCtr.IsInterstitialReady();
    }
    public bool ShowInterstitial(Panel panel = null)
    {
        LocalNotificationManager.Instance.isAd = true;
        Debug.Log("-------------------------SHOW INTERSTITIAL AD: !!!!--------------------");
        if (MasterControl.Instance.adsState == AdsState.UnlockAll)
        {
            return false;
        }
        if (!isAllowShowInter(false))
        {
            Debug.Log("-<<<<<<<<<<<<<<<<<<<<NOT ALLOW SHOW INTERSTITIAL AD: !!!!");
            //InterstitialCallback(false);
            return false;
        }
        if (!PrefInfo.IsUsingAd()) return false;
        PlayerPrefs.DeleteKey("TimeToShowAds");

        Debug.Log("->>>>>>>>>>>>>>>>>>>>>>>>>>>>ALLOW SHOW INTERSTITIAL AD: !!!!");
        currentPanel = panel;
        showInterstitial = true;

#if UNITY_EDITOR
        //if (!masterControl.isConnected && AdInterPanel.Instance.SetUp())
        //{
        //    showInterstitial = false;
        //}
        if (currentLoadedInterstitialCtr == null)
        {
            LoadInterstitial();
        }
#elif UNITY_ANDROID
        if (currentLoadedInterstitialCtr == null)
        {
            LoadInterstitial();
        }
#elif UNITY_IOS
        if (currentLoadedInterstitialCtr == null)
        {
            LoadInterstitial();
        }
#endif
        return true;
    }
    public void InterstitialCallback(bool showResult)
    {
        Debug.Log("CALL BACK " + showResult );
        if (currentPanel != null)
        {
            canShow = true;
        }
        if (showResult)
        {
         
            chk = true;
            //PlayerPrefs.SetString("TimeToShowAds", System.DateTime.Now.ToString());
            isReward = false;
            LocalNotificationManager.Instance.isAd = false;

        }
        else
        {
            //PlayerPrefs.SetString("TimeToShowAds", System.DateTime.Now.ToString());
            chk2 = true;
            isReward = false;
            LocalNotificationManager.Instance.isAd = false;
        }
    }
    private void UpdateInter()
    {
        if (interstitialTimer > 0)
        {
            interstitialTimer -= Time.deltaTime;
        }

        if (currentLoadedInterstitialCtr != null && showInterstitial)
        {
            FireBaseManager.Instance.LogLevelStart(FireBaseManager.LOG_CONTENT_CC_INTER, FireBaseManager.LOG_CONTENT_CC_MODE_AD);
            currentLoadedInterstitialCtr.ShowInterstitial();
            showInterstitial = false;
            //currentLoadedInterstitialCtr = null;
        }
    }
    private void FixedUpdate()
    {
        if (chk)
        {
            PlayerPrefs.SetString("TimeToShowAds", System.DateTime.Now.ToString());
            chk = false;
            PrefInfo.AddInterWatched();
            //Controller.Instance.AddInterWatch();
            FireBaseManager.Instance.LogEvent("ADS_INTERSTITIAL");
            ResetCountGame();
        }
        if (chk2)
        {
            PlayerPrefs.SetString("TimeToShowAds", System.DateTime.Now.ToString());
            ResetCountGame();
            chk2 = false;
            //AdInterPanel.Instance.SetUp();
        }
    }
    //void LoadInterstitialEvents()
    //{
    //    adsControllers[0].LoadInterstitialEvents();
    //}
    #endregion
    //
    #region reward
    int currentNetworkForReward = 0;
    public void LoadRewardedVideo()
    {
        if (currentLoadedRewardCtr == null)
        {
            Debug.Log("LOAD REWARD MAC");
            adsControllers[0].LoadRewardedVideo();
            currentNetworkForReward = 0;
        }
    }
    public void ShowRewardedVideo()
    {
      
        Debug.Log("SHOW REWARD MAC");
        LocalNotificationManager.Instance.isAd = true;
        showReward = true;
        if (currentLoadedRewardCtr == null)
        {
            LoadRewardedVideo();
        }
        else
        {
            SetRewardUser();
        }
    }
    public void OnRewarded()
    {
        Debug.Log("ON REWARD MAC");
        masterControl.OnRewardedAd();
        LocalNotificationManager.Instance.isAd = false;

        LoadRewardedVideo();

        PrefInfo.AddRewardWatched();
       // Controller.Instance.AddRewardWatch();
        FireBaseManager.Instance.LogEvent("ADS_REWARD");
    }
    public void OnFail()
    {
        masterControl.OnFail();
        LoadRewardedVideo();
        LocalNotificationManager.Instance.isAd = false;
    }
    public void SetRewardUser()
    {
        isReward = true;
        PlayerPrefs.SetString("TimeToShowAds", System.DateTime.Now.AddSeconds(adSetting_time_reward).ToString());

    }
    private void UpdateReward()
    {
        if (delayRewardTimer > 0)
        {
            delayRewardTimer -= Time.deltaTime;
        }
        if (currentLoadedRewardCtr != null && showReward)
        {
            int count = 0;
            for (int i = 0; i < adsControllers.Count; i++)
            {
                if (adsControllers[i] == currentLoadedRewardCtr)
                {
                    count = i;
                    break;
                }
            }

            FireBaseManager.Instance.LogLevelStart(FireBaseManager.LOG_CONTENT_CC_REWARD, FireBaseManager.LOG_CONTENT_CC_MODE_AD);
            currentLoadedRewardCtr.ShowRewardedVideo();
            delayRewardTimer = 5;
            showReward = false;
            SetRewardUser();
            //currentLoadedRewardCtr = null;
        }
        else if (showReward && currentLoadedRewardCtr == null)
        {
            if (rewardedAdWaitTime > 0.5f)
            {
                rewardedAdWaitTime = 0;
                showReward = false;
                // thông báo ko có AD

                //if (masterControl.isConnected && Controller.Instance.notiBuyChapter.gameObject.activeInHierarchy)
                //{
                //    FireBaseManager.Instance.LogEvent("premium_rewrad_default_ads");
                //    Controller.Instance.adsOffLineController.Active();
                //}
                //else
                //{
                //    //Controller.Instance.current = screen.message;
                //    Controller.Instance.notiNoAdsAvail.gameObject.SetActive(true);
                MasterControl.Instance.CheckInternet(res =>
                {
                    if (!masterControl.isConnected)
                    {
                        Debug.Log("NO internet");
                    }
                }, false);
                OnFail();
                //}

            }
            else
            {
                rewardedAdWaitTime += Time.deltaTime;
            }
        }
    }
    #endregion
    //
    #region NativeAD
    public bool IsNativeAdAvailable()
    {
        foreach (IAdsController adHandler in adsControllers)
        {
            if (adHandler.IsNativeAvailable())
            {
                return true;
            }
        }
        return false;
    }
    public object GetNativeAd()
    {
        foreach (IAdsController adHandler in adsControllers)
        {
            if (adHandler.IsNativeAvailable())
            {
                return adHandler.GetCurrentNativeAd();
            }
        }

        //LoadNativeAd();
        return null;
    }
    public void LoadNativeAd()
    {
        Debug.Log("LOAD NATIVE AD");
        foreach (IAdsController adHandler in adsControllers)
        {
            adHandler.LoadNativeAd();
        }
    }
    public void ReloadAllNativeAdBanner()
    {
        Debug.Log("RELOAD NATIVE AD");
        LoadNativeAd();
        onNativeAdRefresh?.Invoke();
    }
    public delegate void OnNativeAdRefresh();
    public OnNativeAdRefresh onNativeAdRefresh;
    public void RemoveAd()
    {
        ReloadAllNativeAdBanner();
    }
    #endregion


    #region IAds Manager
    //private IAdsController Get(AdType type)
    //{
    //    foreach(var ad in adsControllers)
    //    {
    //        if (ad.HasAdType(type))
    //            return ad;
    //    }

    //    return adsControllers[0];
    //}
    #endregion

    #region logicads
    string adSetting_time_iap = "";
    string adSetting_time_normal = "";
    string adSetting_play = "";
    string adSetting_level = "";

    int adSetting_time_reward = 30;
    int[] adSetting_time_iap_list;
    int[] adSetting_time_normal_list;
    int[] adSetting_play_list;
    int[] adSetting_level_list;
    [ReadOnly]
    public bool isReward, isIAP;
    [ReadOnly]
    public int countGame = 0;
    public bool failedBanner;

    public void LoadRule()
    {
        adSetting_level =  PlayerPrefs.GetString("AdSetting_level", "3,15,35,60,100") ;
        Debug.Log("ADADAD:" + adSetting_level);
        adSetting_level_list = Array.ConvertAll(adSetting_level.Split(','), int.Parse);

        adSetting_play = PlayerPrefs.GetString("AdSetting_play", "3,2,2,2,2");
        adSetting_play_list = Array.ConvertAll(adSetting_play.Split(','), int.Parse);

        adSetting_time_normal =  PlayerPrefs.GetString("AdSetting_time_normal", "45,45,50,55,55") ;
        adSetting_time_normal_list = Array.ConvertAll(adSetting_time_normal.Split(','), int.Parse);

        adSetting_time_reward = PlayerPrefs.GetInt("AdSetting_time_reward", 15);

        adSetting_time_iap = PlayerPrefs.GetString("AdSetting_time_iap", "65,60,55,50,45");
        adSetting_time_iap_list = Array.ConvertAll(adSetting_time_iap.Split(','), int.Parse);

        isIAP = (PlayerPrefs.GetInt("UserIAP", 0) == 1);

        LoadRuleOpenAd();
    }


    string openAd;
    int[] OpenAd_list;
    int timerequireShow;

    private void LoadRuleOpenAd()
    {
        openAd = PlayerPrefs.GetString("Open_ADS", "1,2");
        OpenAd_list = Array.ConvertAll(openAd.Split(','), int.Parse);
        timerequireShow = PlayerPrefs.GetInt("Time_ROpenAd", 10);
    }
    public bool isAllowFirstOpen(int time)
    {
        if (!PrefInfo.IsUsingAd()) return false;
        //if (!isProduc) return false;
        if (OpenAd_list[0] == 0) return false;
        if (time < OpenAd_list[1]) return false;
        return true;
    }
    public bool isAllowTabOpen(int time)
    {
        if (!PrefInfo.IsUsingAd()) return false;
        if (OpenAd_list[0] == 0) return false;
        //if (!masterControl.allowOpenAd) return false;
        if (time < OpenAd_list[1]) return false;
        DateTime timeOld = DateTime.Parse(PlayerPrefs.GetString("TimeToShowOpenAds", new DateTime(1990, 1, 1).ToString()));
        TimeSpan tp = DateTime.Now - timeOld;
        double time2 = tp.TotalSeconds;
        //float timeRequire = adSetting_time_normal_list[0];
        //Debug.Log(("<><><><>Open Ad TIME AD :" + time2 + " || " + timeRequire));
        if (time2 < timerequireShow) return false;

        //if (time < openAd_Tab_list[1]) return false;
        return true;
    }
    public void ResetCountGame()
    {
        countGame = 0;
        PlayerPrefs.SetInt("CountGameToShowAds", 0);
    }
    public bool isAllowShowInter(bool overwrite)
    {
        try
        {
            if (!PrefInfo.IsUsingAd()) return false;

            if (overwrite)
            {
                return true;
            }
            int index = 0;
            int level = 1;

            isIAP = PlayerPrefs.GetInt("UserIAP", 0) == 1;

            DateTime timeOld = DateTime.Parse(PlayerPrefs.GetString("TimeToShowAds", new DateTime(1990, 1, 1).ToString()));
            TimeSpan tp = DateTime.Now - timeOld;
            double time = tp.TotalSeconds;
            Debug.Log(("<><><><>LEVEL :" + level)+ " || "+ "TIME AD :" + time);
            if (level < adSetting_level_list[0])
            {
                return false;
            }
            else if (level >= adSetting_level_list[0] && level < adSetting_level_list[1])
            {
                index = 1;
            }
            else if (level >= adSetting_level_list[1] && level < adSetting_level_list[2])
            {
                index = 2;
            }
            else if (level >= adSetting_level_list[2] && level < adSetting_level_list[3])
            {
                index = 3;
            }
            else
            {
                index = 4;
            }


            float timeRequire = adSetting_time_normal_list[index - 1];
            if (isIAP)
            {
                timeRequire = adSetting_time_iap_list[index - 1];
            }
            else
            {
                if (isReward)
                {
                    timeRequire = adSetting_time_normal_list[index - 1] + adSetting_time_reward;
                }
                else
                {
                    timeRequire = adSetting_time_normal_list[index - 1];
                }
            }

            Debug.Log("CHECK TIME : count: " + countGame + " \ttime: " + timeRequire);
            if (countGame >= adSetting_play_list[index - 1] || time >= timeRequire)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e);
            return false;
        }

    }
#endregion
}


public interface IAdsController
{

    void Init(MasterAdsController ctr,System.Action callback);
    //bool HasAdType(AdType ad);

    void LoadBanner();
    void ReloadBanner();
    bool ShowBanner();
    void HideBanner();
    //
    void LoadInterstitial();
    bool ShowInterstitial();
    void LoadInterstitialEvents();
    bool IsInterstitialReady();
    //
    void LoadRewardedVideo();
    bool ShowRewardedVideo();
    void OnRewarded();
    //
    void LoadNativeAd();
    bool IsNativeAvailable();
    object GetCurrentNativeAd();
    void SetNext(IAdsController ctr);

}

//public enum AdType
//{
//    Banner, Inter, Reward, Open, Native
//}

//public enum PiorityAd
//{
//    LoadBeforeLoadingScene, LoadAfterLoadingScene, LoadInGameScene
//}