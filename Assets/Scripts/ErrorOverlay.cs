using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ErrorOverlay : MonoBehaviour {

	public static ErrorOverlay Instance;
	
	public GameObject Overlay;

	public Text OverlayText;

	void Awake()
	{
		Instance = this;
	}

	public void ShowOverlay(string _Text)
	{
		Overlay.SetActive(true);
		OverlayText.text = _Text;
	}

	public void HideOverlay()
	{
		Overlay.SetActive(false);
	}
}
