using UnityEngine;
using System.Collections;

namespace CafeBazarIab
{

	/// <summary>
	/// Very simple unit tests for StoreHandler.cs
	/// </summary>
	public class StoreHandlerTests : MonoBehaviour {

		#if UNITY_ANDROID
		void OnEnable() {
			//TestConsumeFinished();
			//TestGetPurchasesFinished();
			TestGetItemBySKU();
		}

		void Start()
		{
			//TestConsumeFinished();
		}

		public void RunAllTests()
		{
			TestOnError();
			TestGetPurchasesFinished();
			TestConsumeFinished();
		}

		public void TestOnError()
		{
			StoreHandler.Instance.OnError("salambehamebarobache irani ke montazere in plugin boodan");
			StoreHandler.Instance.OnError("hahaha slaam@1123124");
			StoreHandler.Instance.OnError("Error@-1003");
			StoreHandler.Instance.OnError("Error {aminjoon}@-1007");
			StoreHandler.Instance.OnError("Error{=1---23213}@-1005");
			StoreHandler.Instance.OnError("Error{}[]@-1006");
		}

		public void TestGetPurchasesFinished()
		{
		//	StoreHandler.Instance.GetPurchasesFinished("");
		//	StoreHandler.Instance.GetPurchasesFinished("sdkkjhaskldjhsadkjahsLKJDHSKLAJFHDKLSJF12$#@$@%430543945820349ADS,JHFCKSDJF");
		//	StoreHandler.Instance.GetPurchasesFinished("dinasduasd,asduasduas,asdunsadu,saudsanudas,asudasudasd@3");
			StoreHandler.Instance.GetPurchasesFinished("gas,premium");
		//	StoreHandler.Instance.GetPurchasesFinished("gas,gas,gas,gas,gas,gas");
		//	StoreHandler.Instance.GetPurchasesFinished("-32948379487239423");
			//StoreHandler.Instance.GetPurchasesFinished("=343284u32894,348349832.djhsadkjas.23094823sdkjdfhs,ksdhs");

		}

		public void TestConsumeFinished()
		{
			StoreHandler.Instance.ConsumeFinished("skdjfhdskafjadsfdskfSDKLJFHSDKJF3243298472938!#@$@#41");
			StoreHandler.Instance.ConsumeFinished("amin joon");
			StoreHandler.Instance.ConsumeFinished("gas");
			StoreHandler.Instance.ConsumeFinished("premium");
		}

		public void TestGetItemBySKU()
		{
			Debug.Log (StoreHandler.Instance.GetShopItemBySKU("gas")._Type);
			Debug.Log (StoreHandler.Instance.GetShopItemBySKU("infinite_gas")._Type);
		}

		#endif
	}

}
