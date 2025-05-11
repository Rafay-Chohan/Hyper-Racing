using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CarPurchase : MonoBehaviour
{
    [SerializeField] private carpurchaselistScriptableObject carPurchaseList;
    public int playerCoins;
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject purchaseButtonPrefab;

    [Header("Player Object")]
    [SerializeField] GameObject Prefab;

    public TextMeshProUGUI CoinsText;

    private HashSet<string> purchasedCars = new HashSet<string>();

    private TextMeshProUGUI previousText;

    public TextMeshProUGUI AlertText;
    void Start()
    {
        AlertText.gameObject.SetActive(false);
        LoadPlayerData();
        LoadPurchasedCars();
        CoinsText.text = $"${playerCoins}";

        foreach (var car in carPurchaseList.carPurchaseList)
        {
            GameObject buttonObj = Instantiate(purchaseButtonPrefab, grid.transform);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI text = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            Image image = buttonObj.GetComponentInChildren<Image>();

            image.sprite = car.carImage.sprite;

            bool isPurchased = purchasedCars.Contains(car.SkinName);
            text.text = isPurchased ? "Owned" : $"${car.price}";
            if(SkinManager.Instance.selectedSkin == car)
            {
                text.text = "Selected";
                previousText = text;
            }

            button.onClick.AddListener(() => PurchaseCar(car, text));
 
        }
    }

    void LoadPlayerData()
    {
        playerCoins = PlayerPrefs.GetInt("Coins", 0);
    }

    void SavePlayerData()
    {
        PlayerPrefs.SetInt("Coins", playerCoins);
        PlayerPrefs.Save();
    }

    void LoadPurchasedCars()
    {
        string data = PlayerPrefs.GetString("PurchasedCars", "");
        purchasedCars = new HashSet<string>(data.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)));
    }

    void SavePurchasedCars()
    {
        string data = string.Join(",", purchasedCars);
        PlayerPrefs.SetString("PurchasedCars", data);
        PlayerPrefs.Save();
    }

    private void PurchaseCar(carpurchaseScriptableObject skin, TextMeshProUGUI text)
    {
        if (purchasedCars.Contains(skin.SkinName))
        {
            Debug.Log("Already purchased.");
            applySkin(skin,text);
            return;
        }

        if (playerCoins >= skin.price)
        {
            playerCoins -= skin.price;
            purchasedCars.Add(skin.SkinName);
            SavePurchasedCars();
            Debug.Log($"Purchased {skin.SkinName} for {skin.price} coins!");
            SkinManager.Instance.selectedSkin = skin;

            text.text = "Owned";
        }
        else
        {
            AlertText.gameObject.SetActive(true);
            AlertText.text = $"You need {skin.price-playerCoins} more coins to purchase this car!";
            Debug.Log($"You need {skin.price-playerCoins} more coins to purchase this car!");
            StartCoroutine(HideAlertAfterDelay(2f));
            return;
        }

        SavePlayerData();
        applySkin(skin,text);
        CoinsText.text = $"${playerCoins}";
    }

    private IEnumerator HideAlertAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        AlertText.gameObject.SetActive(false);
    }
    void applySkin(carpurchaseScriptableObject skin,TextMeshProUGUI text)
    {

        SkinManager.Instance.selectedSkin = skin;
        text.text = "Selected";
        if(previousText!=text)
        {   
            previousText.text = "Owned";
            previousText = text;
        }
    }
}
