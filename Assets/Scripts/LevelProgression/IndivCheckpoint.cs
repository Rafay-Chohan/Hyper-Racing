using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndivCheckpoint : MonoBehaviour
{
    private CheckpointManager manager;
    private int checkpointNumber;

    void Start()
    {
        manager = CheckpointManager.Instance;

        if (manager == null)
        {
            Debug.LogError("CheckpointManager not found in parent!", this);
        }
    }

    public void SetCheckpointNumber(int number)
    {
        checkpointNumber = number;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.CheckpointReached(checkpointNumber);
        }
    }
}
