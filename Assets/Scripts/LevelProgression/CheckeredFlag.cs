using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckeredFlag : MonoBehaviour
{

    int totalLaps = 0;
    int currentLap = 1;
    [SerializeField] private TextMeshPro flagTextBox;

    void Start()
    {
       totalLaps = LapManager.Instance.GetTotalLaps();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var checkpointManager = CheckpointManager.Instance;

        if (!checkpointManager.HasRaceStarted())
        {
            Debug.Log("Race start detected. Resetting checkpoints.");
            flagTextBox.text = (currentLap + 1 > totalLaps) ? "FINISH" : "LAP "+(currentLap + 1).ToString();
            currentLap++;
            checkpointManager.ResetCheckpoints(); // ✅ show first checkpoint
            return;
        }

        if (checkpointManager.IsLapComplete())
        {
            flagTextBox.text = (currentLap + 1 > totalLaps) ? "FINISH" : "LAP "+(currentLap + 1).ToString();
            Debug.Log("Lap completed fully!");
            LapManager.Instance.LapCompleted();
            checkpointManager.ConsumeLapCompletion();
            currentLap++;
        }
        else
        {
            Debug.Log("Tryna be smart huh?");
        }
    }

}
