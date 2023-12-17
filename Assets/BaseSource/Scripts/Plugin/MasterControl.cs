using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class MasterControl : MonoBehaviour
{
    public static MasterControl Instance;
    [ReadOnly]
    public MasterAdsController adsController;
    [ReadOnly]
    public Purchaser purchaser;
    //[ReadOnly]
    //public FireBaseManager firebaseManager;
    [ReadOnly]
    [SerializeField]
    private string [] timeServer;
    bool isInit = false;
    [ReadOnly]
    public bool isConnected = false;
    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            adsController = GetComponentInChildren<MasterAdsController>();
            adsController.masterControl = this;
            purchaser = GetComponentInChildren<Purchaser>();
            purchaser.masterControl = this;
            //firebaseManager = GetComponentInChildren<FireBaseManager>();
#if !UNITY_IOS
            StartCoroutine(DoFindBestHost());
#endif
        }
        else
        {
            Destroy(gameObject);
        }
        Application.lowMemory += OnLowMemory;
    }

    private void OnLowMemory()
    {
        System.GC.Collect();
    }

    void Start()
    {
            Init();

            //CheckInternet();
    }
    //private void OnApplicationFocus(bool focus)
    //{
    //    if (focus)
    //    {
    //        Debug.Log("---------------------- FOCUS: CHECK INTERNET ");
    //        //CheckInternet();
    //    }
    //}
    public void Init()
    {
        if (Instance.isInit) return;
        StartCoroutine(DelayInit());

    }

    IEnumerator DelayInit()
    {
        yield return null;
        Instance.isInit = true;
        adsController.Init();
        //firebaseManager.Init();
        yield return null;
        purchaser.Init();
    }
  
   
    public bool CheckAppInstallation(string bundleId)
    {
#if UNITY_EDITOR
        return false;
#elif UNITY_ANDROID
        bool installed = false;
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = curActivity.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
            if (launchIntent == null)
                installed = false;

            else
                installed = true;
        }

        catch (System.Exception e)
        {
            installed = false;
        }
        return installed;

#elif UNITY_IOS
        return false;
#else
        return false;
#endif
    }

    
   
    public void CheckInternet()
    {
#if !UNITY_IOS
        CheckInternet(res => { }, false);
#endif
    }
    public void CheckInternet(Action<bool> action,bool useWaitingUI=false)
    {
#if !UNITY_IOS
        StartCoroutine(DoCheckInternet(action,useWaitingUI));
#endif
    }
    [ReadOnly]
    [SerializeField]
    private string[] hosts = { "1.1.1.1", "8.8.8.8", "180.76.76.76" };
    [ReadOnly]
    [SerializeField]
    private string bestHost = "1.1.1.1";
    IEnumerator DoFindBestHost()
    {
        yield return null;
        int pingTime = int.MaxValue;
        for (int i = 0; i < hosts.Length; i++)
        {
            Ping ping = new Ping(hosts[i]);

            float timeOutCooldown = 0;
            while (!ping.isDone && timeOutCooldown < 5)
            {
                
                timeOutCooldown += Time.unscaledDeltaTime;
            }

            if (ping.isDone && pingTime > ping.time)
            {
                pingTime = ping.time;
                bestHost = hosts[i];
            }
        }
        Debug.Log("Ping Host: " + bestHost);
    }
    IEnumerator DoCheckInternet(Action<bool> action,bool useWaitingUI=false)
    {
        yield return new WaitForSecondsRealtime(2);
        if (Application.internetReachability.Equals(NetworkReachability.NotReachable))
        {
            isConnected = false;
            action(false);
            yield break;
        }
        Ping ping = new Ping(bestHost);
        
        float timeOutCooldown = 0;
        bool check = false;
        while (!ping.isDone && timeOutCooldown < 5)
        {
            yield return null;
            timeOutCooldown += Time.unscaledDeltaTime;
            if (useWaitingUI)
            {
                if (!check &&timeOutCooldown > 0.3f )
                {
                    check = true;
                    Debug.Log("NoInternet");
                    /* Controller.Instance.pleaseWaitPanel.SetActive(true);*/
                }
            }
        }
        Debug.Log("IS DONE: " + ping.isDone + " " + ping.time + " " + bestHost);
        if (ping.isDone)
        {
            //isConnected = true;
            StartCoroutine(checkInternetConnection(action,useWaitingUI));
            //action(true);
        }
        else
        {
            isConnected = false;
            action(false);
        }
    }
    public IEnumerator checkInternetConnection(Action<bool> action,bool useWaitingUI)
    {
        WWW www = new WWW("http://google.com");

        bool check = false;
        float timeOut = 5;
        while (!www.isDone &&timeOut>0)
        {
            if (www.bytesDownloaded >= 2)
            {
                break;
            }
            if (!check &&useWaitingUI && timeOut < 4.7f)
            {
                check = true;
                Debug.Log("NoInternet");
               /* Controller.Instance.pleaseWaitPanel.SetActive(true);*/
            }
            
            timeOut -= Time.deltaTime;
            yield return null;
        }

        //yield return www;
        Debug.Log("1: " + www.bytesDownloaded);
        if (www.bytesDownloaded < 1)
        {
            WWW www2 = new WWW("https://www.baidu.com/");
            timeOut = 5;
            while (!www2.isDone && timeOut > 0)
            {
                if (www2.bytesDownloaded >= 2)
                {
                    break;
                }
                
                timeOut -= Time.deltaTime;
                yield return null;
            }
            Debug.Log("2 : " + www2.bytesDownloaded);

            if (www2.bytesDownloaded < 1)
            {
                WWW www3 = new WWW("https://www.baidu.com/");
                yield return www3;
                Debug.Log("3 : " + www3.bytesDownloaded);

                if (www3.bytesDownloaded < 1)
                {
                    WWW www4 = new WWW("http://worldclockapi.com/api/json/utc/now");
                    yield return www4;
                    Debug.Log("4 : " + www4.bytesDownloaded);

                    if (www4.bytesDownloaded < 1)
                    {
                        action(false);
                    }
                    else
                    {
                        action(true);
                    }
                }
                else
                {
                    action(true);
                }
            }
            else
            {
                action(true);
            }
        }
        else
        {
            action(true);
        }
    }


    [Space]
    [InfoBox("=========> <size=30>WARNING</size>  <========\n\n <size=15>Unlock All</size>: build test level. No ads\n\n<size=15>Ads Test</size>: build test logic ads. Keys ads test\n\n<size=15>Ads Production</size>: build production. Keys ads production", InfoMessageType.Warning)]
    [Space]
    public AdsState adsState;
    #region IAP
    [Space]
    [Space]
    [InfoBox("Mảng key IAP. <size=15>CHÚ Ý:</size> key đầu tiên luôn là RemoveAds",InfoMessageType.Warning)]
    public ProductKey[] productKeys;
    private string[] priceTemplates = { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
    private string[] prices = { "2.99$", "4.99$", "0.99$", "0.99$", "0.99$", "1.99$", "2.99$", "4.99$", "7.99$", "3.99$", "9.99$", };
    public delegate void OnPurchasedRefresh();
    public OnPurchasedRefresh onPurchaseRefresh;
    public void OnPurchased(UnityEngine.Purchasing.Product product)
    {
        OnPurchased(product.definition.id);
        Debug.Log("PURCHASED: " + product.definition.id);
    }
    public void OnFailedToPurchase(UnityEngine.Purchasing.Product product, UnityEngine.Purchasing.PurchaseFailureReason reason)
    {
        //Debug.Log("FAILED TO PURCHASED: " + product.definition.id + " " + reason.ToString());
        if (reason.Equals(UnityEngine.Purchasing.PurchaseFailureReason.DuplicateTransaction))
        {
            if (product != null)
            {
                if (IsNoneComsumProduct(product.definition.id) && Purchaser.Instance.HasReceipt(product.definition.id))
                {
                    OnPurchased(product.definition.id);
                }
            }
        }

    }

    private bool IsNoneComsumProduct(string id)
    {
        for(int i =0; i< productKeys.Length; i++)
        {
            if (id.Equals(productKeys[i].key) && productKeys[i].type == UnityEngine.Purchasing.ProductType.NonConsumable)
                return true;
        }
        return false;
    }
    public void OnPurchased(string item)
    {
        if (GameManger.instance == null) return;
        Debug.Log("MASTERCONTROLL: ONPURCHASED " + item);
        PlayerPrefs.SetInt("UserIAP", 1); 
        if (item.Equals(productKeys[0].key))
        {
            Debug.Log("buy");
            GameConfig.instance.GetDataPack().BuyBack(TypeGame.Easy, 1);
            MyEvent.BuyPack?.Invoke();
            ShopUI.instance.popUpConfirm.Show();
            DataGame.SetInt(DataGame.IapPack + productKeys[0].key, 1);
        }
        if (item.Equals(productKeys[1].key))
        {
            Debug.Log("buy");
            GameConfig.instance.GetDataPack().BuyBack(TypeGame.Easy, 2);
            MyEvent.BuyPack?.Invoke();
            ShopUI.instance.popUpConfirm.Show();
            DataGame.SetInt(DataGame.IapPack + productKeys[1].key, 1);
        }
        if (item.Equals(productKeys[2].key))
        {
            Debug.Log("buy");
            GameConfig.instance.GetDataPack().BuyBack(TypeGame.Easy, 3);
            MyEvent.BuyPack?.Invoke();
            ShopUI.instance.popUpConfirm.Show();
            DataGame.SetInt(DataGame.IapPack + productKeys[2].key, 1);
        }
        if (item.Equals(productKeys[3].key))
        {
            Debug.Log("buy");
            GameConfig.instance.GetDataPack().BuyBack(TypeGame.Medium, 1);
            MyEvent.BuyPack?.Invoke();
            ShopUI.instance.popUpConfirm.Show();
            DataGame.SetInt(DataGame.IapPack + productKeys[3].key, 1);
        }
        if (item.Equals(productKeys[4].key))
        {
            Debug.Log("buy");
            GameConfig.instance.GetDataPack().BuyBack(TypeGame.Medium, 2);
            MyEvent.BuyPack?.Invoke();
            ShopUI.instance.popUpConfirm.Show();
            DataGame.SetInt(DataGame.IapPack + productKeys[4].key, 1);
        }
        if (item.Equals(productKeys[5].key))
        {
            Debug.Log("buy");
            GameConfig.instance.GetDataPack().BuyBack(TypeGame.Difficult, 1);
            MyEvent.BuyPack?.Invoke();
            ShopUI.instance.popUpConfirm.Show();
            DataGame.SetInt(DataGame.IapPack + productKeys[5].key, 1);
        }
        if (item.Equals(productKeys[6].key))
        {
            Debug.Log("buy");
            GameConfig.instance.GetDataPack().BuyBack(TypeGame.Difficult, 2);
            MyEvent.BuyPack?.Invoke();
            ShopUI.instance.popUpConfirm.Show();
            DataGame.SetInt(DataGame.IapPack + productKeys[6].key, 1);
        }
        if (item.Equals(productKeys[7].key))
        {
            Debug.Log("buy");
            GameConfig.instance.GetDataPack().BuyBack(TypeGame.Genius, 1);
            MyEvent.BuyPack?.Invoke();
            ShopUI.instance.popUpConfirm.Show();
            DataGame.SetInt(DataGame.IapPack + productKeys[7].key, 1);
        }
        if (item.Equals(productKeys[8].key))
        {
            Debug.Log("buy");
            GameConfig.instance.GetDataPack().BuyBack(TypeGame.Genius, 2);
            MyEvent.BuyPack?.Invoke();
            ShopUI.instance.popUpConfirm.Show();
            DataGame.SetInt(DataGame.IapPack + productKeys[8].key, 1);
        }



        onPurchaseRefresh?.Invoke();
    }

    

    public void OnExpired(string item)
    {

        //if (item.Equals(productKeys[1].key))
        //{
        //    //if (PrefInfo.isSub(0))
        //    //{
        //    //    Debug.LogWarning("Expired " + item);

        //    //    if (PrefInfo.GetCurID(6) == 15)
        //    //    {
        //    //        PrefInfo.SetCurID(6, 0);
        //    //    }

        //    //    if (!Purchaser.Instance.HasReceipt(productKeys[0].key))
        //    //    {
        //    //        PrefInfo.SetAd(true);
        //    //    }
        //    //    PrefInfo.SetExpired(0, true);
        //    //}

        //}

        onPurchaseRefresh?.Invoke();
    }
    public void OnRestore(string item)
    {
        Debug.Log("MASTERCONTROLL: ONRESTORED " + item);
        //Controller.Instance.pleaseWaitPanel.SetActive(false);
        if (item.Equals(productKeys[0].key))
        {
            PrefInfo.SetAd(false);
            Debug.Log("Restore");
            /*//SpecialManager.Instance.UnlockedCombo(3);
            HideBanner();
            if (Home.Instance != null)
            {
                Home.Instance.noAdsBtn.SetActive(false);
                Home.Instance.specialBtn.SetActive(false);
            }*/
            //if (GameUI.Instance != null)
            //    GameUI.Instance.CheckAd();
            //if (NewResultPanel.Instance != null)
            //    NewResultPanel.Instance.CheckAd();
        }


        onPurchaseRefresh?.Invoke();
    }

    public string GetPrice(int id)
    {
        string price = prices[id];
        try
        {
            if (priceTemplates[id] == "0")
            {
                try
                {
                    priceTemplates[id] = purchaser.GetPrices()[id];
                    price = priceTemplates[id];

                }
                catch
                {

                    priceTemplates[id] = "0";
                    return prices[id];

                }
            }
            else
            {
                price = priceTemplates[id];
            }

        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }

        return price;

    }

    //public decimal GetItemPrice(int id)
    //{
    //    try
    //    {
    //        return purchaser.GetPrice(id);
    //        return 0;
    //    }
    //    catch (Exception)
    //    {
    //        return decimal.Parse("0");
    //    }
    //}
    #endregion


    public void OpenURL(string link)
    {
        Application.OpenURL(link);

    }
    public void ToStore()
    {
    }

    #region AD
    private Action<bool> rewardCallBack;
    public void ShowRewardedAd(Action<bool> rewardCallBack)
    {
        this.rewardCallBack = rewardCallBack;
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No internet");
            return;
        }
        adsController.ShowRewardedVideo();

    }
    public void OnRewardedAd()
    {
        if (rewardCallBack != null)
        {
            Debug.Log("ON REWARD CALLBACK");
            rewardCallBack(true);
            rewardCallBack = null;
        }
    }

    public void OnFail()
    {
        if (rewardCallBack != null)
        {
            rewardCallBack(false);
        }
    }
    public bool ShowInterstitialAd(Panel panel = null)
    {
        //interShowSucceedEvent = action;
        bool res = adsController.ShowInterstitial(panel);
        return res;
    }
    public void HideBanner()
    {
        adsController.HideBanner();
    }
    public void ShowBanner()
    {
        adsController.ShowBanner();
    }

    #endregion


    #region NativeAd

    public object GetNativeAd()
    {
        return adsController.GetNativeAd();
    }

    public bool IsNativeAdAvailable()
    {
        return adsController.IsNativeAdAvailable();
    }


    #endregion

    #region policy
    public void ShowBGBanner()
    {
        StopCoroutine(DoShowBGBanner());
        StartCoroutine(DoShowBGBanner());
    }

    IEnumerator DoShowBGBanner()
    {
        yield return new WaitUntil(() => GameManger.instance != null);
        Debug.Log("Show banner ");
    }

    public void HideBGBanner()
    {
        StopCoroutine(DoShowBGBanner());
        StopCoroutine(DoHideBGBanner());
        StartCoroutine(DoHideBGBanner());
    }
    IEnumerator DoHideBGBanner()
    {
        yield return new WaitUntil(() => GameManger.instance != null);
        Debug.Log("Hide Banner");
    }

    #endregion

}

[System.Serializable]
public struct ProductKey
{
    [BoxGroup("Product", centerLabel: true)]
    public UnityEngine.Purchasing.ProductType type;
    [BoxGroup("Product")]
    public string key;
}
public enum AdsState {
    UnlockAll,
    
    AdsProduction,

}