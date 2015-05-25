using UnityEngine;
using System.Collections;

public interface IStoreEventHandler  {

	void ProcessPurchase(string sku);

	void OnConsumeFinished(string sku);

	void OnGetPurchasesFinished(string allRawSKU , int length);

	void OnSetupSuccessful();

	//Errors

	void OnProblemSettingUpIAB(string message , StoreErrorCodes errorCode);
	
	void OnFailedToQueryInventory(string message, StoreErrorCodes errorCode);

	void OnMissingToken(string message, StoreErrorCodes errorCode , string sku = "");

	void OnSubscriptionNotAvilable(string message, StoreErrorCodes errorCode , string sku = "");
	
	void OnFailedToConsumePurchase(string message, StoreErrorCodes errorCode , string sku = "");
	
	void OnConsumeFinishedListenerError(string message, StoreErrorCodes errorCode , string sku = "");
	
	void OnPurchaseFailed(string message, StoreErrorCodes errorCode , string sku = "");
	
	void OnPurchasePayloadVerificationFailed(string message, StoreErrorCodes errorCode , string sku = "");

	void OnUserCancelled(string message, StoreErrorCodes errorCode , string sku = "");

	void OnUnknownError(StoreErrorCodes errorCode , string message = "" , string sku = "");
}
