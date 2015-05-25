using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Overlay : MonoBehaviour {

	public static Overlay Instance;
	
	public GameObject _Overlay;

	public Text OverlayText;

	void Awake()
	{
		Instance = this;
	}

	public void ShowOverlay(string _Text)
	{
		_Overlay.SetActive(true);
		OverlayText.text = _Text;
	}

	public void HideOverlay()
	{
		_Overlay.SetActive(false);
	}
}
