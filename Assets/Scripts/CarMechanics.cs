using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMechanics : MonoBehaviour
{
    public float speed = 20f;
    public float turnSpeed = 10f;
    private Rigidbody rb;
    public float gasInput, turnInput;
    private float currentSpeed = 0f; 
    public GameManager gameManager;

    private string powerUpName;
    bool isPowerUpAvailable = false;
    private float nosMultiplier = 2f; // nosMultiplier for speed boost
    private float nosDuration = 3f; // Duration of NOS effect 

    [Header("Missile Settings")]
    public Transform missileSpawnPoint; // Assign in Inspector (empty GameObject at rear of car)
    public GameObject missilePrefab;

    public ParticleSystem leftExhaust;
    public ParticleSystem rightExhaust;

    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            if (gameManager == null)
            {
                Debug.LogError("GameManager not found in scene!", this);
                return;
            }
        }
    }

    void FixedUpdate() 
    {
        KeyboardMove();
        OnScreenButtonMove();
        
    }
    void KeyboardMove()
    {
        // Forward/backward movement
        float moveInput = Input.GetAxis("Vertical");
        rb.AddForce(transform.forward * moveInput * speed, ForceMode.Acceleration);
        currentSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;

        // Debug.Log("Current Speed: " + currentSpeed);
        gameManager.UpdateNeedle(currentSpeed); 

        // Steering (turn only if car is moving)
        if (currentSpeed > 0.5f) {  // Threshold to prevent tiny movements
            float turnInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.deltaTime);
        }
    }
    void OnScreenButtonMove()
    {
        rb.AddForce(transform.forward * gasInput * speed, ForceMode.Acceleration);
        float currentSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;

        // Steering (turn only if car is moving)
        if (currentSpeed > 0.5f) {  // Threshold to prevent tiny movements
            transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.deltaTime);
        }
    }
     public void SetGasInput(float input) {
        gasInput = input;
    }

    public void SetTurnInput(float input) {
        turnInput = input;
    }
    public void ActivatePowerUp(string powerUpName) {
        this.powerUpName = powerUpName;
        isPowerUpAvailable = true;
        Debug.Log("Power-up activated: " + powerUpName);
        gameManager.UpdatePowerupUI(powerUpName); 
    }
    
    public void UsePowerUp()
    {
        if (isPowerUpAvailable) {
            switch (powerUpName) {
                case "Nitro":
                    if (leftExhaust != null && !leftExhaust.isPlaying)
                        leftExhaust.Play();
                    if (rightExhaust != null && !rightExhaust.isPlaying)
                        rightExhaust.Play();
                    speed *= nosMultiplier;
                    Invoke(nameof(ResetNOS), nosDuration); // Duration of NOS effect
                    break;
                case "Missile":
                    FireMissile();
                    break;
                default:
                    Debug.Log("Unknown power-up: " + powerUpName);
                    break;
            }
            
            gameManager.UpdatePowerupUI("None");
            isPowerUpAvailable = false; // Reset power-up availability
        } else {
            Debug.Log("No power-up available to use.");
        }
    }
     private void FireMissile() {
        if (missilePrefab == null || missileSpawnPoint == null) return;

        GameObject missile = Instantiate(
            missilePrefab,
            missileSpawnPoint.position,
            missileSpawnPoint.rotation
        );

        // Add force backward (since missiles fire at opponents behind)
        Rigidbody missileRb = missile.GetComponent<Rigidbody>();
        if (missileRb != null) {
            missileRb.AddForce(missileSpawnPoint.forward * 50f, ForceMode.Impulse);
        }

        Destroy(missile, 5f); // Auto-destroy after 5 seconds
    }
    private void ResetNOS() {
        speed /= nosMultiplier;
        Debug.Log("NOS effect ended.");
    }
}
