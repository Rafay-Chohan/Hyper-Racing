using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // speedometer variables
    public GameObject needle;  
    private float minNeedleRotationAngle = 220f; 
    private float maxNeedleRotationAngle = -44f;
    private float desiredNeedlePosition = 0f;

    // checkpoint variables
    public TextMeshProUGUI checkpointText;

    void Start()
    {

    }

    public void UpdateNeedle(float vehicleSpeed)
    {
        desiredNeedlePosition = minNeedleRotationAngle - maxNeedleRotationAngle;

        // speedometer is 0 to 180
        float temp = vehicleSpeed / 180f; // normalizing speed between 0 and 1
        needle.transform.localRotation = Quaternion.Euler(0, 0, (minNeedleRotationAngle - temp * desiredNeedlePosition));
    }

    public void UpdateCheckpointUI(int currentCheckpoint, int totalCheckpoints)
    {
        checkpointText.text = $"{currentCheckpoint}/{totalCheckpoints}";
    }
}
