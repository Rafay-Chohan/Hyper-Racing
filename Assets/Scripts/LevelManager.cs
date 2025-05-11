using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; 

public class LevelManager : MonoBehaviour
{
    public GameObject buttonPrefab; 
    public Transform buttonContainer; 
    public int totalLevels = 2; 

    public int playerXP=0;
    public int playerLevel=1;
    public int playerCoins=0;

    public TextMeshProUGUI XPText; 

    public Slider xpFillImage;
    public TextMeshProUGUI levelText;

    public TextMeshProUGUI AlertText;

    

    void Start()
    {
        LoadPlayerData();
        if ( xpFillImage || XPText || levelText)
        {
            UpdateXPBar();
        }
        else
        {
            Debug.LogWarning("XP UI elements are not assigned in the inspector.");
        }
        // UpdateXPBar();
        GenerateLevelButtons();
        AlertText.gameObject.SetActive(false);
        
    }

    private void UpdateXPBar()
    {
        int xpNeeded = playerLevel * 200;
        float fillAmount = (float)playerXP / xpNeeded;
        xpFillImage.value = fillAmount;

   
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

        if (playerXP >= xpNeeded && playerLevel< totalLevels)
        {
            playerLevel++;
            SavePlayerData();
            Debug.Log($"LEVEL UP! Now Level {playerLevel}");
            xpNeeded = playerLevel * 200;
        }
        else
        {
            AlertText.gameObject.SetActive(true);
            AlertText.text = $"You need {xpNeeded-playerXP} more XP to unlock this level!";
            Debug.Log($"You need {xpNeeded} XP to unlock this level!");
            StartCoroutine(HideAlertAfterDelay(2f));
        }
        
    }
    private IEnumerator HideAlertAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        AlertText.gameObject.SetActive(false);
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