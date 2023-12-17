using com.adjust.sdk;
using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
//using GoogleMobileAds.Common;
using Sirenix.OdinInspector;
using UnityEngine;


public class IronSouceManager : MonoBehaviour, IAdsController
{
	public string uniqueUserId = "demoUserUnity";
	public string INTERSTITIAL_INSTANCE_ID = "0";
	public string REWARDED_INSTANCE_ID = "0";
	public string appKey;
	public MasterAdsController masterAdsController;
	//public static IronSouceManager Instance;

	bool check1, check2;
	bool isTrackingEndInter = false;
	bool isTrackingEndReward = false;
	void Awake()
	{
		LoadQuitTime();
	}
	public IAdsController next;

	public void Init(MasterAdsController ctr, Action callback)
	{
		this.callback = callback;

//#if UNITY_ANDROID
//		string appKey = "85460dcd";
//#elif UNITY_IPHONE
//		string appKey = "8545d445";
//#else
//        string appKey = "unexpected_platform";
//#endif
		Debug.Log("unity-script: MyAppStart Start called");

        //Dynamic config example

        MobileAds.Initialize(initStatus =>
        {
			AppStateEventNotifier.AppStateChanged += OnAppStateChanged;


			LoadOpenAppAds();
			LoadNativeAd();


        });

		IronSourceAdQuality.Initialize(appKey);
		IronSourceConfig.Instance.setClientSideCallbacks(true);

		string id = IronSource.Agent.getAdvertiserId();
		Debug.Log("unity-script: IronSource.Agent.getAdvertiserId : " + id);

		Debug.Log("unity-script: IronSource.Agent.validateIntegration");
		IronSource.Agent.validateIntegration();

		Debug.Log("unity-script: unity version" + IronSource.unityVersion());

		LoadBannerEvent();
		LoadInterEvent();
		LoadRewardEvent();

		// SDK init
		IronSourceEvents.onSdkInitializationCompletedEvent += InitializationCompleteEvent;
		IronSourceEvents.onImpressionDataReadyEvent += ImpressionSuccessEvent;
		Debug.Log("unity-script: IronSource.Agent.init: ");
		IronSource.Agent.init(appKey);

		//this.callback = callback;
        callback?.Invoke();
	}

	List<IronSourceImpressionData> listImpressionData = new List<IronSourceImpressionData>();
	bool check3 = false, checkAdmobOpen = false, checkAdmobNative = false;
	AdValue adValueAdmobOpen;
	AdValue adValueAdmobNative;

	void Update()
	{
		if (isTrackingEndInter)
		{
			isTrackingEndInter = false;
			FireBaseManager.Instance.LogLevelEnd(FireBaseManager.LOG_CONTENT_CC_INTER, FireBaseManager.LOG_CONTENT_CC_MODE_AD, true);
		}
		if (isTrackingEndReward)
		{
			isTrackingEndReward = false;
			FireBaseManager.Instance.LogLevelEnd(FireBaseManager.LOG_CONTENT_CC_REWARD, FireBaseManager.LOG_CONTENT_CC_MODE_AD, true);
		}


		if (isOnReward)
		{
			isOnReward = false;
			masterAdsController.OnRewarded();
		}
		if (check3)
		{
			if (listImpressionData.Count > 0)
			{
				IronSourceImpressionData impressionData = listImpressionData[0];
				//OnPaidEvent là hàm tracking rev ad_impression
				FireBaseManager.Instance.OnPaidEvent(impressionData);
				//Xem lại lưu ý về level và level_mode ở trên
				/*if (SortGameController.Instance.CurrentLevel != -1)
				{
					FireBaseManager.Instance.CostCenter_OnPaidEvent(impressionData, SortGameController.Instance.CurrentLevel, SortGameController.Instance.CurrentMode.ToString());
				}
				else
				{
					FireBaseManager.Instance.CostCenter_OnPaidEvent(impressionData, SortGameController.Instance.GetTotalMainLevel(), SortGameController.Instance.CurrentMode.ToString());
				}*/
				try
				{
					if (impressionData != null)
					{
						AdjustAdRevenue adjustRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceIronSource);
						adjustRevenue.setRevenue((float)impressionData.revenue, "USD");
						Adjust.trackAdRevenue(adjustRevenue);
					}
				}
				catch { }
				listImpressionData.RemoveAt(0);
			}
			else
			{
				check3 = false;
			}
		}
		if (checkAdmobOpen)
		{
			checkAdmobOpen = false;
			if (adValueAdmobOpen != null)
			{
				//OnPaidEvent là hàm tracking rev ad_impression
				FireBaseManager.Instance.OnPaidEvent(adValueAdmobOpen, FireBaseManager.LOG_CONTENT_OPEN);
				//Xem lại lưu ý về level và level_mode ở trên
				/*if (SortGameController.Instance.CurrentLevel != -1)
				{
					FireBaseManager.Instance.CostCenter_OnPaidEvent(adValueAdmobOpen, FireBaseManager.LOG_CONTENT_OPEN, SortGameController.Instance.CurrentLevel, SortGameController.Instance.CurrentMode.ToString());
				}
				else
				{
					FireBaseManager.Instance.CostCenter_OnPaidEvent(adValueAdmobOpen, FireBaseManager.LOG_CONTENT_OPEN, SortGameController.Instance.GetTotalMainLevel(), SortGameController.Instance.CurrentMode.ToString());
				}*/
				//Hàm ở dòng dưới sử dụng để tracking revenue open ad cho Additional Ad Revenue
				FireBaseManager.Instance.LogAdRevenueOpenNative(adValueAdmobOpen, "open");
				try
				{
					if (adValueAdmobOpen != null)
					{
						AdjustAdRevenue adjustRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
						adjustRevenue.setRevenue(adValueAdmobOpen.Value / 1000000f, adValueAdmobOpen.CurrencyCode);
						Adjust.trackAdRevenue(adjustRevenue);
					}
				}
				catch { }
				adValueAdmobOpen = null;
			}
		}
		if (checkAdmobNative)
		{
			checkAdmobNative = false;
			if (adValueAdmobNative != null)
			{
				//OnPaidEvent là hàm tracking rev ad_impression
				FireBaseManager.Instance.OnPaidEvent(adValueAdmobNative, FireBaseManager.LOG_CONTENT_NATIVE);
				//Xem lại lưu ý về level và level_mode ở trên
				/*if (SortGameController.Instance.CurrentLevel != -1)
				{
					FireBaseManager.Instance.CostCenter_OnPaidEvent(adValueAdmobNative, FireBaseManager.LOG_CONTENT_NATIVE, SortGameController.Instance.CurrentLevel, SortGameController.Instance.CurrentMode.ToString());
				}
				else
				{
					FireBaseManager.Instance.CostCenter_OnPaidEvent(adValueAdmobNative, FireBaseManager.LOG_CONTENT_NATIVE, SortGameController.Instance.GetTotalMainLevel(), SortGameController.Instance.CurrentMode.ToString());
				}*/
				//Hàm ở dòng dưới sử dụng để tracking revenue open ad cho Additional Ad Revenue
				FireBaseManager.Instance.LogAdRevenueOpenNative(adValueAdmobNative, "native");
				try
				{
					if (adValueAdmobNative != null)
					{
						AdjustAdRevenue adjustRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
						adjustRevenue.setRevenue(adValueAdmobNative.Value / 1000000f, adValueAdmobNative.CurrencyCode);
						Adjust.trackAdRevenue(adjustRevenue);
					}
				}
				catch { }
				adValueAdmobNative = null;
			}
			else
			{
				checkAdmobNative = false;
			}
		}
	}

	private void ImpressionSuccessEvent(IronSourceImpressionData impressionData)
	{
		Debug.Log("unity-script:  ImpressionSuccessEvent impressionData = " + impressionData);

		if (impressionData != null)
		{
			listImpressionData.Add(impressionData);
			check3 = true;
		}
		else
		{
			Debug.Log("Impression success but data null");
		}
		//if (impressionData != null)
		//{
		//	Firebase.Analytics.Parameter[] AdParameters = {
		//		new Firebase.Analytics.Parameter("ad_platform", "ironSource"),
		//			new Firebase.Analytics.Parameter("ad_source", impressionData.adNetwork),
		//			new Firebase.Analytics.Parameter("ad_unit_name", impressionData.adUnit),
		//	new Firebase.Analytics.Parameter("ad_format", impressionData.instanceName),
		//			new Firebase.Analytics.Parameter("currency","USD"),
		//			new Firebase.Analytics.Parameter("value", (float)impressionData.revenue)
		//};
		//	Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", AdParameters);
		//}

		//      AdjustAdRevenue adjustEvent = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceIronSource);
		//      //most important is calling setRevenue with two parameters
		//      adjustEvent.setRevenue((float)impressionData.revenue, "USD");
		//      //Sent event to Adjust server
		//      Adjust.trackAdRevenue(adjustEvent);

	}



	Action callback;
	private void InitializationCompleteEvent()
    {
		//IronSource.Agent.validateIntegration();
		Debug.Log("unity-script: IronSource.Agent.inited");
		callback?.Invoke();
		if (PrefInfo.IsUsingAd() && MasterControl.Instance.adsState== AdsState.AdsProduction)
		{
            //masterAdsController.currentLoadedBannerCtr = this;
            //masterAdsController.ShowBanner();
            //IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        }
		
	}
	public void LoadBannerEvent()
	{
		// Add Banner Events
		IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
		IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
		IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
		IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
		IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
		IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
	}
	//public void UnLoadBannerEvent()
 //   {
	//	IronSourceEvents.onBannerAdLoadedEvent -= BannerAdLoadedEvent;
	//	IronSourceEvents.onBannerAdLoadFailedEvent -= BannerAdLoadFailedEvent;
	//	IronSourceEvents.onBannerAdClickedEvent -= BannerAdClickedEvent;
	//	IronSourceEvents.onBannerAdScreenPresentedEvent -= BannerAdScreenPresentedEvent;
	//	IronSourceEvents.onBannerAdScreenDismissedEvent -= BannerAdScreenDismissedEvent;
	//	IronSourceEvents.onBannerAdLeftApplicationEvent -= BannerAdLeftApplicationEvent;
	//}
	public void LoadInterEvent()
	{
		// Add Interstitial Events
		IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
		IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
		IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
		IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
		IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
		IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
		IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

		// Add Interstitial DemandOnly Events
		IronSourceEvents.onInterstitialAdReadyDemandOnlyEvent += InterstitialAdReadyDemandOnlyEvent;
		IronSourceEvents.onInterstitialAdLoadFailedDemandOnlyEvent += InterstitialAdLoadFailedDemandOnlyEvent;
		IronSourceEvents.onInterstitialAdShowFailedDemandOnlyEvent += InterstitialAdShowFailedDemandOnlyEvent;
		IronSourceEvents.onInterstitialAdClickedDemandOnlyEvent += InterstitialAdClickedDemandOnlyEvent;
		IronSourceEvents.onInterstitialAdOpenedDemandOnlyEvent += InterstitialAdOpenedDemandOnlyEvent;
		IronSourceEvents.onInterstitialAdClosedDemandOnlyEvent += InterstitialAdClosedDemandOnlyEvent;

	}
	//public void UnloadInterEvent()
 //   {
	//	// Add Interstitial Events
	//	IronSourceEvents.onInterstitialAdReadyEvent -= InterstitialAdReadyEvent;
	//	IronSourceEvents.onInterstitialAdLoadFailedEvent -= InterstitialAdLoadFailedEvent;
	//	IronSourceEvents.onInterstitialAdShowSucceededEvent -= InterstitialAdShowSucceededEvent;
	//	IronSourceEvents.onInterstitialAdShowFailedEvent -= InterstitialAdShowFailedEvent;
	//	IronSourceEvents.onInterstitialAdClickedEvent -= InterstitialAdClickedEvent;
	//	IronSourceEvents.onInterstitialAdOpenedEvent -= InterstitialAdOpenedEvent;
	//	IronSourceEvents.onInterstitialAdClosedEvent -= InterstitialAdClosedEvent;

	//	// Add Interstitial DemandOnly Events
	//	IronSourceEvents.onInterstitialAdReadyDemandOnlyEvent -= InterstitialAdReadyDemandOnlyEvent;
	//	IronSourceEvents.onInterstitialAdLoadFailedDemandOnlyEvent -= InterstitialAdLoadFailedDemandOnlyEvent;
	//	IronSourceEvents.onInterstitialAdShowFailedDemandOnlyEvent -= InterstitialAdShowFailedDemandOnlyEvent;
	//	IronSourceEvents.onInterstitialAdClickedDemandOnlyEvent -= InterstitialAdClickedDemandOnlyEvent;
	//	IronSourceEvents.onInterstitialAdOpenedDemandOnlyEvent -= InterstitialAdOpenedDemandOnlyEvent;
	//	IronSourceEvents.onInterstitialAdClosedDemandOnlyEvent -= InterstitialAdClosedDemandOnlyEvent;
	//}
	public void LoadRewardEvent()
	{
		//Add Rewarded Video Events
		IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
		IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
		IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
		IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
		IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
		IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
		IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
		IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
		IronSourceEvents.onRewardedVideoAdLoadFailedEvent += RewardedVideoLoadedFailedEvent;
		IronSourceEvents.onRewardedVideoAdReadyEvent += RewardedVideoLoadedReadyEvent;

		//Add Rewarded Video DemandOnly Events
		IronSourceEvents.onRewardedVideoAdOpenedDemandOnlyEvent += RewardedVideoAdOpenedDemandOnlyEvent;
		IronSourceEvents.onRewardedVideoAdClosedDemandOnlyEvent += RewardedVideoAdClosedDemandOnlyEvent;
		IronSourceEvents.onRewardedVideoAdLoadedDemandOnlyEvent += this.RewardedVideoAdLoadedDemandOnlyEvent;
		IronSourceEvents.onRewardedVideoAdRewardedDemandOnlyEvent += RewardedVideoAdRewardedDemandOnlyEvent;
		IronSourceEvents.onRewardedVideoAdShowFailedDemandOnlyEvent += RewardedVideoAdShowFailedDemandOnlyEvent;
		IronSourceEvents.onRewardedVideoAdClickedDemandOnlyEvent += RewardedVideoAdClickedDemandOnlyEvent;
		IronSourceEvents.onRewardedVideoAdLoadFailedDemandOnlyEvent += this.RewardedVideoAdLoadFailedDemandOnlyEvent;
	}



    //public void UnloadRewardEvent()
    //   {
    //	//Add Rewarded Video Events
    //	IronSourceEvents.onRewardedVideoAdOpenedEvent -= RewardedVideoAdOpenedEvent;
    //	IronSourceEvents.onRewardedVideoAdClosedEvent -= RewardedVideoAdClosedEvent;
    //	IronSourceEvents.onRewardedVideoAvailabilityChangedEvent -= RewardedVideoAvailabilityChangedEvent;
    //	IronSourceEvents.onRewardedVideoAdStartedEvent -= RewardedVideoAdStartedEvent;
    //	IronSourceEvents.onRewardedVideoAdEndedEvent -= RewardedVideoAdEndedEvent;
    //	IronSourceEvents.onRewardedVideoAdRewardedEvent -= RewardedVideoAdRewardedEvent;
    //	IronSourceEvents.onRewardedVideoAdShowFailedEvent -= RewardedVideoAdShowFailedEvent;
    //	IronSourceEvents.onRewardedVideoAdClickedEvent -= RewardedVideoAdClickedEvent;

    //	//Add Rewarded Video DemandOnly Events
    //	IronSourceEvents.onRewardedVideoAdOpenedDemandOnlyEvent -= RewardedVideoAdOpenedDemandOnlyEvent;
    //	IronSourceEvents.onRewardedVideoAdClosedDemandOnlyEvent -= RewardedVideoAdClosedDemandOnlyEvent;
    //	IronSourceEvents.onRewardedVideoAdLoadedDemandOnlyEvent -= this.RewardedVideoAdLoadedDemandOnlyEvent;
    //	IronSourceEvents.onRewardedVideoAdRewardedDemandOnlyEvent -= RewardedVideoAdRewardedDemandOnlyEvent;
    //	IronSourceEvents.onRewardedVideoAdShowFailedDemandOnlyEvent -= RewardedVideoAdShowFailedDemandOnlyEvent;
    //	IronSourceEvents.onRewardedVideoAdClickedDemandOnlyEvent -= RewardedVideoAdClickedDemandOnlyEvent;
    //	IronSourceEvents.onRewardedVideoAdLoadFailedDemandOnlyEvent -= this.RewardedVideoAdLoadFailedDemandOnlyEvent;
    //}


    void DelayActiveBackBtn()
    {
		Debug.Log("DelayActiveBackBtn()");
		
    }

    void OnApplicationPause(bool isPaused)
	{
		Debug.Log("unity-script: OnApplicationPause = " + isPaused);
		IronSource.Agent.onApplicationPause(isPaused);
	}

	//Banner Events
	void BannerAdLoadedEvent()
	{
		Debug.Log("unity-script: I got BannerAdLoadedEvent");
		masterAdsController.currentLoadedBannerCtr = this;
		masterAdsController.failedBanner = false;


	}

	void BannerAdLoadFailedEvent(IronSourceError error)
	{
		//masterAdsController.currentLoadedBannerCtr = null;
		masterAdsController.failedBanner = true;
        Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
		//MessagePanel.Instance.SetUp(error.getCode() + ", description : " + error.getDescription());
	}

	void BannerAdClickedEvent()
	{
		Debug.Log("unity-script: I got BannerAdClickedEvent");
	}

	void BannerAdScreenPresentedEvent()
	{
		Debug.Log("unity-script: I got BannerAdScreenPresentedEvent");
		FireEventAdPrepare("Banner");
	}


	void BannerAdScreenDismissedEvent()
	{
		Debug.Log("unity-script: I got BannerAdScreenDismissedEvent");
		masterAdsController.failedBanner = true;
	}

	void BannerAdLeftApplicationEvent()
	{
		Debug.Log("unity-script: I got BannerAdLeftApplicationEvent");
	}

	/************* Interstitial API *************/
	public void LoadInterstitialButtonClicked()
	{
		Debug.Log("unity-script: LoadInterstitialButtonClicked");
		
		//LoadInterEvent();
		IronSource.Agent.loadInterstitial();

		//DemandOnly
		// LoadDemandOnlyInterstitial ();

	}

	public void ShowInterstitialButtonClicked()
	{
		Debug.Log("unity-script: ShowInterstitialButtonClicked");
		if (IronSource.Agent.isInterstitialReady())
		{
			isAD = true;
			//if(Controller.Instance !=null && Controller.Instance.GetIsuseAdBreak)
   //         {
			//	Controller.Instance.ShowAdBreak(() =>
			//	{
			//		IronSource.Agent.showInterstitial();
			//	});
			//}
			//else
			//{
				
			//}

			IronSource.Agent.showInterstitial();

			FireEventAdPrepare("Inter");
		}
		else
		{

			Debug.Log("unity-script: IronSource.Agent.isInterstitialReady - False");
			FireEventAdNoFill("Inter");
		}

		// DemandOnly
		// ShowDemandOnlyInterstitial ();
	}

	void LoadDemandOnlyInterstitial()
	{
		Debug.Log("unity-script: LoadDemandOnlyInterstitialButtonClicked");
		IronSource.Agent.loadISDemandOnlyInterstitial(INTERSTITIAL_INSTANCE_ID);
	}

	void ShowDemandOnlyInterstitial()
	{
		Debug.Log("unity-script: ShowDemandOnlyInterstitialButtonClicked");
		if (IronSource.Agent.isISDemandOnlyInterstitialReady(INTERSTITIAL_INSTANCE_ID))
		{
			IronSource.Agent.showISDemandOnlyInterstitial(INTERSTITIAL_INSTANCE_ID);
		}
		else
		{
			Debug.Log("unity-script: IronSource.Agent.isISDemandOnlyInterstitialReady - False");
		}
	}

	/************* Interstitial Delegates *************/
	void InterstitialAdReadyEvent()
	{
		Debug.Log("unity-script: I got InterstitialAdReadyEvent");
		masterAdsController.currentLoadedInterstitialCtr = this;


	}

	void InterstitialAdLoadFailedEvent(IronSourceError error)
	{
		Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
		masterAdsController.InterstitialCallback(false);


		if (error.getDescription().ToLower().Contains("no fill"))
		{

		}
	}

	void InterstitialAdShowSucceededEvent()
	{
		Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
		//Controller.Instance.HideAdBreak();
		check1 = true;
	}

	void InterstitialAdShowFailedEvent(IronSourceError error)
	{
		isAD = false;

		isTrackingEndInter = true;

		//Controller.Instance.HideAdBreak();
		Time.timeScale = 1;

		Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
	}

	void InterstitialAdClickedEvent()
	{
		Debug.Log("unity-script: I got InterstitialAdClickedEvent");

		//if (PrefInfo.ShowOpenAdinAd)
		//	DoShowOpenAd();
	}

	void InterstitialAdOpenedEvent()
	{
		Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
		//SoundManager.Instance.SetSoundTypeOnOff(SoundManager.SoundType.Music, false);
		//SoundManager.Instance.ChangePitch(0);
		Time.timeScale = 0;
	}

	void InterstitialAdClosedEvent()
	{
		Debug.Log("unity-script: I got InterstitialAdClosedEvent");
		isTrackingEndInter = true;
		masterAdsController.InterstitialCallback(true);
		//Controller.Instance.HideAdBreak();
		Time.timeScale = 1;
		isAD = false;
        //SoundManager.Instance.SetSoundTypeOnOff(SoundManager.SoundType.Music, true);
        //SoundManager.Instance.ChangePitch(1);
        //UnloadInterEvent();
		Invoke(nameof(LoadInterstitial), 1f);
		DelayShowOpenAd();

		//Controller.Instance.ShowSubPanel();

		//IngameController.Instance.ShowRemoveAd();
	}

	/************* Interstitial DemandOnly Delegates *************/

	void InterstitialAdReadyDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got InterstitialAdReadyDemandOnlyEvent for instance: " + instanceId);
	}

	void InterstitialAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
	{
		Debug.Log("unity-script: I got InterstitialAdLoadFailedDemandOnlyEvent for instance: " + instanceId + ", error code: " + error.getCode() + ",error description : " + error.getDescription());
	}

	void InterstitialAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
	{
		Time.timeScale = 1;
		Debug.Log("unity-script: I got InterstitialAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", error code :  " + error.getCode() + ",error description : " + error.getDescription());
        if (error.getCode() == 509)
        {
			Invoke(nameof(LoadInterstitial), 1);
        }
    }

	void InterstitialAdClickedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got InterstitialAdClickedDemandOnlyEvent for instance: " + instanceId);
	}

	void InterstitialAdOpenedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got InterstitialAdOpenedDemandOnlyEvent for instance: " + instanceId);
	}

	void InterstitialAdClosedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got InterstitialAdClosedDemandOnlyEvent for instance: " + instanceId);
	}


	/************* RewardedVideo API *************/
	public void ShowRewardedVideoButtonClicked()
	{
		Debug.Log("unity-script: ShowRewardedVideoButtonClicked");
		if (IronSource.Agent.isRewardedVideoAvailable())
		{
			isAD = true;
			IronSource.Agent.showRewardedVideo();

			FireEventAdPrepare("Reward");
		}
		else
		{
			Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False");
			FireEventAdNoFill("Reward");
		}

		// DemandOnly
		// ShowDemandOnlyRewardedVideo ();
		//if(PrefInfo.ShowOpenAdinAd)
		//	DoShowOpenAd();
	}

	void LoadDemandOnlyRewardedVideo()
	{
		Debug.Log("unity-script: LoadDemandOnlyRewardedVideoButtonClicked");
		IronSource.Agent.loadISDemandOnlyRewardedVideo(REWARDED_INSTANCE_ID);
	}

	void ShowDemandOnlyRewardedVideo()
	{
		Debug.Log("unity-script: ShowDemandOnlyRewardedVideoButtonClicked");
		if (IronSource.Agent.isISDemandOnlyRewardedVideoAvailable(REWARDED_INSTANCE_ID))
		{
			IronSource.Agent.showISDemandOnlyRewardedVideo(REWARDED_INSTANCE_ID);
		}
		else
		{
			Debug.Log("unity-script: IronSource.Agent.isISDemandOnlyRewardedVideoAvailable - False");
		}
	}

	/************* RewardedVideo Delegates *************/
	void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
	{
		Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
		masterAdsController.currentLoadedRewardCtr = this;
	}

	void RewardedVideoAdOpenedEvent()
	{
		Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
		Time.timeScale = 0;
        //SoundManager.Instance.SetSoundTypeOnOff(SoundManager.SoundType.Music, false);
        //SoundManager.Instance.ChangePitch(0);
    }

	void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
	{
		Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount() + " name = " + ssp.getRewardName());
		OnRewarded();
	}

	void RewardedVideoAdClosedEvent()
	{
		Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
		isTrackingEndReward = true;
		Time.timeScale = 1;
		isAD = false;
		DelayShowOpenAd();
		//SoundManager.Instance.SetSoundTypeOnOff(SoundManager.SoundType.Music, true);
		//SoundManager.Instance.ChangePitch(1);
		//UnloadRewardEvent();
		Invoke("LoadRewardedVideo", 1);
	}

	void RewardedVideoAdStartedEvent()
	{
		Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
	}

	void RewardedVideoAdEndedEvent()
	{
		Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
	}

	void RewardedVideoAdShowFailedEvent(IronSourceError error)
	{
		isAD = false;
		isTrackingEndReward = true;

		Time.timeScale = 1;
		Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
	}

	void RewardedVideoAdClickedEvent(IronSourcePlacement ssp)
	{
		Debug.Log("unity-script: I got RewardedVideoAdClickedEvent, name = " + ssp.getRewardName());
		//if (PrefInfo.ShowOpenAdinAd)
		//	DoShowOpenAd();
	}

	private void RewardedVideoLoadedReadyEvent()
	{

	}

	private void RewardedVideoLoadedFailedEvent(IronSourceError error)
	{
		if (error.getDescription().ToLower().Contains("no fill"))
		{

		}
	}

	/************* RewardedVideo DemandOnly Delegates *************/

	void RewardedVideoAdLoadedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got RewardedVideoAdLoadedDemandOnlyEvent for instance: " + instanceId);
	}

	void RewardedVideoAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
	{
		Debug.Log("unity-script: I got RewardedVideoAdLoadFailedDemandOnlyEvent for instance: " + instanceId + ", code :  " + error.getCode() + ", description : " + error.getDescription());

		if (error.getDescription().ToLower().Contains("no fill"))
		{

		}
	}

	void RewardedVideoAdOpenedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got RewardedVideoAdOpenedDemandOnlyEvent for instance: " + instanceId);
	}

	void RewardedVideoAdRewardedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got RewardedVideoAdRewardedDemandOnlyEvent for instance: " + instanceId);
	}

	void RewardedVideoAdClosedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got RewardedVideoAdClosedDemandOnlyEvent for instance: " + instanceId);
		PlayerPrefs.SetString("TimeToShowAds", System.DateTime.Now.ToString());
	}

	void RewardedVideoAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
	{
		Time.timeScale = 1;
		Debug.Log("unity-script: I got RewardedVideoAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", code :  " + error.getCode() + ", description : " + error.getDescription());
	}

	void RewardedVideoAdClickedDemandOnlyEvent(string instanceId)
	{
		Debug.Log("unity-script: I got RewardedVideoAdClickedDemandOnlyEvent for instance: " + instanceId);
	}

	public void LoadBanner()
	{
		if (!HasInternet())
		{
            Debug.Log("no internet ironsource LOAD BANNER");
			return;
		}
		if (!PrefInfo.IsUsingAd()) return;
		IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
	}

	public void ReloadBanner()
	{
		if (!HasInternet())
		{
			//masterAdsController.currentLoadedBannerCtr = null;
			Debug.Log("no internet ironsource LOAD BANNER");
			return;
		}
		if (!PrefInfo.IsUsingAd()) return;
		Debug.Log("reload ironsource banner");
		//UnLoadBannerEvent();
		//LoadBannerEvent();
		IronSource.Agent.destroyBanner();
		IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
		
	}

	public bool ShowBanner()
	{
		Debug.Log("	TRYING SHOW BANNER ");
		if (!HasInternet())
		{
            //masterAdsController.currentLoadedBannerCtr = null;
            Debug.Log("no internet ironsource BANNER");
			masterAdsController.failedBanner = true;
			return false;
		}

		if (PrefInfo.IsUsingAd())
		{
			Debug.Log("SHOW BANNER ");

			MasterControl.Instance.ShowBGBanner();
			IronSource.Agent.displayBanner();

			return true;
		}

		FireEventAdNoFill("Banner");
		return false;
	}

	public void HideBanner()
	{
		IronSource.Agent.hideBanner();

		MasterControl.Instance.HideBGBanner();
	}

	public void LoadInterstitial()
	{

		if (!PrefInfo.IsUsingAd()) return;
		if (!HasInternet())
		{
			Debug.Log("no internet ironsource");
			return;
		}
		Debug.Log("LOAD LOAD INTERSITITIAL");
		
		LoadInterstitialButtonClicked();
	}

    public bool ShowInterstitial()
    {
        if (!PrefInfo.IsUsingAd())
		{
			masterAdsController.InterstitialCallback(false);

            return false;
        }

		if (IronSource.Agent.isInterstitialReady())
        {
			ShowInterstitialButtonClicked();
        }
        else
        {
			FireEventAdNoFill("Inter");
			masterAdsController.InterstitialCallback(false);
            LoadInterstitial();
        }
        return false;
    }
	private void DelayShowInterstitalAd()
	{

		//ShowInterstitialButtonClicked();
	}
	public void LoadInterstitialEvents()
	{
		//LoadInterEvent();
	}

	public bool IsInterstitialReady()
	{
		return IronSource.Agent.isInterstitialReady();
	}

	public bool IsRewardLoaded()
	{
		return IronSource.Agent.isRewardedVideoAvailable();
	}

	public void LoadRewardedVideo()
	{
		if (!HasInternet())
		{
			Debug.Log("no internet ironsource");
			return;
		}
		Debug.Log("ADMOB LOAD REWARDED VIDEO");
		//LoadRewardEvent();
	}

	public bool ShowRewardedVideo()
	{
		if (!HasInternet())
		{
			Debug.Log("no internet ironsource");
			return false;
		}

		Debug.Log("ADMOB SHOW REWARD:" + IronSource.Agent.isRewardedVideoAvailable());
		if (IronSource.Agent.isRewardedVideoAvailable())
		{
			ShowRewardedVideoButtonClicked();
			return true;
		}

		MasterControl.Instance.CheckInternet(res =>
		{
			if (!masterAdsController.masterControl.isConnected)
			{
				Debug.Log("no internet");

			}
		}, false);
		masterAdsController.OnFail();
		FireEventAdNoFill("Reward");
		return false;
	}

	bool isOnReward = false;
	public void OnRewarded()
	{
		Debug.Log("ON REWARDed");
		isOnReward = true;
	}

	public void SetNext(IAdsController ctr)
	{
		next = ctr;
	}


	public static bool HasInternet()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			Debug.Log("Error. Check internet connection!");
			return false;
		}
		return true;
	}

    #region OpenAds
    [GUIColor(1, 0, 0)]
    public string openAdsID;
	bool isAD;

    public bool canShowOpenAds = true;
    public bool isStart = true;

    int quitTime = 0;
    //int tabOutTime = 0;
    private void LoadQuitTime()
    {
        quitTime = PlayerPrefs.GetInt("QuitTime", 0) + 1;
        PlayerPrefs.SetInt("QuitTime", quitTime);
    }
	//bool isDelaying;
	private void DelayShowOpenAd()
    {
		return;
		//if (isDelaying) return;
		//if (PrefInfo.ShowOpenAdinAd)
		//{
		//	isDelaying = true;

		//	//Dung unitask
		//	DoDelayShowOpenAd();

		//	//Khong dung unitask
		//	//var time = PrefInfo.GetTimeShowOpenAd;
		//	//Invoke(nameof(DoDelayShowOpenAd), time);
		//}
		//else isDelaying = true;
	}

	async void DoDelayShowOpenAd()
    {
		//Remove 2 dong nay neu khong dung unitask (Remove ca async)
		//var time = PrefInfo.GetTimeShowOpenAd;
		//await Cysharp.Threading.Tasks.UniTask.Delay((int)(time * 100));

		////Giu nguyen
		//DoShowOpenAd();
		//isDelaying = false;
    }

    public void ChangeValueShow()
    {
        canShowOpenAds = true;
    }
    public void DisableValueShow()
    {
        canShowOpenAds = false;
    }

    //private void OnApplicationPause(bool pause)
    //{
    //    if (!pause)
    //    {

    //        //ShowOpenAdIfReady();
    //    }
    //}

    private void LateUpdate()
    {
		if (GameManger.instance == null) return; 
        if (isStart)
        {
            if (ad == null) return;
            isStart = false;
            if (!masterAdsController.isAllowFirstOpen(quitTime)) return;
            ShowOpenAppAds();
        }
    }

    private AppOpenAd ad;
    private bool isShowingAd = false;
    //public bool canShowOpenAds = true;
    //    [BoxGroup("MobileAds-OpendAds")]
    //    public string openAdsID;
    //[BoxGroup("MobileAds-OpendAds")]
    private string openAdsAndroidTestID = "ca-app-pub-3940256099942544/3419835294";
	//[BoxGroup("MobileAds-OpendAds")]
	private string openAdsIOSTestID = "ca-app-pub-3940256099942544/5662855259";
    private void OnAppStateChanged(AppState state)
    {
        // Display the app open ad when the app is foregrounded.
        Debug.Log("App State is " + state);
        if (state == AppState.Foreground)
        {
            if (isStart) return;
			//tabOutTime++;
			DoShowOpenAd();
        }
    }
	private void DoShowOpenAd()
    {
		if (!masterAdsController.isAllowTabOpen(quitTime))
		{
			Debug.Log("Not allow Open Ad Tab");
			return;
		}
		ShowOpenAppAds();
	}
	private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }
    bool failedLoadOpenAd;
	[SerializeField]
    private bool testOpenAds;
    public void LoadOpenAppAds()
    {
        var id = openAdsID;
        if (testOpenAds)
        {
#if PLATFORM_ANDROID
            Debug.LogWarning("Is using test open Ads Android");
            id = openAdsAndroidTestID;
#endif
#if PLATFORM_IOS
                Debug.LogWarning("Is using test open Ads IOS");
                id = openAdsIOSTestID;
#endif
        }
        //Load an app open ad for portrait orientation

        AppOpenAd.Load(id, ScreenOrientation.Portrait, CreateAdRequest(), ((appOpenAd, error) =>
        {
            if (error != null)
            {
                //Handle the error.
                failedLoadOpenAd = true;
                if (isStart) isStart = false;
                Debug.LogFormat("Failed to load the ad. (reason: {0})", error.GetMessage());

				return;
            }

			//App open ad is loaded.

            ad = appOpenAd;
            loadTime = DateTime.UtcNow;
        }));


    }



    private DateTime loadTime;
    private bool IsAdAvailable
    {
        get
        {
            return PrefInfo.IsUsingAd() && canShowOpenAds == true && ad != null && (System.DateTime.UtcNow - loadTime).TotalHours < 4;
        }
    }
    public bool ShowOpenAppAds()
    {
		if (isAD) return false;
        Debug.Log("Trying to open App");
        if (MasterControl.Instance.adsState == AdsState.UnlockAll)
        {
            return false;
        }
        //if (!MasterControl.Instance.adsController.is) return false;
        if (PrefInfo.IsUsingAd())
        {
            if (!IsAdAvailable || isShowingAd)
            {
                if (ad == null)
                {
                    Debug.Log("Reload Openads");
                    LoadOpenAppAds();
					FireEventAdNoFill("OpenAd");
				}
                return false;
            }

            LoadOpenAppAdsEvents();
			 
            ad.Show();
            PlayerPrefs.SetString("TimeToShowOpenAds", DateTime.Now.ToString());

			Debug.Log("Show app open ads!!!");
			FireEventAdPrepare("OpenAd");

		}
        return true;
    }


    public void LoadOpenAppAdsEvents()
    {
        if (ad != null)
        {
            ad.OnAdFullScreenContentClosed += HandleAdFullScreenContentClosed;
            ad.OnAdFullScreenContentFailed += HandleFullScreenContentFailed;
            ad.OnAdFullScreenContentOpened += HandleAdFullScreenContentOpened;
            ad.OnAdImpressionRecorded += HandleAdImpressionRecorded;
			ad.OnAdPaid += HandleAdPaid;
			ad.OnAdClicked += HandleAdClicked;
		}
    }

    private void HandleAdClicked()
    {
		MobileAdsEventExecutor.ExecuteInUpdate(() =>
		{
			Debug.Log("Recorded open ad clicked");
		});
	}
	private void HandlePaidEvent_OpenAd(object sender, AdValueEventArgs args)
	{

	}
	private void HandleAdPaid(AdValue obj)
    {
		MobileAdsEventExecutor.ExecuteInUpdate(() =>
		{
            Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
					obj.CurrencyCode, obj.Value);
			adValueAdmobOpen = obj;
			checkAdmobOpen = true;
		});
	}

    private void HandleAdImpressionRecorded()
    {
		MobileAdsEventExecutor.ExecuteInUpdate(() =>
		{
			Debug.Log("Recorded ad impression");
		});
	}

    private void HandleAdFullScreenContentOpened()
    {
		MobileAdsEventExecutor.ExecuteInUpdate(() =>
		{
			Debug.Log("Displayed app open ad");
			isShowingAd = true;
			PlayerPrefs.SetString("TimeToShowOpenAds", System.DateTime.Now.ToString());
			FireBaseManager.Instance.LogEvent("OpenAd");
		});
	}

    private void HandleFullScreenContentFailed(AdError error)
    { 
		MobileAdsEventExecutor.ExecuteInUpdate(() =>
		{
			Debug.LogFormat("Failed to present the ad (reason: {0})", error.GetMessage());
			//Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
			isStart = false;
			ad = null;
			LoadOpenAppAds();
		});
	}

    private void HandleAdFullScreenContentClosed()
    {

		MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                    //Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
                ad = null; 
				isShowingAd = false;
                Debug.Log("Closed app open ad");
				PlayerPrefs.SetString("TimeToShowOpenAds", System.DateTime.Now.ToString());
				LoadOpenAppAds();
            });
    }

	#endregion

	#region NativeAds
	public bool testNativeAds;
	public string nativeAdID;
	private Stack<NativeAd> nativeAds = new Stack<NativeAd>();
	string NativeKey => testNativeAds ? "ca-app-pub-3940256099942544/2247696110" : nativeAdID;
	AdLoader adsLoader;
	public void LoadNativeAd()
	{
		adsLoader = new AdLoader.Builder(NativeKey)
			.ForNativeAd().SetNumberOfAdsToLoad(1)
			.Build();
		adsLoader.OnNativeAdLoaded += HandleNativeAdLoaded;
		adsLoader.OnNativeAdClicked += HandleNativeAdClicked;
		adsLoader.OnAdFailedToLoad += HandleNativeAdFailedToLoad;
		adsLoader.OnNativeAdImpression += HandleNativeAdImpression;
		adsLoader.OnNativeAdOpening += HandleNativeOpening;
		
		adsLoader.LoadAd(CreateAdRequest());
	}

    private void HandleNativeAdLoaded(object sender, NativeAdEventArgs e)
    {
		MobileAdsEventExecutor.ExecuteInUpdate(() =>
		{
			Debug.Log("HandleNativeAdLoaded event received");

			NativeAd nativeAd = e.nativeAd;
			nativeAd.OnPaidEvent += NativeAdOnPaidEvent;

			nativeAds.Push(nativeAd);
			Debug.Log("Native ad loaded " + e.nativeAd.GetHeadlineText() + " " + nativeAd.GetResponseInfo().GetResponseId() + " count:" + nativeAds.Count);

		});
	}

    private void NativeAdOnPaidEvent(object sender, AdValueEventArgs e)
    {
		MobileAdsEventExecutor.ExecuteInUpdate(() =>
		{
			Debug.Log("ADMOB natve ONPAID EVENT");
			adValueAdmobNative = e.AdValue;
			checkAdmobNative = true;
		});


		//FirebaseAnalysticController.Instance.SendRevenueToAdjust(e.AdValue);
	}

    private void HandleNativeOpening(object sender, EventArgs e)
    {
		MobileAdsEventExecutor.ExecuteInUpdate(() =>
		{
			Debug.Log("HandleNativeAdOpening event received");
		});
	}



    private void HandleNativeAdImpression(object sender, EventArgs e)
	{

		MobileAdsEventExecutor.ExecuteInUpdate(() =>
		{
			Debug.Log("HandleNativeAdImpression event received");
		});
	}
	private void HandleNativeAdClicked(object sender, EventArgs e)
	{
		MobileAdsEventExecutor.ExecuteInUpdate(() =>
		{
			Debug.Log("HandleNativeAdClicked event received");
		});

		//throw new NotImplementedException();
	}


	private void HandleNativeAdPaidEvent(object sender, AdValueEventArgs e)
	{
		MobileAdsEventExecutor.ExecuteInUpdate(() =>
		{
			Debug.LogWarning("HandleNativeAdPaidEvent event received");

			//FireBaseManager.Instance.OnPaidEvent(e.AdValue, "native");
			//FireBaseManager.Instance.LogAdRevenueOpenNative("native", e.AdValue.Value / 1000000f, e.AdValue.CurrencyCode);
			checkAdmobNative = true;

			//try
			//{
			//	AdjustAdRevenue adjustRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
			//	adjustRevenue.setRevenue(e.AdValue.Value / 1000000f, e.AdValue.CurrencyCode);
			//	Adjust.trackAdRevenue(adjustRevenue);
			//}
			//catch { }
		});

	}

	private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		MobileAdsEventExecutor.ExecuteInUpdate(() =>
		{
			if (args.LoadAdError.GetMessage().ToLower().Contains("no fill"))
			{

			}

			Debug.Log("HandleNativeAdFailedToLoad event received with message: " + args.ToString());
		});
		
	}

    public object GetCurrentNativeAd()
    {
		Debug.Log("GET NATIVE AD: " + nativeAds.Count);
		if (nativeAds.Count > 0)
		{
			Debug.Log("+native>>>>> " + nativeAds.Peek().GetHeadlineText());
			return nativeAds.Pop();
		}
		else
		{
			FireEventAdNoFill("NativeAd");
			return null;
		}
	}

    public bool IsNativeAvailable()
    {
		return nativeAds.Count > 0;
	}

    //public void ShowNativeAd()
    //{

    //}

    //public void HideNativeAd()
    //{

    //}


    //}
    //private void RequestNativeAd()
    //{
    //	AdLoader adLoader = new AdLoader.Builder("")
    //		.ForNativeAd()
    //		.Build();
    //	adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
    //	adLoader.OnAdFailedToLoad += this.HandleAdFailedToLoad;
    //	adLoader.LoadAd(new AdRequest.Builder().Build());
    //}
    #endregion


    #region event
	private void FireEventAdPrepare(string adType)
    {
		if (FireBaseManager.Instance == null) return;
		FireBaseManager.Instance.LogEvent("ADS_" + adType +"_Prepare");
		FireBaseManager.Instance.LogEvent("ADS_Prepare");
    }

	private void FireEventAdNoFill(string adType)
    {
		FireBaseManager.Instance.LogEvent("ADS_" + adType + "_NOFILL");
		FireBaseManager.Instance.LogEvent("ADS_NOFILL");
    }

    //public bool HasAdType(AdType ad)
    //{
    //    throw new NotImplementedException();
    //}
    #endregion
}
