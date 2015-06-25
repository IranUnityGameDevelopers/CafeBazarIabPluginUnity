using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CafeBazarIab
{
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

		public bool DebugMode;
		private bool StoreStarted = false;
		private List<ShopItem> shopItems;

		//&& !UNITY_EDITOR
		#if UNITY_ANDROID 

		public const int BILLING_RESPONSE_RESULT_OK = 0;


		private string Base64EncodedPublicKey;
		private string Payload;
		private AndroidJavaObject StoreController;
		private IStoreEventHandler EventHandler;
		private List<Purchase> cachedPurchaseList;


		/// <summary>
		/// Starts the store.
		/// </summary>
		/// <param name="_eventHandler">_event handler.</param>
		/// <param name="_publicKey">_public key.</param>
		/// <param name="_payload">_payload.</param>
		public void StartStore (IStoreEventHandler _eventHandler , string _publicKey , string _payload) {

			if (string.IsNullOrEmpty(_publicKey)) {
				Debug.LogError("Store : Public Key is empty or null");
				return;
			}
			if (string.IsNullOrEmpty(_payload)) {
				Debug.LogError("Store : Payload is empty or null");
				return;
			}
			Base64EncodedPublicKey = _publicKey;
			Payload = _payload;
			if (_eventHandler == null) {
				Debug.LogError("Store :  Event Handler is null");
				return;
			}
			EventHandler = _eventHandler;

			shopItems = new List<ShopItem>();

			shopItems = gameObject.GetComponentsInChildren<ShopItem>().ToList();

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

		/// <summary>
		/// Setups the successful event , called from java plugin.
		/// </summary>
		public void SetupSuccessful()
		{
			StoreStarted = true;
			if (EventHandler != null) {
				EventHandler.OnSetupSuccessful();
			}
		}

		/// <summary>
		/// Gets the shop item by SKU.
		/// </summary>
		/// <returns>The shop item by SKU.</returns>
		/// <param name="_sku">_sku.</param>
		public ShopItem GetShopItemBySKU(string _sku)
		{
			var item = from element in shopItems
				where element.SKU == _sku
					select element;
			return item.First();
		}

		public Purchase GetPurchaseBySKU(string _sku)
		{
			var item = from element in cachedPurchaseList
				where element.Sku == _sku
					select element;
			return item.First();
		}


		/// <summary>
		/// Gets the purchases.
		/// </summary>
		public void GetPurchases()
		{
			if (StoreStarted) {
				StoreController.Call("QueryInventory");
			}
		}

		/// <summary>
		/// Consume the specified shopItem.
		/// </summary>
		/// <param name="sku">Sku.</param>
		public void Consume(Purchase item)
		{
			if (StoreStarted) {
				StoreController.Call("Consume" , new AndroidJavaObject("java.lang.String" , item.Sku));
			}
		}

		/// <summary>
		/// Raises the error event , called from java plugin
		/// </summary>
		/// <param name="message">Error Message</param>
		public void OnError(string message)
		{
			if (EventHandler != null) {

				string[] msg = message.Split('@');

				StoreErrorCodes errorCode = StoreErrorCodes.DONOTHING;

				int result = 0;
				if (int.TryParse(msg[msg.Length - 1] ,out result)) {
					errorCode = (StoreErrorCodes) result;
				}

				ShopItem item = null;

				if (message.Split('{', '}').Length > 1) {
					string sku = message.Split('{', '}')[1] == null ? "" : message.Split('{', '}')[1];
					item = GetShopItemBySKU(sku);
				}
				if (errorCode == StoreErrorCodes.BILLING_RESPONSE_RESULT_BILLING_UNAVAILABLE 
				    || errorCode == StoreErrorCodes.IABHELPER_ERROR_BASE
				    || errorCode == StoreErrorCodes.IABHELPER_SEND_INTENT_FAILED
				    || errorCode == StoreErrorCodes.IABHELPER_REMOTE_EXCEPTION) {
					EventHandler.OnProblemSettingUpIAB(msg[0] , errorCode);
				}
				else if (errorCode == StoreErrorCodes.IABHELPER_INVALID_CONSUMPTION)
				{
					EventHandler.OnFailedToConsumePurchase(msg[0] , errorCode , item);
				}
				else if (errorCode == StoreErrorCodes.IABHELPER_MISSING_TOKEN)
				{
					EventHandler.OnMissingToken(msg[0] , errorCode , item);
				}
				else if (errorCode == StoreErrorCodes.IABHELPER_PURCHASE_PAYLOAD_VERIFICATION_FAILED
				         || errorCode == StoreErrorCodes.IABHELPER_VERIFICATION_FAILED) {
					EventHandler.OnPurchasePayloadVerificationFailed(msg[0] , errorCode , item);
				}
				else if (errorCode == StoreErrorCodes.IABHELPER_SUBSCRIPTIONS_NOT_AVAILABLE) {
					EventHandler.OnSubscriptionNotAvilable(msg[0] , errorCode , item);
				}
				else if (errorCode == StoreErrorCodes.IABHELPER_UNKNOWN_PURCHASE_RESPONSE) {
					EventHandler.OnPurchaseFailed(msg[0] , errorCode , item);
				}
				else if (errorCode == StoreErrorCodes.IABHELPER_USER_CANCELLED) {
					EventHandler.OnUserCancelled(msg[0] , errorCode , item);
				}
				else if (errorCode == StoreErrorCodes.IABHELPER_BAD_RESPONSE
				         || errorCode == StoreErrorCodes.IABHELPER_UNKNOWN_ERROR) {
					EventHandler.OnUnknownError(errorCode , msg[0] , item);
				}
			}
		}

		/// <summary>
		/// Get the purchases finished event , called from java plugin.
		/// </summary>
		/// <param name="allRawSKU">All raw SK</param>
		public void GetPurchasesFinished(string JsonArray)
		{
			if (EventHandler == null) {
				return;
			}

			JSONObject jsonObject = new JSONObject(JsonArray);
			cachedPurchaseList = new List<Purchase>();

			if (jsonObject.type == JSONObject.Type.ARRAY) {

				for (int i = 0; i < jsonObject.list.Count; i++) {
					Purchase p = new Purchase();
					p.ItemType = jsonObject.list[i]["ItemType"].str;
					p.OrderId = jsonObject.list[i]["OrderId"].str;
					p.PurchaseTime = jsonObject.list[i]["PurchaseTime"].n;
					p.Signature = jsonObject.list[i]["Signature"].str;
					p.Token = jsonObject.list[i]["Token"].str;
					p.DeveloperPayload = jsonObject.list[i]["DeveloperPayload"].str;
					p.PurchaseState = jsonObject.list[i]["PurchaseState"].n;
					p.PackageName = jsonObject.list[i]["PackageName"].str;
					p.OriginalJson = jsonObject.list[i]["OriginalJson"].str;
					p.Sku = jsonObject.list[i]["Sku"].str;
					cachedPurchaseList.Add(p);
					EventHandler.ProcessPurchase(p);
				}
				EventHandler.OnGetPurchasesFinished(JsonArray , jsonObject.list.Count);
			}
			else
			{
				EventHandler.OnGetPurchasesFinished(JsonArray , 0);
			}
		}

		/// <summary>
		/// Consumes the finished event , called from java plugin.
		/// </summary>
		/// <param name="sku">SKU</param>
		public void ConsumeFinished(string sku)
		{
			if (EventHandler != null) {
				EventHandler.OnConsumeFinished(GetPurchaseBySKU(sku));
			}
		}

		/// <summary>
		/// Purchase the specified item.
		/// </summary>
		/// <param name="item">Item</param>
		public void Purchase(ShopItem item)
		{
			StartCoroutine(delayAndShop(item));
		}

		IEnumerator delayAndShop(ShopItem item)
		{
			yield return new WaitForSeconds(1);
			if (StoreStarted) {	
				if (item._Type == ShopItemType.inapp) {
					StoreController.Call("launchPurchaseFlow" , new AndroidJavaObject("java.lang.String" , item.SKU));
				}
				else if (item._Type == ShopItemType.subs) {
					StoreController.Call("launchSubscriptionPurchaseFlow" , new AndroidJavaObject("java.lang.String" , item.SKU));
				}
			}
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

}
