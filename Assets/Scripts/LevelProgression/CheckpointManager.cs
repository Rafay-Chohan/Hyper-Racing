using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private GameObject[] checkpoints;
    public int totalCheckpoints;
    public int currentCheckpoint = 1;
    private CheckeredFlag manager;
    public GameManager gameManager;

    void Start()
    {
        totalCheckpoints = transform.childCount;
        checkpoints = new GameObject[totalCheckpoints];
        for (int i = 0; i < transform.childCount; i++)
        {
            checkpoints[i] = transform.GetChild(i).gameObject;
        }

        manager = transform.parent.GetComponentInChildren<CheckeredFlag>();
        if (manager == null)
        {
            Debug.LogError("CheckeredFlag not found in siblings!", this);
            return;
        }
    }

    public void ResetCheckpoints()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            checkpoints[i].SetActive(i == 0);
        }
        currentCheckpoint = 1;
        gameManager.UpdateCheckpointUI(currentCheckpoint, totalCheckpoints);
    }

    public void CheckpointReached(int checkpointNumber)
    {
        if (checkpointNumber != currentCheckpoint) return;
        checkpoints[currentCheckpoint - 1].SetActive(false);
        if (currentCheckpoint < checkpoints.Length)
        {
            checkpoints[currentCheckpoint].SetActive(true);
            currentCheckpoint++;
            Debug.Log($"Checkpoint {currentCheckpoint - 1} passed!");
            gameManager.UpdateCheckpointUI(currentCheckpoint, totalCheckpoints);
        }
        else
        {
            Debug.Log("Final checkpoint passed for lap!");
            manager.lapCompletedFully = true;
        }
    }

}
