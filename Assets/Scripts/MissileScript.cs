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
            aiScript.KnockUp();
            // Destroy the missile after impact
            Destroy(gameObject);
        }
        else if(carMechanics != null)
        {
            Debug.Log("Missile hit AI!");
            // carMechanics.KnockUp();
        }


        // Destroy the missile after impact
        Destroy(gameObject);
    }
    
}
