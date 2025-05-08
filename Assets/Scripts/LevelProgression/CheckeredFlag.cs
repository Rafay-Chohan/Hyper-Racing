using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckeredFlag : MonoBehaviour
{
    [HideInInspector] public bool lapCompletedFully = true;
    int currentLap = 0;
    private LapManager manager;


    void Start()
    {
        manager = GetComponentInParent<LapManager>();
        if (manager == null)
        {
            Debug.LogError("LapManager not found in parent!", this);
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (lapCompletedFully)
        {
            if (other.CompareTag("Player"))
            {
                currentLap += 1;
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
