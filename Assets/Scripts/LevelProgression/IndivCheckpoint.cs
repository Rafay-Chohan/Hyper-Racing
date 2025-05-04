using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndivCheckpoint : MonoBehaviour
{
    private CheckpointManager manager;
    private int checkpointNumber;

    void Start()
    {
        manager = GetComponentInParent<CheckpointManager>();
        if (manager == null)
        {
            Debug.LogError("CheckpointManager not found in parent!", this);
            return;
        }
        // 2. Parse checkpoint number from the GameObject's name (e.g., "Checkpoint (5)")
        if (int.TryParse(gameObject.name.Replace("Checkpoint (", "").Replace(")", ""), out checkpointNumber))
        {
            Debug.Log($"Assigned checkpoint number: {checkpointNumber}");
        }
        else
        {
            Debug.LogError($"Failed to parse checkpoint number from name: {gameObject.name}", this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.CheckpointReached(checkpointNumber);
        }
    }
}
