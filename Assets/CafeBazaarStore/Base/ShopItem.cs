using UnityEngine;
using System.Collections;

namespace CafeBazarIab
{
	[System.Serializable]
	public class ShopItem : MonoBehaviour {
		public string SKU;
		public ShopItemType _Type;
	}


	public enum ShopItemType {
		inapp ,
		subs ,
	}

}
