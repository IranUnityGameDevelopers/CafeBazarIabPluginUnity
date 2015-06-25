using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace CafeBazarIab
{

	public class CafeBazarIabWindow : EditorWindow {

		public Texture2D CafeBazarLogo;
		public Texture2D Unity3dir;

		// Add menu item named "CafeBazarIabPlugin" to the Window menu
		private List<GameObject> items = new List<GameObject>();

		private int index = 0;
		private Vector2 scrollPos;
		
		[MenuItem("Window/CafeBazaar")]
		public static void ShowWindow()
		{
			//Show existing window instance. If one doesn't exist, make one.
			EditorWindow.GetWindow(typeof(CafeBazarIabWindow));
		}
		

		
		
		private float itemsHeight = 40;
		
		void OnGUI(){
			var script = MonoScript.FromScriptableObject( this );
			string path = AssetDatabase.GetAssetPath( script );
			path = path.Replace("CafeBazarIabWindow.cs" , "");
			CafeBazarLogo =(Texture2D) AssetDatabase.LoadAssetAtPath(path + "logo.png" , typeof(Texture2D));
			Unity3dir =(Texture2D) AssetDatabase.LoadAssetAtPath(path + "unity3dir.png" , typeof(Texture2D));

			EditorGUILayout.BeginVertical();
			
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label( new GUIContent("", CafeBazarLogo, "") , GUILayout.Height(150) , GUILayout.Width(300));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label( new GUIContent("", Unity3dir, "") , GUILayout.Height(100), GUILayout.Width(300));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();



			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if(GUILayout.Button( new GUIContent("Add Item", null, "") ,GUILayout.Width(100), GUILayout.Height(50)))
			{
				addItem();
			}

			GUILayout.FlexibleSpace();
			if(GUILayout.Button(new GUIContent("Refresh", null, "") ,GUILayout.Width(100), GUILayout.Height(50)))
			{
				refreshItems();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.Box(new GUIContent("Items", null, "") , GUILayout.MinHeight(30) , GUILayout.ExpandWidth(true));

			GUILayout.BeginHorizontal();

			GUILayout.Label("SKU");
			GUILayout.FlexibleSpace();
			GUILayout.Label("Item Type");
			GUILayout.FlexibleSpace();
			GUILayout.Label("");
			GUILayout.EndHorizontal();

			for(int i = 0 ; i < items.Count ; i++) {

				if (items[i] == null) {
					items.RemoveAt(i);
					continue;
				}
				ShopItem shopitem = items[i].GetComponent<ShopItem>();

				GUILayout.Box(new GUIContent("", null, "") , GUILayout.MinHeight(5) , GUILayout.ExpandWidth(true));


				GUILayout.BeginHorizontal();

				shopitem.SKU = EditorGUILayout.TextField(shopitem.SKU , GUILayout.Height(itemsHeight));

				shopitem.gameObject.name = shopitem.SKU;

				GUILayout.FlexibleSpace();

				shopitem._Type = (ShopItemType) EditorGUILayout.EnumPopup(shopitem._Type);


				GUILayout.FlexibleSpace();

				if (GUILayout.Button(new GUIContent("Delete", null, "") , GUILayout.Height(itemsHeight)))
				{
					removeItem(i);
				}

				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}

			EditorGUILayout.EndScrollView();

			EditorGUILayout.EndVertical();


		}

		void refreshItems ()
		{
			StoreHandler storeHandler = GameObject.FindObjectOfType<StoreHandler>();
			items.Clear();
			for (int i = 0; i < storeHandler.gameObject.transform.childCount; i++) {
				items.Add(storeHandler.gameObject.transform.GetChild(i).gameObject);
			}

		}

		public Rect addPadding(Rect rect , int padding , int minWidth , int maxWidth)
		{
			if (rect.width < minWidth) {
				rect.width = minWidth;
				rect.x = (Screen.width - rect.width)/2;
			}
			if (rect.width > maxWidth) {
				rect.width = maxWidth;
				rect.x = (Screen.width - rect.width)/2;
			}
			rect.xMin += padding;
			rect.width = rect.width - padding;

			return rect;
			//return new Rect(rect.xMin + padding , rect.yMax , rect.width - 2 * padding , rect.height);
		}

		void removeItem (int i)
		{
			string objectName = items[i].name;
			DestroyImmediate(GameObject.Find(objectName));
			refreshItems();
		}		
		
		
		void addItem()
		{
			StoreHandler storeHandler = GameObject.FindObjectOfType<StoreHandler>();
			GameObject newShopItemObject = new GameObject();
			newShopItemObject.name = index.ToString();
			newShopItemObject.AddComponent<ShopItem>();
			newShopItemObject.GetComponent<ShopItem>().SKU = "NewItem";
			newShopItemObject.GetComponent<ShopItem>()._Type = ShopItemType.inapp;
			newShopItemObject.transform.parent = storeHandler.gameObject.transform;
			refreshItems();
			//Debug.Log (index + " , " + texts.Count + " , " + times.Count);
		}


	}

}