using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Purchasing;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager, 
// one of the existing Survival Shooter scripts.
//namespace CompleteProject
//{
// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.

public class Purchaser : MonoBehaviour, IStoreListener
{
    public static Purchaser Instance
    {
        get;
        set;
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }



    [ReadOnly]
    public MasterControl masterControl;

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    //private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    private static IAppleExtensions m_AppleExtensions;

    [ReadOnly]
    public bool isPurchasing = false;
    public bool didRestoreSuccess = false;
    public bool isInit => m_StoreController != null;

    List<string> prices = new List<string>();


    public void Init()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        
        if (IsInitialized())
        {
            return;
        }

        Debug.Log("SCRIPT INit PURCHASER");


        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        var keys = masterControl.productKeys;

        for (int i =0; i < keys.Length; i++)
        {
            builder.AddProduct(keys[i].key, keys[i].type);
        }

        UnityPurchasing.Initialize(CodelessIAPStoreListener.Instance, builder);
        StartCoroutine(DoWaitForInit());


    }
    IEnumerator DoWaitForInit()
    {
        yield return new WaitUntil(() => CodelessIAPStoreListener.initializationComplete);
        OnInitialized(CodelessIAPStoreListener.Instance.StoreController, null);
        //ConfirmingPurchase();
    }

    private bool IsInitialized()
    {
        return m_StoreController != null;
    }
#if UNITY_IOS || UNITY_EDITOR
    float timer = 0;
    bool isProcessing = false;
    float timer2 = 0;

    private bool checkRestore = false;
    void Update()
    {
        //if (!isProcessing && isPurchasing)
        //{
        //    isProcessing = true;
        //    Controller.Instance.pleaseWaitPanel.SetActive(true);
        //    MasterControl.Instance.CheckInternet(action =>
        //    {
        //        if (!action)
        //        {
        //            Controller.Instance.pleaseWaitPanel.SetActive(false);
        //            isProcessing = false;
        //            isPurchasing = false;
        //        }
        //    }, true);

        //}
        //else if (isProcessing && !isPurchasing)
        //{
        //    if (Controller.Instance.pleaseWaitPanel.activeInHierarchy)
        //    {
        //        Controller.Instance.pleaseWaitPanel.SetActive(false);
        //    }
        //    isProcessing = false;
        //}

        //if (didRestoreSuccess)
        //{
        //    didRestoreSuccess = false;
        //    MessagePanel.Instance.SetUp(Wugner.Localize.Localization.GetEntry(null, Loc.ID.Message.RestoreSuccess).Content, Wugner.Localize.Localization.GetEntry(null, Loc.ID.Common.MessageText).Content);
        //    Debug.Log("Restore Success");
        //    CheckRestore();
        //}

    }
#endif
    public void CheckRestore()
    {
        Debug.Log("CHECK RESTORE");
        if (m_StoreController != null)
        {
            Debug.Log("PRODUCT: " + MasterControl.Instance.productKeys[0].key + " :" + m_StoreController.products.WithID(MasterControl.Instance.productKeys[0].key).hasReceipt);
            bool check = false;
            if (m_StoreController.products.WithID(MasterControl.Instance.productKeys[0].key).hasReceipt)
            {
                check = true;
                MasterControl.Instance.OnRestore(MasterControl.Instance.productKeys[0].key);
            }

            if (m_StoreController.products.WithID(MasterControl.Instance.productKeys[1].key).hasReceipt)
            {
                check = true;
                MasterControl.Instance.OnRestore(MasterControl.Instance.productKeys[1].key);
            }

            if (m_StoreController.products.WithID(MasterControl.Instance.productKeys[2].key).hasReceipt)
            {
                check = true;
                MasterControl.Instance.OnRestore(MasterControl.Instance.productKeys[2].key);
            }


            if (!check)
            {
                /*PanelManager.GetPanel<MessagePanel>((panel) =>
                {
                    (panel).SetUp("Nothing to Restore!");
                });*/
                return;
                //MessagePanel.Instance.SetUp(Wugner.Localize.Localization.GetEntry(null, Loc.ID.Common.NothingRestore).Content, Wugner.Localize.Localization.GetEntry(null, Loc.ID.Common.MessageText).Content);
            }
            else
            {

              /*  PanelManager.GetPanel<MessagePanel>((panel) =>
                {
                    (panel).SetUp("Restore Successed!");
                });*/
                //MessagePanel.Instance.SetUp(Wugner.Localize.Localization.GetEntry(null, Loc.ID.Common.RestoreSuccess).Content, Wugner.Localize.Localization.GetEntry(null, Loc.ID.Common.MessageText).Content);
            }
        }
    }

    public bool HasReceipt(string id)
    {
        if (m_StoreController != null)
        {
            if (m_StoreController.products.WithID(id).hasReceipt)
            {
                return true;
            }
        }
        return false;
    }


    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer
            || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = CodelessIAPStoreListener.Instance.GetStoreExtensions<IAppleExtensions>();

            apple.RestoreTransactions((result) =>
            {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.LogError("OnInitialized: PASS");
        m_StoreController = controller;
        //m_StoreExtensionProvider = extensions;
        //m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
        m_AppleExtensions = CodelessIAPStoreListener.Instance.GetStoreExtensions<IAppleExtensions>();
        CheckSubscription();
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }


    public List<string> GetPrices()
    {
        foreach (var product in m_StoreController.products.all)
        {
            Debug.Log(product.metadata.localizedTitle + " " + product.metadata.localizedPriceString + " " + product.metadata.localizedPrice);
            prices.Add(product.metadata.localizedPriceString);
        }
        return prices;
    }
    public decimal GetPrice(int id)
    {
        try
        {
            return m_StoreController.products.all[id].metadata.localizedPrice;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            return 0;
        }
    }

    public string GetPriceWithCurreny(int id, bool isCurrencyFirst = false)
    {
        var price = IsThisInteger((float)GetPrice(id));

        if(!isCurrencyFirst)
            return price.ToString() +  GetCurrency(id);
        return GetCurrency(id) + " " + price.ToString();
    }

    private string IsThisInteger(float myFloat)
    {
        return myFloat.ToString(Mathf.Approximately(myFloat, Mathf.RoundToInt(myFloat)) ? "F0" : "F2");
    }
    public string GetCurrency(int id)
    {
        try
        {
            return m_StoreController.products.all[id].metadata.isoCurrencyCode;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            return "";
        }
    }

    public bool CheckSubscription()
    {
        Dictionary<string, string> introductory_info_dict = m_AppleExtensions.GetIntroductoryPriceDictionary();
        // Sample code for expose product sku details for apple store
        //Dictionary<string, string> product_details = m_AppleExtensions.GetProductDetails();


        Debug.Log("Available items:");
        foreach (var item in m_StoreController.products.all)
        {
            if (item.availableToPurchase)
            {
                Debug.Log(string.Join(" - ",
                    new[]
                    {
                        item.metadata.localizedTitle,
                        item.metadata.localizedDescription,
                        item.metadata.isoCurrencyCode,
                        item.metadata.localizedPrice.ToString(),
                        item.metadata.localizedPriceString,
                        item.transactionID,
                        item.receipt
                    }));
#if INTERCEPT_PROMOTIONAL_PURCHASES
                // Set all these products to be visible in the user's App Store according to Apple's Promotional IAP feature
                // https://developer.apple.com/library/content/documentation/NetworkingInternet/Conceptual/StoreKitGuide/PromotingIn-AppPurchases/PromotingIn-AppPurchases.html
                m_AppleExtensions.SetStorePromotionVisibility(item, AppleStorePromotionVisibility.Show);
#endif
                // this is the usage of SubscriptionManager class\
                if (item.receipt != null)
                {
                    if (item.definition.type == ProductType.Subscription)
                    {
                        Debug.Log("CHECK SUBSCRIPTION :" + item.metadata.localizedTitle + " :: " + checkIfProductIsAvailableForSubscriptionManager(item.receipt));
                        if (checkIfProductIsAvailableForSubscriptionManager(item.receipt))
                        {
                            string intro_json = (introductory_info_dict == null || !introductory_info_dict.ContainsKey(item.definition.storeSpecificId)) ? null : introductory_info_dict[item.definition.storeSpecificId];
                            SubscriptionManager p = new SubscriptionManager(item, intro_json);
                            SubscriptionInfo info = p.getSubscriptionInfo();
                            Debug.Log("product id is: " + info.getProductId());
                            Debug.Log("purchase date is: " + info.getPurchaseDate());
                            Debug.Log("subscription next billing date is: " + info.getExpireDate());
                            Debug.Log("is subscribed? " + info.isSubscribed().ToString());
                            Debug.Log("is expired? " + info.isExpired().ToString());
                            Debug.Log("is cancelled? " + info.isCancelled());
                            Debug.Log("product is in free trial peroid? " + info.isFreeTrial());
                            Debug.Log("product is auto renewing? " + info.isAutoRenewing());
                            Debug.Log("subscription remaining valid time until next billing date is: " + info.getRemainingTime());
                            Debug.Log("is this product in introductory price period? " + info.isIntroductoryPricePeriod());
                            Debug.Log("the product introductory localized price is: " + info.getIntroductoryPrice());
                            Debug.Log("the product introductory price period is: " + info.getIntroductoryPricePeriod());
                            Debug.Log("the number of product introductory price period cycles is: " + info.getIntroductoryPricePeriodCycles());

                            //neu nhu da het han
                            OnExpiredSub(item);
                        }
                        else
                        {
                            Debug.Log("This product is not available for SubscriptionManager class, only products that are purchase by 1.19+ SDK can use this class.");
                        }
                    }
                    else
                    {
                        Debug.Log("the product is not a subscription product");
                       
                    }
                }
                else
                {
                    Debug.Log("the product should have a valid receipt " + item.definition.id);
                   if(item.definition.type == ProductType.Subscription)
                    {
                        OnExpiredSub(item);
                    }
                }
            }
        }
        return false;
    }

 
    private void OnExpiredSub(Product product)
    {
        MasterControl.Instance.OnExpired(product.definition.id);
    }
    private bool checkIfProductIsAvailableForSubscriptionManager(string receipt)
    {
        var receipt_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
        if (!receipt_wrapper.ContainsKey("Store") || !receipt_wrapper.ContainsKey("Payload"))
        {
            Debug.Log("The product receipt does not contain enough information");
            return false;
        }
        var store = (string)receipt_wrapper["Store"];
        var payload = (string)receipt_wrapper["Payload"];

        if (payload != null)
        {
            switch (store)
            {
                case GooglePlay.Name:
                    {
                        var payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
                        if (!payload_wrapper.ContainsKey("json"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the 'json' field is missing");
                            return false;
                        }
                        var original_json_payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode((string)payload_wrapper["json"]);
                        if (original_json_payload_wrapper == null || !original_json_payload_wrapper.ContainsKey("developerPayload"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the 'developerPayload' field is missing");
                            return false;
                        }
                        var developerPayloadJSON = (string)original_json_payload_wrapper["developerPayload"];
                        var developerPayload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(developerPayloadJSON);
                        if (developerPayload_wrapper == null || !developerPayload_wrapper.ContainsKey("is_free_trial") || !developerPayload_wrapper.ContainsKey("has_introductory_price_trial"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the product is not purchased using 1.19 or later");
                            return false;
                        }
                        return true;
                    }
                case AppleAppStore.Name:
                case AmazonApps.Name:
                case MacAppStore.Name:
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
        return false;
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError("Purchaser init failed: <" + error + "> with mess " + message);
    }

    //private void ConfirmingPurchase()
    //{
    //    var m_Store = CodelessIAPStoreListener.Instance.StoreController;
    //    foreach(var p in m_Store.products.all)
    //    {
    //        m_Store.ConfirmPendingPurchase(p);
    //    }
    //}

}
//}
