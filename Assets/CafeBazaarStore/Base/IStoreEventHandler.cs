using UnityEngine;
using System.Collections;

namespace CafeBazarIab
{

	public interface IStoreEventHandler  {

		void ProcessPurchase(ShopItem item);

		void OnConsumeFinished(ShopItem item);

		void OnGetPurchasesFinished(string allRawSKU , int length);

		void OnSetupSuccessful();

		//Errors

		void OnProblemSettingUpIAB(string message , StoreErrorCodes errorCode);
		
		void OnFailedToQueryInventory(string message, StoreErrorCodes errorCode);

		void OnMissingToken(string message, StoreErrorCodes errorCode , ShopItem item = null);

		void OnSubscriptionNotAvilable(string message, StoreErrorCodes errorCode , ShopItem item = null);
		
		void OnFailedToConsumePurchase(string message, StoreErrorCodes errorCode , ShopItem item = null);
		
		void OnConsumeFinishedListenerError(string message, StoreErrorCodes errorCode , ShopItem item = null);
		
		void OnPurchaseFailed(string message, StoreErrorCodes errorCode , ShopItem item = null);
		
		void OnPurchasePayloadVerificationFailed(string message, StoreErrorCodes errorCode , ShopItem item = null);

		void OnUserCancelled(string message, StoreErrorCodes errorCode , ShopItem item = null);

		void OnUnknownError(StoreErrorCodes errorCode , string message = "" , ShopItem item = null);
	}

}
