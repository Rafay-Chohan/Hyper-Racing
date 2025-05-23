using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Added for TextMeshPro support

public class LapManager : MonoBehaviour
{
    public static LapManager Instance { get; private set; }

    [Header("Race Settings")]
    public int totalLaps;
    public int currentLap = 1;

    private CheckpointManager manager;

    public TextMeshProUGUI LapText;
    public GameObject LapUI; // Reference to the UI element that shows the lap count

    public GameManager gameManager;

    void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LapText.text = $"{currentLap}/{totalLaps}";

        // Initialize references
        manager = GetComponentInChildren<CheckpointManager>();
        if (manager == null)
            Debug.LogError("CheckpointManager not found in children!", this);
    }

    void Start()
    {
        gameManager.OnRaceEnded += DisableLapUI; // Subscribe to
       
    }

    void DisableLapUI()
    {
        LapUI.SetActive(false);
    }

    public int GetTotalLaps()
    {
        return totalLaps;
    }

    public void LapCompleted()
    {
        currentLap++;
        LapText.text = $"{currentLap}/{totalLaps}";
        if (currentLap > totalLaps)
        {
            Debug.Log("Race Over");
            gameManager.RaceOver();
        }
        else
        {
            Debug.Log($"Lap {currentLap} completed");
            manager.ResetCheckpoints();
        }
    }
}
