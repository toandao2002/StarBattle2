using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.RemoteConfig;
using System.Threading.Tasks;
using System;
using Sirenix.OdinInspector;
//using System.Threading.Tasks;
//using Cysharp.Threading.Tasks;

public class FireBaseManager
{
    public static FireBaseManager Instance { get; set; }

    #region Init beforeSplash
    // Use this for initialization
    private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    private bool isReady = false;
    public bool doneFetch = false;
    private LastFetchStatus result;

    public void InitFirebase()
    {
        Init();
        //Instance.FetchData();
    }
    private void Init()
    {
        DelayInit();
    }
    private void DelayInit()
    {
        isReady = false;
        Debug.Log("INIT FIREBASE");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            Debug.Log("FIREBASE INITIALIZED");
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }
    private void InitializeFirebase()
    {
        Debug.Log("Enabling data collection.");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

        isReady = true;

        FetchData();
    }
    private void FetchData()
    {
        //check = false;
        //StartCoroutine(WaitForFetch());
        //StartCoroutine(DoWaitForFetchDone());
        Fetch();
    }
    private void Fetch()
    {
        Debug.Log("Fetching data...");
        Debug.LogFormat("Firebase init 3");
        try
        {
            FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).ContinueWith(task2 =>
            {

                ConfigInfo info = FirebaseRemoteConfig.DefaultInstance.Info;
                result = info.LastFetchStatus;
                if (info.LastFetchStatus.Equals(LastFetchStatus.Success))
                {
                    DoWaitForFetchDone();
                }
                else if (info.LastFetchStatus.Equals(LastFetchStatus.Failure))
                {
                    Debug.Log("FAIL TO LOAD");

                }
                else
                {
                    Debug.Log("PENDING");

                }
                //MenuController.Instance.levelButtonManager.RefreshButtons(GameMode.hardest);
                //MenuController.Instance.levelButtonManager.RefreshButtons(GameMode.challenge);
            });
            Debug.LogFormat("Firebase init 4");
        }
        catch (System.Exception e)
        {
            Debug.Log("LOI 5108410284: " + e.ToString());

        }
    }
    //private bool check = false;
    private async void DoWaitForFetchDone()
    {
        await Cysharp.Threading.Tasks.UniTask.SwitchToMainThread();
        doneFetch = false;
        if (result.Equals(LastFetchStatus.Success))
        {
            try
            {
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
                //Firebase.RemoteConfig.FirebaseRemoteConfigDeprecated.ActivateFetched();
                ConfigValue value = FirebaseRemoteConfig.DefaultInstance.GetValue("AdSetting_time_iap2");
                Debug.Log("AdSetting_time_iap: " + value.StringValue);
                if (!string.IsNullOrEmpty(value.StringValue))
                {
                    PlayerPrefs.SetString("AdSetting_time_iap", value.StringValue);
                }

                ConfigValue value1 = FirebaseRemoteConfig.DefaultInstance.GetValue("AdSetting_time_reward2");
                Debug.Log("AdSetting_time_reward: " + value1.StringValue);
                if (!string.IsNullOrEmpty(value1.StringValue))
                {
                    PlayerPrefs.SetInt("AdSetting_time_reward", int.Parse(value1.StringValue));
                }

                ConfigValue value2 = FirebaseRemoteConfig.DefaultInstance.GetValue("AdSetting_time_normal2");
                Debug.Log("AdSetting_time_normal: " + value2.StringValue);
                if (!string.IsNullOrEmpty(value2.StringValue))
                {
                    PlayerPrefs.SetString("AdSetting_time_normal", value2.StringValue);
                }

                ConfigValue value3 = FirebaseRemoteConfig.DefaultInstance.GetValue("AdSetting_play2");
                Debug.Log("AdSetting_play: " + value3.StringValue);
                if (!string.IsNullOrEmpty(value3.StringValue))
                {
                    PlayerPrefs.SetString("AdSetting_play", value3.StringValue);
                }

                ConfigValue value4 = FirebaseRemoteConfig.DefaultInstance.GetValue("AdSetting_level2");
                Debug.Log("AdSetting_level: " + value4.StringValue);
                if (!string.IsNullOrEmpty(value4.StringValue))
                {
                    PlayerPrefs.SetString("AdSetting_level", value4.StringValue);
                }
                ConfigValue value5 = FirebaseRemoteConfig.DefaultInstance.GetValue("level_show_rate");
                Debug.Log("level_show_rate: " + value5.StringValue);
                if (!string.IsNullOrEmpty(value5.StringValue))
                {
                    PlayerPrefs.SetInt("level_show_rate", int.Parse(value5.StringValue));
                }



                #region myRemote
                ConfigValue Unlock_Booster = FirebaseRemoteConfig.DefaultInstance.GetValue("Unlock_Booster");
                Debug.Log("Unlock_Booster: " + Unlock_Booster.StringValue);
                if (!string.IsNullOrEmpty(Unlock_Booster.StringValue))
                {
                    PlayerPrefs.SetInt("Unlock_Booster", int.Parse(Unlock_Booster.StringValue));
                }

                ConfigValue unlockMode = FirebaseRemoteConfig.DefaultInstance.GetValue("Unlock_Mode");
                Debug.Log("Unlock_Mode: " + unlockMode.StringValue);
                if (!string.IsNullOrEmpty(unlockMode.StringValue))
                {
                    PlayerPrefs.SetString("Unlock_Mode", (unlockMode.StringValue));
                }

                ConfigValue Chest_reward = FirebaseRemoteConfig.DefaultInstance.GetValue("Chest_reward");
                Debug.Log("Chest_reward: " + Chest_reward.StringValue);
                if (!string.IsNullOrEmpty(Chest_reward.StringValue))
                {
                    PlayerPrefs.SetString("Chest_reward", (Chest_reward.StringValue));
                }

                ConfigValue DailyC_win = FirebaseRemoteConfig.DefaultInstance.GetValue("DailyC_win");
                Debug.Log("DailyC_win: " + DailyC_win.StringValue);
                if (!string.IsNullOrEmpty(DailyC_win.StringValue))
                {
                    PlayerPrefs.SetString("DailyC_win", (DailyC_win.StringValue));
                }

                ConfigValue DailyC_progress = FirebaseRemoteConfig.DefaultInstance.GetValue("DailyC_progress");
                Debug.Log("DailyC_progress: " + DailyC_progress.StringValue);
                if (!string.IsNullOrEmpty(DailyC_progress.StringValue))
                {
                    PlayerPrefs.SetString("DailyC_progress", (DailyC_progress.StringValue));
                }

                ConfigValue Challenge_reward = FirebaseRemoteConfig.DefaultInstance.GetValue("Challenge_reward");
                Debug.Log("Challenge_reward: " + Challenge_reward.StringValue);
                if (!string.IsNullOrEmpty(Challenge_reward.StringValue))
                {
                    PlayerPrefs.SetString("Challenge_reward", (Challenge_reward.StringValue));
                }

                ConfigValue Paint_Enable = FirebaseRemoteConfig.DefaultInstance.GetValue("Paint_Enable");
                Debug.Log("Paint_Enable: " + Paint_Enable.StringValue);
                if (!string.IsNullOrEmpty(Paint_Enable.StringValue))
                {
                    PlayerPrefs.SetInt("Paint_Enable", int.Parse(Paint_Enable.StringValue));
                }

                ConfigValue AddTube_limit = FirebaseRemoteConfig.DefaultInstance.GetValue("AddTube_limit");
                Debug.Log("AddTube_limit: " + AddTube_limit.StringValue);
                if (!string.IsNullOrEmpty(AddTube_limit.StringValue))
                {
                    PlayerPrefs.SetInt("AddTube_limit", int.Parse(AddTube_limit.StringValue));
                }

                ConfigValue Open_Home = FirebaseRemoteConfig.DefaultInstance.GetValue("Open_Home");
                Debug.Log("Open_Home: " + Open_Home.StringValue);
                if (!string.IsNullOrEmpty(Open_Home.StringValue))
                {
                    PlayerPrefs.SetInt("Open_Home", int.Parse(Open_Home.StringValue));
                }

                ConfigValue Reward_AdsCoin = FirebaseRemoteConfig.DefaultInstance.GetValue("Reward_AdsCoin");
                Debug.Log("Reward_AdsCoin: " + Reward_AdsCoin.StringValue);
                if (!string.IsNullOrEmpty(Reward_AdsCoin.StringValue))
                {
                    PlayerPrefs.SetInt("Reward_AdsCoin", int.Parse(Reward_AdsCoin.StringValue));
                }

                ConfigValue Remote_NativeAd = FirebaseRemoteConfig.DefaultInstance.GetValue("Remote_NativeAd");
                Debug.Log("Remote_NativeAd: " + Remote_NativeAd.StringValue);
                if (!string.IsNullOrEmpty(Remote_NativeAd.StringValue))
                {
                    PlayerPrefs.SetInt("Remote_NativeAd", int.Parse(Remote_NativeAd.StringValue));
                }

                ConfigValue Open_ADS = FirebaseRemoteConfig.DefaultInstance.GetValue("Open_ADS");
                Debug.Log("Open_ADS: " + Open_ADS.StringValue);
                if (!string.IsNullOrEmpty(Open_ADS.StringValue))
                {
                    PlayerPrefs.SetString("Open_ADS", (Open_ADS.StringValue));
                }

                ConfigValue AD_INGAME = FirebaseRemoteConfig.DefaultInstance.GetValue("AD_INGAME");
                Debug.Log("AD_INGAME: " + AD_INGAME.StringValue);
                if (!string.IsNullOrEmpty(AD_INGAME.StringValue))
                {
                    PlayerPrefs.SetInt("AD_INGAME", int.Parse(AD_INGAME.StringValue));
                }

                ConfigValue Remote_NewPopUp = FirebaseRemoteConfig.DefaultInstance.GetValue("Remote_NewPopUp");
                Debug.Log("Remote_NewPopUp: " + Remote_NewPopUp.StringValue);
                if (!string.IsNullOrEmpty(Remote_NewPopUp.StringValue))
                {
                    PlayerPrefs.SetString("Remote_NewPopUp", (Remote_NewPopUp.StringValue));
                }

                ConfigValue Event_Theme = FirebaseRemoteConfig.DefaultInstance.GetValue("Event_Theme");
                Debug.Log("Event_Theme: " + Event_Theme.StringValue);
                if (!string.IsNullOrEmpty(Event_Theme.StringValue))
                {
                    PlayerPrefs.SetInt("Event_Theme", int.Parse(Event_Theme.StringValue));
                }         
                
                ConfigValue Popup_AfterOpen = FirebaseRemoteConfig.DefaultInstance.GetValue("Popup_AfterOpen");
                Debug.Log("Popup_AfterOpen: " + Popup_AfterOpen.StringValue);
                if (!string.IsNullOrEmpty(Popup_AfterOpen.StringValue))
                {
                    PlayerPrefs.SetInt("Popup_AfterOpen", int.Parse(Popup_AfterOpen.StringValue));
                }       
                        
                
                ConfigValue Noel_progress = FirebaseRemoteConfig.DefaultInstance.GetValue("Noel_progress");
                Debug.Log("Noel_progress: " + Noel_progress.StringValue);
                if (!string.IsNullOrEmpty(Noel_progress.StringValue))
                {
                    PlayerPrefs.SetString("Noel_progress", (Noel_progress.StringValue));
                }
                #endregion


                LoadRule();

            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
        else if (result.Equals(LastFetchStatus.Failure))
        {

            Debug.Log("FAIL TO LOAD");
        }
        else
        {
            Debug.Log("PENDING");
        }

        doneFetch = true;
    }


    //private void SavePref(string name, string value, Type type)
    //{
    //    ac += () => { FirebaseRemoteConfig.DefaultInstance.ActivateAsync(); };
    //    if(type == typeof(string))
    //    {
    //        ac += () => { PlayerPrefs.SetString(name, value); };
    //    }       
        
    //    if(type == typeof(int))
    //    {
    //        ac += () => { PlayerPrefs.SetInt(name, int.Parse(value)); };
    //    }        
        
    //    if(type == typeof(float))
    //    {
    //        ac += () => { PlayerPrefs.SetFloat(name, float.Parse(value)); };
    //    }
    //}

    private async void LoadRule()
    {
        doneFetch = true;
        Debug.Log("Finish Firebase");
        while (MasterControl.Instance == null)
        {
            await Task.Yield();
        }
        MasterControl.Instance.adsController.LoadRule();
    }
    private async void DoLogEvent(string e)
    {
        await Task.Delay(1000);
        float timer = 4f;
        while (!isReady)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                break;
            }
            await Task.Yield();
        }
        try
        {
            Debug.Log("LOG EVENT:" + e);
            FirebaseAnalytics.LogEvent(e);

        }
        catch { }
    }
    #endregion

    //

    public void LogEvent(string eventName)
    {
        
        try
        {
            Debug.Log("LOG " + eventName);
            //StartCoroutine(DoLogEvent(eventName));
            DoLogEvent(eventName);
        }
        catch { }
    }

    public void LogLevel(int level,int mode)
    {
        //StartCoroutine(DoLogEvent("LEVEL_" + mode + "_" + GetLevel(level)));
        DoLogEvent("LEVEL_" + mode + "_" + GetLevel(level));
    }
    public static string GetLevel(int level)
    {
        string lv = "";
        if (level < 10) lv = "00" + level;
        else if (level < 100) lv = "0" + level;
        else lv += level;
        return lv;
    }


    //int updatesBeforeException=5;

    //// Use this for initialization
    //void Start()
    //{
    //    updatesBeforeException = 0;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // Call the exception-throwing method here so that it's run
    //    // every frame update
    //    throwExceptionEvery60Updates();
    //}

    //// A method that tests your Crashlytics implementation by throwing an
    //// exception every 60 frame updates. You should see non-fatal errors in the
    //// Firebase console a few minutes after running your app with this method.
    //void throwExceptionEvery60Updates()
    //{
    //    if (updatesBeforeException > 0)
    //    {
    //        updatesBeforeException--;
    //    }
    //    else
    //    {
    //        // Set the counter to 60 updates
    //        updatesBeforeException = 15;

    //        // Throw an exception to test your Crashlytics implementation
    //        throw new System.Exception("test exception please ignore");
    //    }
    //}

    #region newFirebaseLog
    const string LOG_CONTENT_LEVEL_AD_FORMAT = "ad_format";
    const string LOG_CONTENT_LEVEL_IAP = "iap_sdk";
    const string LOG_CONTENT_LEVEL_AD_REVENUE = "ad_revenue_sdk";
    const string LOG_CONTENT_OPEN_NATIVE_COSTCENTER = "cc_openad_native_revenue";

    const string LOG_CONTENT_LEVEL_MODE = "level_mode";
    const string LOG_CONTENT_LEVEL_SUCCESS = "True";
    const string LOG_CONTENT_LEVEL_FAIL = "False";
    public const string LOG_CONTENT_CC_INTER = "inter";
    public const string LOG_CONTENT_CC_REWARD = "reward";
    public const string LOG_CONTENT_CC_MODE_AD = "ADS";
    public const string LOG_CONTENT_OPEN = "open";
    public const string LOG_CONTENT_NATIVE = "native";

    [SerializeField]
    internal bool isActive = true;


    public void LogLevelStart(string level, string mode)
    {
        Parameter[] LevelStartParameters = {
            new Parameter(FirebaseAnalytics.ParameterLevel, level.ToString()),	// examples: 1, 2, 3, 4, 5
            new Parameter(LOG_CONTENT_LEVEL_MODE, mode.ToString()) // examples: Hard, Easy, Challenge
        };
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, LevelStartParameters);
    }
    // Log Level End
    public void LogLevelEnd(string level, string mode, bool isWin)
    {
        Parameter[] LevelEndParameters = {
            new Parameter(FirebaseAnalytics.ParameterLevel, level.ToString()),
            new Parameter(LOG_CONTENT_LEVEL_MODE, mode.ToString()),
            new Parameter(FirebaseAnalytics.ParameterSuccess, isWin? LOG_CONTENT_LEVEL_SUCCESS: LOG_CONTENT_LEVEL_FAIL)
        };
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd, LevelEndParameters);
    }
    // Log IAP 
    public void LogIAPLevel(string level, string mode, UnityEngine.Purchasing.Product product)
    {
        if (!isActive)
            return;
        Parameter[] IAPRevenueParameters = {
            new Parameter(FirebaseAnalytics.ParameterLevel, level.ToString()),
            new Parameter(LOG_CONTENT_LEVEL_MODE, mode.ToString()),
            new Parameter(FirebaseAnalytics.ParameterValue, (float)product.metadata.localizedPrice),
            new Parameter( FirebaseAnalytics.ParameterCurrency, product.metadata.isoCurrencyCode)
        };
        FirebaseAnalytics.LogEvent(LOG_CONTENT_LEVEL_IAP, IAPRevenueParameters);
    }
    // Log Ad revenue IronSource
    public void LogAdRevenueLevel(string level, string mode, IronSourceImpressionData impressionData)
    {
        Parameter[] AdRevenueParameters = {
            new Parameter(FirebaseAnalytics.ParameterLevel, level.ToString()),
            new Parameter(LOG_CONTENT_LEVEL_MODE, mode.ToString()),
            new Parameter(FirebaseAnalytics.ParameterAdFormat, impressionData.adUnit ?? ""),
            new Parameter(FirebaseAnalytics.ParameterValue,  (float)impressionData.revenue),
            new Parameter(FirebaseAnalytics.ParameterCurrency, "USD")
        };
        FirebaseAnalytics.LogEvent(LOG_CONTENT_LEVEL_AD_REVENUE, AdRevenueParameters);
    }
    // Log Ad revenue Admob, adUnit truyền vào "open" hoặc "native"
    public void LogAdRevenueLevel(int level, GameMode mode, GoogleMobileAds.Api.AdValueEventArgs impressionData, string adUnit)
    {
        Parameter[] AdRevenueParameters = {
            new Parameter(FirebaseAnalytics.ParameterLevel, level.ToString()),
            new Parameter(LOG_CONTENT_LEVEL_MODE, mode.ToString()),
            new Parameter(FirebaseAnalytics.ParameterAdFormat, adUnit),
            new Parameter(FirebaseAnalytics.ParameterValue,  impressionData.AdValue.Value/1000000f),
            new Parameter(FirebaseAnalytics.ParameterCurrency,impressionData.AdValue.CurrencyCode)
        };
        FirebaseAnalytics.LogEvent(LOG_CONTENT_LEVEL_AD_REVENUE, AdRevenueParameters);
    }

    public void CostCenter_OnPaidEvent(IronSourceImpressionData impressionData, int level, string gameMode)
    {
        if (impressionData == null) return;
        double revenue = (float)impressionData.revenue;
        var impressionParameters = new[] {
                  new Parameter(FirebaseAnalytics.ParameterLevel, level.ToString()),
                  new Parameter(LOG_CONTENT_LEVEL_MODE, gameMode),
                  new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdPlatform, "ironSource"),
                  new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdSource, impressionData.adNetwork),
                  new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdUnitName, impressionData.adUnit),
                  new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdFormat, impressionData.adUnit),
                  new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterValue, revenue),
                  new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterCurrency, "USD"),
                  };
        Firebase.Analytics.FirebaseAnalytics.LogEvent(LOG_CONTENT_LEVEL_AD_REVENUE, impressionParameters);
    }

    public void OnPaidEvent(IronSourceImpressionData impressionData)
    {
        if (impressionData == null) return;
        var revenue = (float)impressionData.revenue;
        if (impressionData != null)
        {
            Firebase.Analytics.Parameter[] AdParameters = {
                new Firebase.Analytics.Parameter("ad_platform", "ironSource"),
                new Firebase.Analytics.Parameter("ad_source", impressionData.adNetwork),
                new Firebase.Analytics.Parameter("ad_unit_name", impressionData.instanceName),
                new Firebase.Analytics.Parameter("ad_format", impressionData.adUnit),
                new Firebase.Analytics.Parameter("currency","USD"),
                new Firebase.Analytics.Parameter("value", revenue)
            };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", AdParameters);
        }
    }

    public void CostCenter_OnPaidEvent(GoogleMobileAds.Api.AdValue impressionData, string unit, int level, string gameMode)
    {
        if (impressionData == null) return;
        double revenue = impressionData.Value / 1000000f;
        var impressionParameters = new[] {
        new Parameter(FirebaseAnalytics.ParameterLevel, level.ToString()),
        new Parameter(LOG_CONTENT_LEVEL_MODE, gameMode),
        new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdPlatform, "Admob"),
        new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdSource, "Admob"),
        new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdUnitName, unit),
        new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdFormat, unit),
        new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterValue, revenue),
        new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterCurrency, impressionData.CurrencyCode),
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_revenue_sdk", impressionParameters);
    }

    public void OnPaidEvent(GoogleMobileAds.Api.AdValue impressionData, string unit)
    {
        if (impressionData == null) return;
        double revenue = impressionData.Value / 1000000f;
        var impressionParameters = new[] {
            new Firebase.Analytics.Parameter("ad_platform", "Admob"),
            new Firebase.Analytics.Parameter("ad_source", "Admob"),
            new Firebase.Analytics.Parameter("ad_unit_name", unit),
            new Firebase.Analytics.Parameter("ad_format", unit),
            new Firebase.Analytics.Parameter("value", revenue),
            new Firebase.Analytics.Parameter("currency", impressionData.CurrencyCode),
            };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
    }


    public void LogAdRevenueOpenNative(GoogleMobileAds.Api.AdValue adValueAdmob, string adFormat)
    {
        if (adValueAdmob == null) return;
        double adValue = adValueAdmob.Value / 1000000f;
        string currency = adValueAdmob.CurrencyCode;
        Parameter[] AdRevenueParameters = {
        new Parameter(FirebaseAnalytics.ParameterAdFormat, adFormat),
        new Parameter(FirebaseAnalytics.ParameterValue, adValue),
        new Parameter(FirebaseAnalytics.ParameterCurrency, currency)
                };
        FirebaseAnalytics.LogEvent(LOG_CONTENT_OPEN_NATIVE_COSTCENTER, AdRevenueParameters);
    }
    #endregion
}
public enum GameMode { 

}