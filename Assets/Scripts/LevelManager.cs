using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public GameObject buttonPrefab; 
    public Transform buttonContainer; 
    public int totalLevels = 2; 

    void Start()
    {
        GenerateLevelButtons();
    }

    void GenerateLevelButtons()
    {
        for (int i = 1; i <= totalLevels; i++)
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            button.GetComponentInChildren<TextMeshProUGUI>().text = $"Level {i}";
            int levelIndex = i; 
            button.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    void LoadLevel(int levelIndex)
    {
        Debug.Log($"Loading Level {levelIndex}");
        UnityEngine.SceneManagement.SceneManager.LoadScene($"Level {levelIndex}");
    }
}