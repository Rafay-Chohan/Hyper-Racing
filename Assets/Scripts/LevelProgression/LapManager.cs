using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapManager : MonoBehaviour
{
    public int totalLaps;
    private CheckpointManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponentInChildren<CheckpointManager>();
        if (manager == null)
        {
            Debug.LogError("CheckpointManager not found in children!", this);
        }
    }

    public void LapCompleted(int lapNumber)
    {
        if (lapNumber > totalLaps)
        {
            Debug.Log("Race Over");
        }
        else
        {
            Debug.Log($"Lap:{lapNumber}");
            manager.ResetCheckpoints();
        }
    }
}
