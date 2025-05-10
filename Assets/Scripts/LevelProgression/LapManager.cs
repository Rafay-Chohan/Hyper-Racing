using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapManager : MonoBehaviour
{
    public int totalLaps;
    private CheckpointManager manager;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponentInChildren<CheckpointManager>();
        if (manager == null)
        {
            Debug.LogError("CheckpointManager not found in children!", this);
        }
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            if (gameManager == null)
            {
                Debug.LogError("GameManager not found in scene!", this);
            }
        }
    }

    public int GetTotalLaps()
    {
        return totalLaps;
    }

    public void LapCompleted(int lapNumber)
    {
        if (lapNumber > totalLaps)
        {
            Debug.Log("Race Over");
            gameManager.RaceOver();
        }
        else
        {

            Debug.Log($"Lap:{lapNumber}");
            manager.ResetCheckpoints();
        }
    }
}
