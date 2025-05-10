using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckeredFlag : MonoBehaviour
{
    [HideInInspector] public bool lapCompletedFully = true;
    int currentLap = 0;
    int totalLaps = 0;
    [SerializeField] private TextMeshPro flagTextBox;
    private LapManager manager;


    void Start()
    {
        manager = GetComponentInParent<LapManager>();
        if (manager == null)
        {
            Debug.LogError("LapManager not found in parent!", this);
            return;
        }
        totalLaps = manager.GetTotalLaps();
    }

    void OnTriggerEnter(Collider other)
    {
        if (lapCompletedFully)
        {
            if (other.CompareTag("Player"))
            {
                currentLap += 1;
                flagTextBox.text = (currentLap + 1 > totalLaps) ? "FINISH" : "LAP "+(currentLap + 1).ToString();
                manager.LapCompleted(currentLap);
                lapCompletedFully = false;
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            { 
                Debug.Log($"Tryna be smart huh?");
            }
        }
    }
}
