using UnityEngine;
using System.Collections;

public class IranUnityLoading : MonoBehaviour {

	public string NextSceneToLoad;

	// Use this for initialization
	void Start () {
		StartCoroutine(ShowSplash());
	}


	IEnumerator ShowSplash()
	{
		yield return new WaitForSeconds(3);
		Application.LoadLevel(NextSceneToLoad);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
