using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CafeBazarIab;

public class GameHandler : MonoBehaviour {

	public static GameHandler Instance;

	public StoreEventHandler eventHandler;
	public Sprite[] Feul;
	public int currentFeul;
	public Image FeulView;
	public bool isInfiniteGas = false;
	public bool isPremium = false;


	void Awake()
	{
		Instance = this;
		StoreHandler.Instance.StartStore(eventHandler , "MIHNMA0GCSqGSIb3DQEBAQUAA4G7ADCBtwKBrwDqcvluFwhix7+hEI9m9ZWEyfSLX1BfvpIrnUzKGGjCHaF/vDnX0p6gr0a4PhgUC8ug2UyITDjaWhtfyRkBs01ZNWofz0Da85jduAnvPmI0mTvtMjhg94llHbYk+V9GpSaWvJpqCVQAT0V5caS8LKptFe7QrDEEcfF+KJtd33RxoyC7rVyPtw36E/h71TvCt2LvUajx9kWonmlih4p7LbGnkBemzeaUFNu8VO1dlvsCAwEAAQ==" , "Payload");
	}

	public void Drive()
	{
		if (currentFeul == 0 && !isInfiniteGas) {
			Overlay.Instance.ShowOverlay("You Need More Gas!");
			return;
		}
		Overlay.Instance.ShowOverlay("Vroooom, you drove a few miles.");
		if (!isInfiniteGas) {
			currentFeul--;
			FeulView.sprite = Feul[currentFeul];
		}
	}

	public void GasBought()
	{
		if (currentFeul != 4 && !isInfiniteGas) {
			currentFeul++;
			FeulView.sprite = Feul[currentFeul];
		}
	}

	public void QueryInventory()
	{
		ActivityIndicator.Instance.Show();
		// call the store
		StoreHandler.Instance.GetPurchases();
	}

	public void Purchase(ShopItem item)
	{
		if (isInfiniteGas && item.SKU == "gas") {
			Overlay.Instance.ShowOverlay("No need! You're subscribed to infinite gas. Isn't that awesome?");
			return;
		}

		ActivityIndicator.Instance.Show();
		// purchase
		StoreHandler.Instance.Purchase(item);
	}

}
