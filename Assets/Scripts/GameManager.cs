using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.SceneManagement;

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


    // checkpoint variables
    public TextMeshProUGUI checkpointText;
    public UnityAction OnRaceEnded;

    // race over variables
    public GameObject gameOverUI;

    public int position;
    

    
    void Start()
    {
        // Optionally initialize UI
        LoadPlayerData();
        gameOverUI.SetActive(false);
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
        
    }

    private IEnumerator ShowGameOverUIAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2f);
        gameOverUI.SetActive(true);

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
        int racePosition = SplineLapManager.Instance.GetRacerPosition(transform);
        int xpEarned = 50;
        int coinEarned = 10;

        if (racePosition == 1)
        {
            xpEarned += 20;
            coinEarned = 100;
        }
        else if (racePosition == 2)
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

        while (playerXP >= xpNeeded)
        {
            playerXP -= xpNeeded;
            playerLevel++;
            Debug.Log($"LEVEL UP! Now Level {playerLevel}");
            xpNeeded = playerLevel * 200;
        }
    }

    public void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1f; // reset time scale in case it's slowed down
            SceneManager.LoadScene(nextSceneIndex);
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
