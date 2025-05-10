using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    private GameObject[] checkpoints;
    public int totalCheckpoints;
    public int currentCheckpoint = 1;
    private bool lapCompletedFully = false;
    private bool raceStarted = false; // NEW


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
    }

    void Start()
    {
        totalCheckpoints = transform.childCount;
        checkpoints = new GameObject[totalCheckpoints];
        for (int i = 0; i < transform.childCount; i++)
        {
            checkpoints[i] = transform.GetChild(i).gameObject;

            // Assign checkpoint number to each IndivCheckpoint
            var cp = checkpoints[i].GetComponent<IndivCheckpoint>();
            if (cp != null)
                cp.SetCheckpointNumber(i + 1); // 1-based index
        }
    }

    public void ResetCheckpoints()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].SetActive(i == 0);
        }
        currentCheckpoint = 1;
        lapCompletedFully = true;
        GameManager.Instance.UpdateCheckpointUI(currentCheckpoint, totalCheckpoints);
    }

    public void CheckpointReached(int checkpointNumber)
    {
        if (checkpointNumber != currentCheckpoint) return;

        if (!raceStarted)
        {
            raceStarted = true;
            Debug.Log("Race started!");
        }

        checkpoints[currentCheckpoint - 1].SetActive(false);
        if (currentCheckpoint < checkpoints.Length)
        {
            checkpoints[currentCheckpoint].SetActive(true);
            currentCheckpoint++;
            Debug.Log($"Checkpoint {currentCheckpoint - 1} passed!");
            GameManager.Instance.UpdateCheckpointUI(currentCheckpoint, totalCheckpoints);
        }
        else
        {
            Debug.Log("All checkpoints passed — lap complete");
            lapCompletedFully = true;
        }
    }

    public bool IsLapComplete() => lapCompletedFully;
    public void ConsumeLapCompletion() => lapCompletedFully = false;
    public bool HasRaceStarted() => raceStarted;

}
