using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    // speedometer variables
    public GameObject needle;  
    private float minNeedleRotationAngle = 220f; 
    private float maxNeedleRotationAngle = -44f;
    private float desiredNeedlePosition = 0f;
    public GameObject defaultPowerupImage;
    public GameObject nitroPowerupImage; 
    public GameObject missilePowerupImage;

    // checkpoint variables
    public TextMeshProUGUI checkpointText;

    // race over variables
    public GameObject gameOverUI;

    void Start()
    {

    }

    public void UpdateNeedle(float vehicleSpeed)
    {
        desiredNeedlePosition = minNeedleRotationAngle - maxNeedleRotationAngle;

        // speedometer is 0 to 180
        float temp = vehicleSpeed / 180f; // normalizing speed between 0 and 1
        needle.transform.localRotation = Quaternion.Euler(0, 0, (minNeedleRotationAngle - temp * desiredNeedlePosition));
    }

    public void UpdateCheckpointUI(int currentCheckpoint, int totalCheckpoints)
    {
        checkpointText.text = $"{currentCheckpoint}/{totalCheckpoints}";
        checkpointText.transform.localScale = Vector3.zero;

        checkpointText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

        // play a sound when checkpoint reached too
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void RaceOver()
    {
        Time.timeScale = 0.2f; // everything runs at 20% speed
        StartCoroutine(ShowGameOverUIAfterDelay());
    }
    IEnumerator ShowGameOverUIAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2f);
        gameOverUI.SetActive(true);
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

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
        
        // To make sure it also works in the Unity Editor
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
