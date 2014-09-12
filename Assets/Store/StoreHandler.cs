using UnityEngine;
using System.Collections;

/////// Trivial Drive Code
using UnityEngine.UI;
///  end of Trivial Drive Code

public class StoreHandler : MonoBehaviour {

	/////// Trivial Drive Code
	public Image _Image;
	public Sprite PremiumImage;
	public Button BuyPremiumButton;
	public Button BuyInfiniteGas;
	///  end of Trivial Drive Code

	//&& !UNITY_EDITOR
#if UNITY_ANDROID 
	private string Base64EncodedPublicKey = "MIHNMA0GCSqGSIb3DQEBAQUAA4G7ADCBtwKBrwDgLxZsZomrmatxmihzk/bypxJXRiegmcmZyirjzlVFZ4vyxmtjjqPvJFwyZeyEW/vXHBlxaF7WaDk1SknUF+eDfh2hYDrQW+ctYve4eZ0oI78lns0yJKUVLh7K91d1ExJdDYo7W+s3pDy1OiGwOLmdTZXsf9bEazGcynXqQrwCK0BGMO40FL67VFdLgoMiaKajwlmMbwuJsy9pBsI2eq2AXTaga4u/yf1fEdGOfEsCAwEAAQ==";
	private string Payload = "Payload";
	private int ItemsConsuming = 0;
	private AndroidJavaObject StoreController;
	
	public void StartStore () {
		ActivityIndicator.Instance.Show();
		AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

		StoreController = new AndroidJavaObject("ir.unity3d.cafebazarplugin.StoreController" 
		                                        , Base64EncodedPublicKey 
		                                        , Payload 
		                                        , UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"));
		StoreController.Call("startSetup");
	}
	


	public void OnError(string message)
	{
		ActivityIndicator.Instance.Hide();
		Debug.LogError(message);
		ErrorOverlay.Instance.ShowOverlay(message);
	}

	/// <summary>
	/// Called from java code when startSetup() is called
	/// this function we be called for every purc
	/// </summary>
	/// <param name="SKU">SK.</param>
	public void ProcessPurchase(string sku)
	{
		// check for Consumables and Consume them and increase ItemsConsuming

		/////// Trivial Drive Code

		if (sku == "premium") {
			_Image.sprite = PremiumImage;
			BuyPremiumButton.enabled = false;
		}	
		else if (sku == "infinite_gas") {
			BuyInfiniteGas.enabled = false;
		}
		///  end of Trivial Drive Code

	}

	public void GetPurchasesFinished()
	{
		if (ItemsConsuming > 0) {
			// Do nothing
		}
		else if (ItemsConsuming == 0)
		{
			// Update UI
			ActivityIndicator.Instance.Hide();
			ErrorOverlay.Instance.ShowOverlay("GetPurchases Finished");
		}
	}

	public void ConsumeFinished(string sku)
	{
		ErrorOverlay.Instance.ShowOverlay("Consumed : " + sku);
		/////// Trivial Drive Code
		GameHandler.Instance.GasBought();
		///  end of Trivial Drive Code

		// consume object then do the following
		ItemsConsuming--;
		GetPurchasesFinished();
	}

	public void PurchaseFinished(string sku)
	{
		ActivityIndicator.Instance.Hide();
		// check if consumable do nothing
		// else do the job :D

		/////// Trivial Drive Code
		if (sku != "gas") {
			ErrorOverlay.Instance.ShowOverlay("Consumed : " + sku);
		}
		if (sku == "premium") {
			_Image.sprite = PremiumImage;
			BuyPremiumButton.enabled = false;
		}	
		else if (sku == "infinite_gas") {
			BuyInfiniteGas.enabled = false;
		}
		///  end of Trivial Drive Code

	}

	public void Purchase(ShopItem item)
	{
		ActivityIndicator.Instance.Show();
		Debug.Log("Purchase Called with SKU :" + item.SKU);
		if (item._Type == ShopItemType.inapp) {
			StoreController.Call("launchPurchaseFlow" , new AndroidJavaObject("java.lang.String" , item.SKU));
		}
		else if (item._Type == ShopItemType.subs) {
			StoreController.Call("launchSubscriptionPurchaseFlow" , new AndroidJavaObject("java.lang.String" , item.SKU));
		}

	}
#endif

}
