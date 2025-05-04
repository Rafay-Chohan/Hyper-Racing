using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("AI") || other.CompareTag("Car")) {
            Debug.Log("Missile hit: " + other.name);
            /*AIController ai = other.GetComponent<AIController>();
            if (ai != null) {
                ai.StartSpin(spinForce, spinDuration);
            }*/
        }
        Destroy(gameObject);
    }
}
