using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // Progression variables
    public int playerXP;
    public int playerLevel;
    public int playerCoins;

    // speedometer variables
    public GameObject needle;  
    private float minNeedleRotationAngle = 220f; 
    private float maxNeedleRotationAngle = -44f;
    private float desiredNeedlePosition = 0f;
    public GameObject defaultPowerupImage;
    public GameObject nitroPowerupImage; 
    public GameObject missilePowerupImage;
    public TextMeshProUGUI PosText;
    public TextMeshProUGUI CoinsText;

    private int totallevels = 2; // Set this to the total number of levels in your game


    // checkpoint variables
    public TextMeshProUGUI checkpointText;

    public TextMeshProUGUI AlertText;
    public UnityAction OnRaceEnded;

    // race over variables
    public GameObject gameOverUI;

    public int position;

    public TextMeshProUGUI XPText; 

    public Slider xpFillImage;
    public TextMeshProUGUI levelText;
    
    void Awake()
    {
        // Ensure only one instance of GameManager exists
        SplineLapManager.Instance.ResetRace();
    }
    
    void Start()
    {
        // Optionally initialize UI
        LoadPlayerData();
        gameOverUI.SetActive(false);
    
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

    public void UpdateNeedle(float vehicleSpeed)
    {
        desiredNeedlePosition = minNeedleRotationAngle - maxNeedleRotationAngle;
        float normalizedSpeed = Mathf.Clamp01(vehicleSpeed / 180f);
        float angle = minNeedleRotationAngle - normalizedSpeed * desiredNeedlePosition;
        needle.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void UpdateCheckpointUI(int currentCheckpoint, int totalCheckpoints)
    {
        checkpointText.text = $"{currentCheckpoint}/{totalCheckpoints}";
        checkpointText.transform.localScale = Vector3.zero;
        checkpointText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;

        AudioListener.pause = true; // pause all audio sources in the game
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; 

        AudioListener.pause = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        
        AudioListener.pause = false;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RaceOver()
    {
        Debug.Log("COINS1" + playerCoins);
        AwardXPAndCoins();
        SavePlayerData();
        LoadPlayerData();
        Debug.Log("COINS2" + playerCoins);
        CoinsText.text = $"{playerCoins}";
        Time.timeScale = 0.2f;
        OnRaceEnded?.Invoke();
        PosText.text=$"{position} / {SplineLapManager.Instance.racers.Count}";
        StartCoroutine(ShowGameOverUIAfterDelay());

        SplineLapManager.Instance. ResetRace();
        
    }

    private IEnumerator ShowGameOverUIAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2f);
        gameOverUI.SetActive(true);
        UpdateXPBar();

        // return to normal speed
        Time.timeScale = 1f;
    }

    public void UpdatePowerupUI(string powerupName)
    {
        defaultPowerupImage.SetActive(false);
        nitroPowerupImage.SetActive(false);
        missilePowerupImage.SetActive(false);

        switch (powerupName)
        {
            case "Nitro":
                nitroPowerupImage.SetActive(true);
                break;
            case "Missile":
                missilePowerupImage.SetActive(true);
                break;
            default:
                defaultPowerupImage.SetActive(true);
                break;
        }
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

    void AwardXPAndCoins()
    {
        
        int xpEarned = 50;
        int coinEarned = 20;

        if (position == 1)
        {
            xpEarned += 20;
            coinEarned = 100;
        }
        else if (position == 2)
        {
            coinEarned = 60;
        }

        playerXP += xpEarned;
        playerCoins += coinEarned;


        CheckLevelUp();

        Debug.Log($"+{xpEarned} XP, +{coinEarned} Coins | XP: {playerXP}, Level: {playerLevel}, Coins: {playerCoins}");
        
    }

    void CheckLevelUp()
    {
        int xpNeeded = playerLevel * 200;

        while (playerXP >= xpNeeded && playerLevel<totallevels)
        {
            playerLevel++;
            Debug.Log($"LEVEL UP! Now Level {playerLevel}");
            xpNeeded = playerLevel * 200;
        }
    }

    private IEnumerator HideAlertAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        AlertText.gameObject.SetActive(false);
    }

    public void NextLevel()
    {
        int xpNeeded = playerLevel * 200;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings && playerLevel >= currentSceneIndex)
        {
            Time.timeScale = 1f; // reset time scale in case it's slowed down
            SceneManager.LoadScene(nextSceneIndex);
        }
        else if(playerXP < xpNeeded)
        {
            AlertText.gameObject.SetActive(true);
            AlertText.text = $"You need {xpNeeded-playerXP} more XP to unlock this level!";
            Debug.Log($"You need {xpNeeded} XP to unlock this level!");
            StartCoroutine(HideAlertAfterDelay(2f));
        }
        else
        {
            Debug.Log("No more levels. Returning to main menu.");
            LoadMainMenu(); // or handle game completion
        }
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;

        AudioListener.pause = false;

        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
