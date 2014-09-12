using UnityEngine;
using System.Collections;

public class ShopItem : MonoBehaviour {

	public string SKU;
	public ShopItemType _Type;

}


public enum ShopItemType {
	inapp ,
	subs ,
}
