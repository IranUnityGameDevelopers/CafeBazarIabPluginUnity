using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CafeBazarIab
{

	[CustomEditor(typeof(StoreHandler))]
	public class ShopItemInspector : Editor {
		
		public override void OnInspectorGUI () {
			serializedObject.Update();
		//	EditorGUILayout.PropertyField(serializedObject.FindProperty("SKU"));
		//	EditorGUILayout.PropertyField(serializedObject.FindProperty("_Type"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("shopItems") , true );
			serializedObject.ApplyModifiedProperties();
		}
	}

}
