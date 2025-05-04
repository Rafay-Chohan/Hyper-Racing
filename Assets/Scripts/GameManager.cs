using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CarMechanics carMechanics;

    public GameObject needle;  
    private float minNeedleRotationAngle = 220f; 
    private float maxNeedleRotationAngle = -44f;
    private float desiredNeedlePosition = 0f;

    private float vehicleSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        vehicleSpeed = carMechanics.currentSpeed;
    }

    void FixedUpdate()
    {
        UpdateNeedle();
    }

    void UpdateNeedle()
    {
        desiredNeedlePosition = minNeedleRotationAngle - maxNeedleRotationAngle;

        // speedometer is 0 to 180
        float temp = vehicleSpeed / 180f; // normalizing speed between 0 and 1
        needle.transform.localRotation = Quaternion.Euler(0, 0, (minNeedleRotationAngle - temp * desiredNeedlePosition));
    }
}
