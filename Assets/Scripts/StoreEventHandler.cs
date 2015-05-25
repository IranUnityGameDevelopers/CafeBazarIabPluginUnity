using UnityEngine;
using System.Collections;
using UnityEngine.UI;


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

	public void ProcessPurchase (string sku)
	{
		// check for Consumables and Consume them and increase ItemsConsuming
		
		Debug.Log("process purchase called for sku : " + sku);
		/////// Trivial Drive Code
		
		if (sku == "gas") {
			StoreHandler.Instance.Consume(sku);
		}
		else if (sku == "premium") {
			_Image.sprite = PremiumImage;
			GameHandler.Instance.isPremium = true;
			BuyPremiumButton.SetActive(false);
		}	
		else if (sku == "infinite_gas") {
			gasSprite.sprite = gasInfinite;
			GameHandler.Instance.isInfiniteGas = true;
			BuyInfiniteGas.SetActive(false);
		}
		///  end of Trivial Drive Code
	}

	public void OnConsumeFinished (string sku)
	{
		Debug.Log("consume finished called for sku : " + sku);
		
		Overlay.Instance.ShowOverlay("Consumed : " + sku);
		/////// Trivial Drive Code
		if (sku == "gas") {
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
	public void OnMissingToken (string message, StoreErrorCodes errorCode, string sku = "")
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + sku);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + sku);
	}
	public void OnSubscriptionNotAvilable (string message, StoreErrorCodes errorCode, string sku = "")
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + sku);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + sku);
	}
	public void OnFailedToConsumePurchase (string message, StoreErrorCodes errorCode, string sku = "")
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + sku);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + sku);
	}
	public void OnConsumeFinishedListenerError (string message, StoreErrorCodes errorCode, string sku = "")
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + sku);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + sku);
	}
	public void OnPurchaseFailed (string message, StoreErrorCodes errorCode, string sku = "")
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + sku);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + sku);
	}
	public void OnPurchasePayloadVerificationFailed (string message, StoreErrorCodes errorCode, string sku = "")
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + sku);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + sku);
	}
	public void OnUserCancelled(string message, StoreErrorCodes errorCode , string sku = "")
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + sku);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + sku);
	}
	public void OnUnknownError (StoreErrorCodes errorCode , string message = "" , string sku = "")
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message + ", error code : " + errorCode + ", with sku : " + sku);
		Overlay.Instance.ShowOverlay(message + ", error code : " + errorCode + ", with sku : " + sku);
	}
	#endregion
}
