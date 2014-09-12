using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {

	public static GameHandler Instance;

	public Sprite[] Feul;
	public int currentFeul = 4;
	public Image FeulView;


	void Awake()
	{
		Instance = this;
	}

	public void Drive()
	{
		if (currentFeul == 0) {
			ErrorOverlay.Instance.ShowOverlay("You Need More Gas!");
			return;
		}
		currentFeul--;
		FeulView.sprite = Feul[currentFeul];
	}

	public void GasBought()
	{
		if (currentFeul != 4) {
			currentFeul++;
			FeulView.sprite = Feul[currentFeul];
		}
	}

}
