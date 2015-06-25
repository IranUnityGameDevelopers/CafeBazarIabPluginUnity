using UnityEngine;
using System.Collections;
using CafeBazarIab;

public class Purchase {

	public string ItemType {
		get;
		set;
	}

	public string Sku {
		get;
		set;
	}

	public string Token {
		get;
		set;
	}
	public string OrderId {
		get;
		set;
	}
	public string PackageName {
		get;
		set;
	}
	public float PurchaseTime {
		get;
		set;
	}
	public float PurchaseState {
		get;
		set;
	}
	public string DeveloperPayload {
		get;
		set;
	}
	public string Signature {
		get;
		set;
	}
	public string OriginalJson {
		get;
		set;
	}
}
