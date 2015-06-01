using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CafeBazarIab;


public class StoreEventHandler : MonoBehaviour , IStoreEventHandler {

	/////// Trivial Drive Code
	public Image _Image;
	public Image gasSprite;
	public Sprite PremiumImage;
	public Sprite gasInfinite;
	public GameObject BuyPremiumButton;
	public GameObject BuyInfiniteGas;
	///  end of Trivial Drive Code

	#region IStoreEventHandler implementation

	public void OnSetupSuccessful()
	{
		StoreHandler.Instance.GetPurchases();
	}

	public void ProcessPurchase (ShopItem item)
	{
		// check for Consumables and Consume them and increase ItemsConsuming
		
		Debug.Log("process purchase called for sku : " + item.SKU);
		/////// Trivial Drive Code
		
		if (item.SKU == "gas") {
			StoreHandler.Instance.Consume(item);
		}
		else if (item.SKU == "premium") {
			_Image.sprite = PremiumImage;
			GameHandler.Instance.isPremium = true;
			BuyPremiumButton.SetActive(false);
		}	
		else if (item.SKU == "infinite_gas") {
			gasSprite.sprite = gasInfinite;
			GameHandler.Instance.isInfiniteGas = true;
			BuyInfiniteGas.SetActive(false);
		}
		///  end of Trivial Drive Code
	}

	public void OnConsumeFinished (ShopItem item)
	{
		Debug.Log("consume finished called for sku : " + item.SKU);
		
		Overlay.Instance.ShowOverlay("Consumed : " + item.SKU);
		/////// Trivial Drive Code
		if (item.SKU == "gas") {
			GameHandler.Instance.GasBought();
		}
		///  end of Trivial Drive Code
	}

	public void OnGetPurchasesFinished (string allRawSKU, int length)
	{
		Debug.Log("get purchases finished called for : " + allRawSKU + " ,with lengh :" + length);
		ActivityIndicator.Instance.Hide();
		Overlay.Instance.ShowOverlay("GetPurchases Finished");
	}

	public void OnProblemSettingUpIAB (string message, StoreErrorCodes errorCode)
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode);
		Overlay.Instance.ShowOverlay(message+ ", error code : " + errorCode);
	}
	public void OnFailedToQueryInventory (string message, StoreErrorCodes errorCode)
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode);
		Overlay.Instance.ShowOverlay(message+ ", error code : " + errorCode);
	}
	public void OnMissingToken (string message, StoreErrorCodes errorCode, ShopItem item = null)
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
	}
	public void OnSubscriptionNotAvilable (string message, StoreErrorCodes errorCode, ShopItem item = null)
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
	}
	public void OnFailedToConsumePurchase (string message, StoreErrorCodes errorCode, ShopItem item = null)
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
	}
	public void OnConsumeFinishedListenerError (string message, StoreErrorCodes errorCode, ShopItem item = null)
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
	}
	public void OnPurchaseFailed (string message, StoreErrorCodes errorCode, ShopItem item = null)
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
	}
	public void OnPurchasePayloadVerificationFailed (string message, StoreErrorCodes errorCode, ShopItem item = null)
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
	}
	public void OnUserCancelled(string message, StoreErrorCodes errorCode, ShopItem item = null)
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
	}
	public void OnUnknownError (StoreErrorCodes errorCode , string message = "" , ShopItem item = null)
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + item.SKU);
	}
	#endregion
}
