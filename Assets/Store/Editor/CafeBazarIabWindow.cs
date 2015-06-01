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
		private Vector2 scrollPosition = Vector2.zero;

		
		[MenuItem("Window/CafeBazaar")]
		public static void ShowWindow()
		{
			//Show existing window instance. If one doesn't exist, make one.
			EditorWindow.GetWindow(typeof(CafeBazarIabWindow));
		}
		

		
		
		private float itemsPadding = 40;
		
		void OnGUI(){
			var script = MonoScript.FromScriptableObject( this );
			string path = AssetDatabase.GetAssetPath( script );
			path = path.Replace("CafeBazarIabWindow.cs" , "");
			CafeBazarLogo =(Texture2D) AssetDatabase.LoadAssetAtPath(path + "logo.png" , typeof(Texture2D));
			Unity3dir =(Texture2D) AssetDatabase.LoadAssetAtPath(path + "unity3dir.png" , typeof(Texture2D));

			GUILayout.Label( new Rect((Screen.width - 256)/2 + 0f, 0f, 256f, 265f), new GUIContent("", CafeBazarLogo, ""));
			GUILayout.Label( new Rect((Screen.width - 128)/2 + 0f, 128f, 128f, 128f), new GUIContent("", Unity3dir, ""));



			GUI.Box(addPadding( new Rect(0f, 280f, Screen.width, Screen.height), 10 , 200 , 400), new GUIContent("Items", null, ""));

			if(GUI.Button( new Rect((Screen.width - 180)/2 + 0f, 180f, 180f, 45f), new GUIContent("Refresh", null, "")))
			{
				refreshItems();
			}

			if(GUI.Button( new Rect((Screen.width - 180)/2 + 0f, 230f, 180f, 45f), new GUIContent("Add Item", null, "")))
			{
				addItem();
			}

			EditorGUILayout.BeginVertical();
			
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

			for(int i = 0 ; i < items.Count ; i++) {
				EditorGUILayout.EnumPopup(items[i].GetComponent<ShopItem>()._Type);

			//	GUI.Label(addPadding( new Rect((Screen.width - 30)/2 + 0f, 300f + i * itemsPadding, 30f, 30f),10 , 30 , 160), new GUIContent(items[i].GetComponent<ShopItem>()._Type.ToString(), null, ""));
				
//				GUI.Label( new Rect((Screen.width - 30)/2 + -160f, 300f+ i * itemsPadding, 30f, 30f), new GUIContent(items[i].GetComponent<ShopItem>().SKU, null, ""));
//				
//				if(GUI.Button( new Rect((Screen.width - 50)/2 + 150f, 300f+ i * itemsPadding, 50f, 30f), new GUIContent("Delete", null, "")))
//				{
//					removeItem(i);
//
//				}
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
			items.RemoveAt(i);
			DestroyImmediate(GameObject.Find(i.ToString()));
		}		
		
		
		void addItem()
		{
			StoreHandler storeHandler = GameObject.FindObjectOfType<StoreHandler>();
			GameObject newShopItemObject = new GameObject();
			newShopItemObject.name = index.ToString();
			newShopItemObject.AddComponent<ShopItem>();
			newShopItemObject.GetComponent<ShopItem>().SKU = "";
			newShopItemObject.GetComponent<ShopItem>()._Type = ShopItemType.inapp;
			newShopItemObject.transform.parent = storeHandler.gameObject.transform;
			items.Add(newShopItemObject);
			index++;
			//Debug.Log (index + " , " + texts.Count + " , " + times.Count);
		}


	}

}