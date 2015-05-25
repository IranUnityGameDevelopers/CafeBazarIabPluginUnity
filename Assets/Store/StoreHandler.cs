using UnityEngine;
using System.Collections;

public class StoreHandler : MonoBehaviour {

	private static StoreHandler _instance;
	public static StoreHandler Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<StoreHandler>();
			}
			return _instance;
		}
	}

	public bool StoreStarted = false;
	public bool DebugMode;

	//&& !UNITY_EDITOR
#if UNITY_ANDROID 

	public const int BILLING_RESPONSE_RESULT_OK = 0;


	private string Base64EncodedPublicKey = "MIHNMA0GCSqGSIb3DQEBAQUAA4G7ADCBtwKBrwDqcvluFwhix7+hEI9m9ZWEyfSLX1BfvpIrnUzKGGjCHaF/vDnX0p6gr0a4PhgUC8ug2UyITDjaWhtfyRkBs01ZNWofz0Da85jduAnvPmI0mTvtMjhg94llHbYk+V9GpSaWvJpqCVQAT0V5caS8LKptFe7QrDEEcfF+KJtd33RxoyC7rVyPtw36E/h71TvCt2LvUajx9kWonmlih4p7LbGnkBemzeaUFNu8VO1dlvsCAwEAAQ==";
	private string Payload = "Payload";
	private AndroidJavaObject StoreController;
	private IStoreEventHandler EventHandler;
	
	public void StartStore (IStoreEventHandler _eventHandler) {
		if (_eventHandler == null) {
			return;
		}
		EventHandler = _eventHandler;
		string _debugmode;
		if (DebugMode == true) {
			_debugmode = "TRUE";
		}
		else{
			_debugmode = "";
		}


		AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");


		StoreController = new AndroidJavaObject("ir.unity3d.cafebazarplugin.StoreController");


		StoreController.Call("startSetup", Base64EncodedPublicKey 
					                     , Payload 
					                     , UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")
					                     , new AndroidJavaObject("java.lang.String" , _debugmode) );

	}

	public void SetupSuccessful()
	{
		StoreStarted = true;
		if (EventHandler != null) {
			EventHandler.OnSetupSuccessful();
		}
	}


	public void GetPurchases()
	{
		if (StoreStarted) {
			StoreController.Call("QueryInventory");
		}
	}

	public void Consume(string sku)
	{
		if (StoreStarted) {
			StoreController.Call("Consume" , new AndroidJavaObject("java.lang.String" , sku));
		}
	}

	public void OnError(string message)
	{
		if (EventHandler != null) {

			string[] msg = message.Split('@');

			StoreErrorCodes errorCode = StoreErrorCodes.DONOTHING;

			int result = 0;
			if (int.TryParse(msg[msg.Length - 1] ,out result)) {
				errorCode = (StoreErrorCodes) result;
			}
			


			string sku = "";

			if (message.Split('{', '}').Length > 1) {
				sku = message.Split('{', '}')[1] == null ? "" : message.Split('{', '}')[1];
			}




			if (errorCode == StoreErrorCodes.BILLING_RESPONSE_RESULT_BILLING_UNAVAILABLE 
			    || errorCode == StoreErrorCodes.IABHELPER_ERROR_BASE
			    || errorCode == StoreErrorCodes.IABHELPER_SEND_INTENT_FAILED
			    || errorCode == StoreErrorCodes.IABHELPER_REMOTE_EXCEPTION) {
				EventHandler.OnProblemSettingUpIAB(msg[0] , errorCode);
			}
			else if (errorCode == StoreErrorCodes.IABHELPER_INVALID_CONSUMPTION)
			{
				EventHandler.OnFailedToConsumePurchase(msg[0] , errorCode , sku);
			}
			else if (errorCode == StoreErrorCodes.IABHELPER_MISSING_TOKEN)
			{
				EventHandler.OnMissingToken(msg[0] , errorCode , sku);
			}
			else if (errorCode == StoreErrorCodes.IABHELPER_PURCHASE_PAYLOAD_VERIFICATION_FAILED
			         || errorCode == StoreErrorCodes.IABHELPER_VERIFICATION_FAILED) {
				EventHandler.OnPurchasePayloadVerificationFailed(msg[0] , errorCode , sku);
			}
			else if (errorCode == StoreErrorCodes.IABHELPER_SUBSCRIPTIONS_NOT_AVAILABLE) {
				EventHandler.OnSubscriptionNotAvilable(msg[0] , errorCode , sku);
			}
			else if (errorCode == StoreErrorCodes.IABHELPER_UNKNOWN_PURCHASE_RESPONSE) {
				EventHandler.OnPurchaseFailed(msg[0] , errorCode , sku);
			}
			else if (errorCode == StoreErrorCodes.IABHELPER_USER_CANCELLED) {
				EventHandler.OnUserCancelled(msg[0] , errorCode , sku);
			}
			else if (errorCode == StoreErrorCodes.IABHELPER_BAD_RESPONSE
			         || errorCode == StoreErrorCodes.IABHELPER_UNKNOWN_ERROR) {
				EventHandler.OnUnknownError(errorCode , msg[0] , sku);
			}
		}
	}
	
	public void GetPurchasesFinished(string allRawSKU)
	{
		if (EventHandler != null) {

			if (allRawSKU == "") {
				//purchase is null or Payloads dosnt match 
				EventHandler.OnGetPurchasesFinished("" , 0);
			}
			else
			{
				string[] allSKU = allRawSKU.Split(',');
				int count = 0;
				for (int i = 0; i < allSKU.Length; i++) {
					if (allSKU[i].Trim() != "") {
						EventHandler.ProcessPurchase(allSKU[i].Trim());
						count++;
					}
				}
				EventHandler.OnGetPurchasesFinished(allRawSKU , count);
			}
		}
	}

	public void ConsumeFinished(string sku)
	{
		if (EventHandler != null) {
			EventHandler.OnConsumeFinished(sku);
		}
	}

	public void Purchase(ShopItem item)
	{
		StartCoroutine(delay());
		if (StoreStarted) {	
			if (item._Type == ShopItemType.inapp) {
				StoreController.Call("launchPurchaseFlow" , new AndroidJavaObject("java.lang.String" , item.SKU));
			}
			else if (item._Type == ShopItemType.subs) {
				StoreController.Call("launchSubscriptionPurchaseFlow" , new AndroidJavaObject("java.lang.String" , item.SKU));
			}
		}

	}

	IEnumerator delay()
	{
		yield return new WaitForSeconds(1);
	}
#endif

}

/// <summary>
/// 	Billing response codes
/// </summary>
public enum StoreErrorCodes
{
	DONOTHING = 0,
	BILLING_RESPONSE_RESULT_BILLING_UNAVAILABLE = 3,
	IABHELPER_ERROR_BASE = -1000,
	IABHELPER_REMOTE_EXCEPTION = -1001,
	IABHELPER_BAD_RESPONSE = -1002,
	IABHELPER_VERIFICATION_FAILED = -1003,
	IABHELPER_SEND_INTENT_FAILED = -1004,
	IABHELPER_USER_CANCELLED = -1005,
	IABHELPER_UNKNOWN_PURCHASE_RESPONSE = -1006,
	IABHELPER_MISSING_TOKEN = -1007,
	IABHELPER_UNKNOWN_ERROR = -1008,
	IABHELPER_SUBSCRIPTIONS_NOT_AVAILABLE = -1009,
	IABHELPER_INVALID_CONSUMPTION = -1010,
	IABHELPER_PURCHASE_PAYLOAD_VERIFICATION_FAILED = -1011,
}
