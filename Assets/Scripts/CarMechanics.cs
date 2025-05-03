using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMechanics : MonoBehaviour
{
    public float speed = 20f;
    public float turnSpeed = 10f;
    private Rigidbody rb;
    public float gasInput, turnInput;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        KeyboardMove();
        OnScreenButtonMove();
        
    }
    void KeyboardMove(){
        // Forward/backward movement
        float moveInput = Input.GetAxis("Vertical");
        rb.AddForce(transform.forward * moveInput * speed, ForceMode.Acceleration);

        // Steering (turn only when moving)
        if (Mathf.Abs(moveInput) > 0.1f) {
            float turnInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.deltaTime);
        }
    }
    void OnScreenButtonMove(){
        rb.AddForce(transform.forward * gasInput * speed, ForceMode.Acceleration);
        if (Mathf.Abs(gasInput) > 0.1f) {
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
