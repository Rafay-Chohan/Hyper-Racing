using System.Collections;
using UnityEngine;
using TMPro;

public class CarMechanics : MonoBehaviour
{
    public float speed = 20f;
    public float turnSpeed = 10f;
    private Rigidbody rb;
    public float gasInput, turnInput;
    private float currentSpeed = 0f;


    private string powerUpName;
    bool isPowerUpAvailable = false;
    private float nosMultiplier = 2f; // nosMultiplier for speed boost
    private float nosDuration = 3f; // Duration of NOS effect 

    [Header("Missile Settings")]
    public Transform missileSpawnPoint; // Assign in Inspector (empty GameObject at rear of car)
    public GameObject missilePrefab;

    public ParticleSystem leftExhaust;
    public ParticleSystem rightExhaust;
    public GameObject positionText;
    public TextMeshProUGUI PosText;

    public GameManager gameManager;
   void Start()
    {
        rb = GetComponent<Rigidbody>();
        SplineLapManager.Instance.RegisterRacer(transform, gameObject.name);
        gameManager.OnRaceEnded += PositionTextDisable; // Subscribe to the event
        
        if (SkinManager.Instance == null || SkinManager.Instance.selectedSkin == null)
        {
            Debug.LogWarning("No skin selected.");
            return;
        }
        else
        {
            Debug.Log("Nope");
        }

        var body = transform.Find("Body");
        if (body != null)
        {
            var renderer = body.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = SkinManager.Instance.selectedSkin.material;
                Debug.Log("Skin applied to car: " + SkinManager.Instance.selectedSkin.SkinName);
            }
            else
            {
                Debug.LogWarning("MeshRenderer not found on 'body' part.");
            }
        }
        else
        {
            Debug.LogWarning("'body' child not found on the car.");
        }
    }


    void FixedUpdate()
    {
        KeyboardMove();
        OnScreenButtonMove();

        // Lap tracking

        SplineLapManager.Instance.SetRacerLap(transform, LapManager.Instance.currentLap);
        int myPosition = SplineLapManager.Instance.GetRacerPosition(transform);
        gameManager.position=myPosition;
        PosText.text = $"{myPosition} / {SplineLapManager.Instance.racers.Count}";

    }
    void PositionTextDisable()
    {
        positionText.SetActive(false);
    }
    void KeyboardMove()
    {
        float moveInput = Input.GetAxis("Vertical");
        rb.AddForce(transform.forward * moveInput * speed, ForceMode.Acceleration);
        currentSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        gameManager.UpdateNeedle(currentSpeed);

        if (currentSpeed > 0.5f)
        {
            float turn = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up * turn * turnSpeed * Time.deltaTime);
        }
    }

    void OnScreenButtonMove()
    {
        rb.AddForce(transform.forward * gasInput * speed, ForceMode.Acceleration);
        float cs = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        if (cs > 0.5f)
            transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.deltaTime);
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
