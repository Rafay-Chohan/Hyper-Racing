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

    void Start() {
        rb = GetComponent<Rigidbody>();
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
}
