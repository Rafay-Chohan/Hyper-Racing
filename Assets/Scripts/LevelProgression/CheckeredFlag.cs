using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckeredFlag : MonoBehaviour
{
   void OnTriggerEnter(Collider other)
{
    if (!other.CompareTag("Player")) return;

    var checkpointManager = CheckpointManager.Instance;

    if (!checkpointManager.HasRaceStarted())
    {
        Debug.Log("Race start detected. Resetting checkpoints.");
        checkpointManager.ResetCheckpoints(); // ✅ show first checkpoint
        return;
    }

    if (checkpointManager.IsLapComplete())
    {
        Debug.Log("Lap completed fully!");
        LapManager.Instance.LapCompleted();
        checkpointManager.ConsumeLapCompletion();
    }
    else
    {
        Debug.Log("Tryna be smart huh?");
    }
}

}
