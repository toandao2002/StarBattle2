using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterControlListenner : MonoBehaviour
{
    public void OnPurchased(UnityEngine.Purchasing.Product product)
    {
        MasterControl.Instance.OnPurchased(product);
    }
    public void OnFailedToPurchase(UnityEngine.Purchasing.Product product, UnityEngine.Purchasing.PurchaseFailureReason reason)
    {
        MasterControl.Instance.OnFailedToPurchase(product, reason);
    }
    
    public void OpenURL(string link)
    {
        MasterControl.Instance.OpenURL(link);
    }
    public static void Restore()
    {
        var m_StoreController = UnityEngine.Purchasing.CodelessIAPStoreListener.Instance.StoreController;
       /* if (m_StoreController != null)
        {
            Debug.Log("PRODUCT: " + MasterControl.Instance.productKeys[0] + " :" + m_StoreController.products.WithID(MasterControl.Instance.productKeys[0]).hasReceipt);
            bool check = false;
            if (m_StoreController.products.WithID(MasterControl.Instance.productKeys[0]).hasReceipt)
            {
                check = true;
                MasterControl.Instance.OnPurchased(MasterControl.Instance.productKeys[0]);
                //return;
            }
            if (!check)
            {
                MessagePanel.Instance.SetUp(Wugner.Localize.Localization.GetEntry(null, Loc.ID.Common.NothingRestore).Content, Wugner.Localize.Localization.GetEntry(null, Loc.ID.Common.MessageText).Content);
            }
        }*/
    }
   
}
