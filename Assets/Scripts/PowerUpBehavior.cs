using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{
    public string powerUpName = "Nitro"; // Name of the power-up
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Car")) {
            Debug.Log("Power-up collected: " + powerUpName);
            CarMechanics car = other.GetComponent<CarMechanics>();
            if (car != null) {
                car.ActivatePowerUp(powerUpName);
                Destroy(gameObject); // Remove pickup
            }
        }
        else if (other.CompareTag("AI")) {
            Debug.Log("Power-up collected by AI: " + powerUpName);
            AIScript car = other.GetComponent<AIScript>();
            if (car != null) {
                car.ActivatePowerUp(powerUpName);
                Destroy(gameObject); // Remove pickup
            }
        }
    }
}
