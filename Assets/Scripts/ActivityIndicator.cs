using UnityEngine;
using System.Collections;

public class ActivityIndicator : MonoBehaviour {

	public static ActivityIndicator Instance;

	public GameObject Indicator;


	void Awake()
	{
		Instance = this;
	}

	public void Show()
	{
		Indicator.SetActive(true);
	}

	public void Hide()
	{
		Indicator.SetActive(false);
	}
}
