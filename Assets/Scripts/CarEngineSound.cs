using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngineSound : MonoBehaviour
{
    public Rigidbody carRigidbody;
    public float minPitch = 0.8f;
    public float maxPitch = 2.5f;
    public float maxSpeed = 100f;
    public float volumeFactor = 0.7f;

    private AudioSource engineSound;

    void Start()
    {
        engineSound = GetComponent<AudioSource>();
        engineSound.loop = true;
        engineSound.Play();
    }

    void Update()
    {
        // Calculate the pitch based on the car's speed
        float speed = carRigidbody.velocity.magnitude;
        float pitch = Mathf.Lerp(minPitch, maxPitch, speed / maxSpeed);
        engineSound.pitch = pitch;

        // Adjust the volume for a more dynamic feel
        engineSound.volume = Mathf.Clamp01(speed / maxSpeed) * volumeFactor;
    }
}

