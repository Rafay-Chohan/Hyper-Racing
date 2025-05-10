using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{
    public string powerUpName = "Nitro"; // Name of the power-up
    public AudioSource pickupSound;
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Car")) {
            Debug.Log("Power-up collected: " + powerUpName);
            CarMechanics car = other.GetComponent<CarMechanics>();
            if (car != null) {
                if(pickupSound != null) {
                AudioSource.PlayClipAtPoint(pickupSound.clip, transform.position);
                } else {
                    Debug.LogWarning("Pickup sound not assigned!", this);
                }
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
