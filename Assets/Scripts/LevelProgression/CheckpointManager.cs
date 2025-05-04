using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private GameObject[] checkpoints;
    private int currentCheckpoint = 1;

    void Start()
    {
        checkpoints = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            checkpoints[i] = transform.GetChild(i).gameObject;
            checkpoints[i].SetActive(i == 0);
        }
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
        }
        else
        {
            Debug.Log("All checkpoints passed! Level complete.");
        }
    }
}
