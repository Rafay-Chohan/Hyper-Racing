using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    public float impactForce = 10000f;
    public float impactDuration = 2f;
    public float controlReductionFactor = 0.5f;
    void OnTriggerEnter(Collider collision) {
        
        Rigidbody carRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        AIScript aiScript = collision.gameObject.GetComponent<AIScript>();
        CarMechanics carMechanics = collision.gameObject.GetComponent<CarMechanics>();
        if (aiScript != null)
        {
            Debug.Log("Missile hit AI!");
            // aiScript.KnockUp();
        }
        else{
            Debug.Log("Missile hit AI!");
            // carMechanics.KnockUp();
        }


        // Destroy the missile after impact
        Destroy(gameObject);
    }
    /*private System.Collections.IEnumerator RecoverControl(GameObject car)
    {
        // Temporarily reduce control (e.g., lower traction)
        CarController carController = car.GetComponent<CarController>();
        if (carController != null)
        {
            float originalTraction = carController.traction;
            carController.traction = originalTraction * 0.5f;
            yield return new WaitForSeconds(driftDuration);
            carController.traction = originalTraction;
        }
    }*/
}
