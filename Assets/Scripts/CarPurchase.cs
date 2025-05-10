using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarPurchase : MonoBehaviour
{
    [SerializeField] private carpurchaselistScriptableObject carPurchaseList; // Reference to the ScriptableObject containing car purchase data
    public int playerCoins;
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject purchaseButtonPrefab; // Prefab for the purchase button

    [Header("Player Object")]
    [SerializeField] GameObject Prefab;

    public TextMeshProUGUI CoinsText;

    private Material material; 
    void Start()

    {
        LoadPlayerData(); // Load player data at the start
        CoinsText.text = $"${playerCoins}"; // Display initial coins
        
        foreach (var car in carPurchaseList.carPurchaseList) // Loop through each car in the purchase list
        {

            GameObject buttonObj = Instantiate(purchaseButtonPrefab, grid.transform);
            // Get components from the instantiated object
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI text = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            Image image = buttonObj.GetComponentInChildren<Image>();
            text.text = $"${car.price}"; // Set the button text to the car price
            image.sprite = car.carImage.sprite; // Set the button image to the car image
            button.onClick.AddListener(() => PurchaseCar(car)); // Add a listener to the button to call PurchaseCar when clicked
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

    private void PurchaseCar(carpurchaseScriptableObject skin)
    {
        if (playerCoins >= skin.price)
        {
            playerCoins -= skin.price;
            Debug.Log($"Purchased {skin.SkinName} for {skin.price} coins!");

            SkinManager.Instance.selectedSkin = skin; // Save for other scenes

        }
        else
        {
            Debug.Log("Not enough coins!");
        }
        SavePlayerData(); // Save player data after purchase
        CoinsText.text = $"${playerCoins}"; // Display initial coins
    }
}