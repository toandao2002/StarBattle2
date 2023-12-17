using System;
using System.Collections;
using System.Collections.Generic;
using com.adjust.sdk;
using UnityEngine;
using UnityEngine.Serialization;
using GoogleMobileAds.Api;
using TMPro;
using Unity.Collections;
using UnityEngine.UI;
using GoogleMobileAds.Common;
using Cysharp.Threading.Tasks;
using System.Threading;

public class NativeAdsHandler : MonoBehaviour
{
    //[SerializeField]
    //private Image visibleUI;

    NativeAd _nativeAd;
    [SerializeField] private Text headline;
    [SerializeField] private Text body, advertiser, callToAction;
    [SerializeField] private Image icon, adChoices, imageAd;
    [SerializeField] private GameObject adsLoaded;
    [SerializeField] private GameObject loadingAds;

    public bool nativeLoaded = false;
    CancellationTokenSource cancellationToken;
    private List<Sprite> adBannerSprites = new List<Sprite>();



    private void OnEnable()
    {
        nativeLoaded = false;
        adsLoaded.SetActive(false);
        loadingAds.SetActive(true);


        #region Remote NativeAD
        if (PrefInfo.isUsingNativeAds)
        {
            //Call a request to show native Ad
            RequestNativeAds();
        }
        #endregion
    }

    //IEnumerator WaitUntilShowInScreen()
    //{
    //    yield return new WaitUntil(() => isVisible());
    //    if (PrefInfo.isUsingNativeAds)
    //    {
    //        RequestNativeAds();
    //    }
    //}

    //private void CaculatePoint()
    //{
    //    var p = MasterControl.Instance.points;
    //    minX = p[0];
    //    minY = p[1];
    //    maxX = p[2];
    //    maxY = p[3];
    //}
    //Vector3[] points = new Vector3[4];
    //float minX, minY, maxX, maxY;
    //private bool isVisible()
    //{
    //    visibleUI.rectTransform.GetWorldCorners(points);
    //    var minX = points[0].x;
    //    var minY = points[0].y;   
    //    var maxX = points[2].x;
    //    var maxY = points[2].y;
    //    if (this.minX >= minX && this.maxX <= maxX && this.minY >= minY && this.maxY <= maxY)
    //        return true;
    //    return false;
    //}


    private void OnDisable()
    {

   adsLoaded.SetActive(false);
        CancelInvoke();
        if (cancellationToken != null)
        {
            cancellationToken.Cancel();
        }
        MasterControl.Instance.adsController.onNativeAdRefresh -= OnRefresh;

    }
    private void OnDestroy()
    {
        if (cancellationToken != null)
        {
            cancellationToken.Cancel();
            cancellationToken.Dispose();
        }
        MasterControl.Instance.adsController.onNativeAdRefresh -= OnRefresh;
    }
    void OnRefresh()
    {
        LoadAd();
    }

    //private void Start()
    //{

    //   // nativeAdsUnitID = "ca-app-pub-3940256099942544/2247696110";


    //    if (PlayerPrefs.GetInt("Native_Ads") == 1)
    //        RequestNativeAds();
    //}




    private void RequestNativeAds()
    {
        adsLoaded.SetActive(false);
        loadingAds.SetActive(true);

        cancellationToken = new CancellationTokenSource();
        LoadAd();

        MasterControl.Instance.adsController.onNativeAdRefresh += OnRefresh;
    }
    async UniTaskVoid LoadAd()
    {

        #region Remote NativeAD
        if (!PrefInfo.isUsingNativeAds)
        {
            adsLoaded.SetActive(false);
            loadingAds.SetActive(true);
            return;
        }
        if (!PrefInfo.IsUsingAd())
        {
            adsLoaded.SetActive(false);
            loadingAds.SetActive(true);
            return;
        }
        #endregion
        float timeOut = 10;
        float time = Time.time;
#if UNITY_EDITOR
        await UniTask.Delay(500, ignoreTimeScale: true, cancellationToken: cancellationToken.Token);
        return;
#endif
        await UniTask.WaitUntil(() => MasterControl.Instance.IsNativeAdAvailable() || Time.time - time > timeOut, cancellationToken: cancellationToken.Token);

        if (_nativeAd != null)
        {
            _nativeAd.Destroy();
        }

        _nativeAd = (GoogleMobileAds.Api.NativeAd)MasterControl.Instance.GetNativeAd();
        MasterControl.Instance.adsController.LoadNativeAd();

        if (_nativeAd != null)
        {

            Texture2D texture2D = null;
            try
            {
                if (imageAd != null)
                {
                    List<Texture2D> texture2Ds = _nativeAd.GetImageTextures();
                    foreach (Texture2D texture in texture2Ds)
                    {
                        adBannerSprites.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero));
                    }
                    index = 0;
                    ChangeImage();

                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("1 " + e);
            }
            try
            {
                texture2D = _nativeAd.GetIconTexture();
                icon.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);

            }
            catch (System.Exception e)
            {
                Debug.LogError("1 " + e);
            }
            try
            {
                texture2D = _nativeAd.GetAdChoicesLogoTexture();
                adChoices.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
            }
            catch (System.Exception e)
            {
                Debug.LogError("2 " + e);
            }
            try
            {
                headline.text = _nativeAd.GetHeadlineText();
            }
            catch (System.Exception e)
            {
                Debug.LogError("3 " + e);
            }
            try
            {
                body.text = _nativeAd.GetBodyText();
            }
            catch (System.Exception e)
            {
                Debug.LogError("6 " + e);
            }

            //try
            //{
            //    ratingImage.fillAmount = (float)(nativeAd.GetStarRating() / 5f);
            //}
            //catch (System.Exception e)
            //{
            //    Debug.LogError("8 " + e);
            //}
            try
            {
                callToAction.text = _nativeAd.GetCallToActionText();
            }
            catch (System.Exception e)
            {
                Debug.LogError("9 " + e);
            }

            try
            {

                _nativeAd.RegisterCallToActionGameObject(gameObject);
                 //FireEventAdPrepare("NativeAd");

                FireBaseManager.Instance.LogEvent("ADS_NativeAd_Prepare");
                FireBaseManager.Instance.LogEvent("ADS_Prepare");

            }
            catch (System.Exception e)
            {
                Debug.LogError("10 " + e);
            }

            await UniTask.Yield();

            await UniTask.Delay(500, ignoreTimeScale: true, cancellationToken: cancellationToken.Token);

            adsLoaded.SetActive(true);
            loadingAds.SetActive(false);
        }
        else
        {
            adsLoaded.SetActive(false);
            loadingAds.SetActive(true);
        }
    }
    int index = 0;
    void ChangeImage()
    {
        imageAd.sprite = adBannerSprites[index % adBannerSprites.Count];
        index++;
        Invoke(nameof(ChangeImage), 4);
    }
    //IEnumerator DelayedRequest()
    //{
    //    Debug.LogWarning("Native Loaded");
    //    yield return null;
    //    nativeLoaded = false;
    //    adsLoaded.SetActive(true);
    //    loadingAds.SetActive(false);
    //    yield return new WaitForSeconds(1f);

    //    //Get icon
    //    if (icon != null)
    //    {
    //        icon.texture = _nativeAd.GetIconTexture();
    //        yield return null;
    //    }

    //    if (adChoices != null)
    //    {
    //        adChoices.texture = _nativeAd.GetAdChoicesLogoTexture();
    //        yield return null;
    //        adChoices.gameObject.SetActive(adChoices.texture != null);
    //    }

    //    if (headline != null)
    //    {
    //        headline.text = _nativeAd.GetHeadlineText();
    //        yield return null;
    //    }

    //    if (body != null)
    //    {
    //        body.text = _nativeAd.GetBodyText(); 
    //        yield return null;
    //    }

    //    if (callToAction != null)
    //    {
    //        callToAction.text = _nativeAd.GetCallToActionText();
    //        yield return null;
    //    }
          
    //    if (imageAd != null)
    //    {
    //        imageAd.texture = _nativeAd.GetImageTextures()[0];
    //        //imageAd.SetNativeSize();
    //        yield return null;
    //    }

    //    if (advertiser != null)
    //    {
    //        advertiser.text = _nativeAd.GetAdvertiserText();
    //        yield return null;
    //    }


    //    if (icon != null)
    //    {
    //        _nativeAd.RegisterIconImageGameObject(icon.gameObject);
    //        yield return null;
    //    }

    //    if (adChoices != null)
    //    {
    //        _nativeAd.RegisterAdChoicesLogoGameObject(adChoices.gameObject);
    //        yield return null;
    //    }

    //    if (headline != null)
    //    {
    //        _nativeAd.RegisterHeadlineTextGameObject(headline.gameObject);
    //        yield return null;
    //    }

    //    if (body != null)
    //    {
    //        _nativeAd.RegisterBodyTextGameObject(body.gameObject);
    //        yield return null;
    //    }

    //    if (callToAction != null) 
    //    {
    //        _nativeAd.RegisterCallToActionGameObject(callToAction.gameObject);
    //        yield return null;
    //    }

    //    if (advertiser != null)
    //    {
    //        _nativeAd.RegisterAdvertiserTextGameObject(advertiser.gameObject);
    //        yield return null;
    //    }

    //    if (imageAd != null)
    //    {
    //        _nativeAd.RegisterImageGameObjects(new List<GameObject>() { imageAd.gameObject });
    //        yield return null;
    //    }



    //    /*Debug.LogWarning("Native Body " + _nativeAd.GetBodyText());
    //    Debug.LogWarning("Native Headline " + _nativeAd.GetHeadlineText());
    //    Debug.LogWarning("Native CallToAction " + _nativeAd.GetCallToActionText());
    //    Debug.LogWarning("Native Icon " + _nativeAd.GetIconTexture());
    //    Debug.LogWarning("Native AdChoicesLogo " + _nativeAd.GetAdChoicesLogoTexture());
    //    Debug.LogWarning("Native Advertiser " + _nativeAd.GetAdvertiserText());
    //    Debug.LogWarning("Native Store " + _nativeAd.GetStore());
    //    Debug.LogWarning("Native Price " + _nativeAd.GetPrice());
    //    Debug.LogWarning("Native StarRating " + _nativeAd.GetStarRating());*/

    //    adsLoaded.SetActive(true);
    //    loadingAds.SetActive(false);
    //}


}