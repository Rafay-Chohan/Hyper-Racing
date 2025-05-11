using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{
    public string powerUpName = "Nitro"; // Name of the power-up
    public AudioSource pickupSound;
    private float floatAmplitude = 0.5f;  
    private float floatFrequency = 1f;     
    private float rotationSpeed = 30f;   

    public Sprite nitroSprite;
    public Sprite missileSprite;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        
        Transform canvasTransform = transform.Find("Canvas/Image");
        if (canvasTransform != null)
        {
            UnityEngine.UI.Image powerUpImage = canvasTransform.GetComponent<UnityEngine.UI.Image>();
            if (powerUpImage != null)
            {
                switch (powerUpName.ToLower())
                {
                    case "nitro":
                        powerUpImage.sprite = nitroSprite;
                        break;
                    case "missile":
                        powerUpImage.sprite = missileSprite;
                        break;
                    default:
                        Debug.LogWarning("No matching sprite for powerUpName: " + powerUpName, this);
                        break;
                }
            }
            else
            {
                Debug.LogWarning("Image component not found on Canvas/Image", this);
            }
        }
        else
        {
            Debug.LogWarning("Canvas/Image not found in child hierarchy", this);
        }
    }

    void Update()
    {
        // Bobbing 
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // Rotation
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

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
