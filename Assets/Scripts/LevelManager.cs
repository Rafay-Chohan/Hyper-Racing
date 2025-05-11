using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public GameObject buttonPrefab; 
    public Transform buttonContainer; 
    public int totalLevels = 2; 

    public TextMeshProUGUI coinsText; // 👈 Reference to the coin UI text

    public int playerXP;
    public int playerLevel;
    public int playerCoins;

    public TextMeshProUGUI XPText; // 👈 Reference to the coin UI text

    public Image xpFillImage;
    public TextMeshProUGUI levelText;

    

    void Start()
    {
        LoadPlayerData();
        UpdateXPBar();
        GenerateLevelButtons();
    }

    private void UpdateXPBar()
    {
        int xpNeeded = playerLevel * 200;
        float fillAmount = (float)playerXP / xpNeeded;
        xpFillImage.fillAmount = fillAmount;

   
        levelText.text = $"Level: {playerLevel}";
        XPText.text=$"{playerXP}/{xpNeeded}";
    }

    void LoadPlayerData()
    {
        playerXP = PlayerPrefs.GetInt("XP", 0);
        playerLevel = PlayerPrefs.GetInt("Level", 1);
        playerCoins = PlayerPrefs.GetInt("Coins", 0);
    }

    void SavePlayerData()
    {
        PlayerPrefs.SetInt("XP", playerXP);
        PlayerPrefs.SetInt("Level", playerLevel);
        PlayerPrefs.SetInt("Coins", playerCoins);
        PlayerPrefs.Save();
    }

    void TryUnlockLevel(int levelIndex)
    {
        int xpNeeded = playerLevel * 200;
        Debug.Log($"XP Needed to unlock Level {levelIndex}: {xpNeeded}");
        Debug.Log($"XP: {playerXP}");

        while (playerXP >= xpNeeded && playerLevel< totalLevels)
        {
            playerLevel++;
            SavePlayerData();
            Debug.Log($"LEVEL UP! Now Level {playerLevel}");
            xpNeeded = playerLevel * 200;
        }
    }

    
     void GenerateLevelButtons()
    {
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        for (int i = 1; i <= totalLevels; i++)
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            TextMeshProUGUI label = button.GetComponentInChildren<TextMeshProUGUI>();
            Button btn = button.GetComponent<Button>();

            Transform lockIcon = button.transform.Find("LockIcon");
            Transform coinText = button.transform.Find("CoinText");

            bool isUnlocked = (i == 1) || PlayerPrefs.GetInt($"UnlockedLevel_{i}", 0) == 1;
            if (playerLevel >= i)
            {
                PlayerPrefs.SetInt($"UnlockedLevel_{i}", 1);
                isUnlocked = true;
            }

            if (isUnlocked)
            {
                label.text = $"Level {i}";
                lockIcon?.gameObject.SetActive(false);
                coinText?.gameObject.SetActive(false);

                int levelIndex = i;
                btn.onClick.AddListener(() => LoadLevel(levelIndex));
            }
            else
            {
                label.text = $"Locked ({i})";
                lockIcon?.gameObject.SetActive(true);
                if (coinText != null) coinText.GetComponent<TextMeshProUGUI>().text = "100 Coins";

                int levelIndex = i;
                btn.onClick.AddListener(() => TryUnlockLevel(levelIndex));
            }
        }
    }

    void LoadLevel(int levelIndex)
    {
        Debug.Log($"Loading Level {levelIndex}");
        UnityEngine.SceneManagement.SceneManager.LoadScene($"Level {levelIndex}");
    }
}