using UnityEngine;
using System.Collections;

public class AutoSave : MonoBehaviour{
	public Texture2D CafeBazarLogo;
	public Texture2D Unity3dir;




	void OnGUI(){
		GUI.Label( new Rect((Screen.width - 256)/2 + 0f, 0f, 256f, 265f), new GUIContent("", CafeBazarLogo, ""));
		if(GUI.Button( new Rect((Screen.width - 180)/2 + 0f, 180f, 180f, 45f), new GUIContent("Add Item", null, ""))){}
		GUI.Label( new Rect((Screen.width - 128)/2 + 0f, 128f, 128f, 128f), new GUIContent("", Unity3dir, ""));
		GUI.Box( new Rect((Screen.width - 400)/2 + 0f, 250f, 400f, 600f), new GUIContent("Items", null, ""));
		GUI.Label( new Rect((Screen.width - 30)/2 + 0f, 275f, 30f, 30f), new GUIContent("Type", null, ""));
		GUI.Label( new Rect((Screen.width - 30)/2 + -160f, 275f, 30f, 30f), new GUIContent("SKU", null, ""));
		if(GUI.Button( new Rect((Screen.width - 50)/2 + 150f, 270f, 50f, 30f), new GUIContent("Delete", null, ""))){}
	}
}