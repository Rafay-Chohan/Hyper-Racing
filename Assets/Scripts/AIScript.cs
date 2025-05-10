using System.Collections;
using UnityEngine;


public class AIScript : MonoBehaviour
{
    [Header("Waypoint Assignment")]
    public Transform waypointsParent;
    public float speed = 20f;
    public float turnSpeed = 2f;
    public float brakeDistance = 5f;

    private Transform[] waypoints;
    private int currentWaypoint = 0;
    private Rigidbody rb;
    private float currentSpeed;
    private float nosMultiplier = 2f; // NOS multiplier for speed boost
    private float nosDuration = 3f; // Duration of NOS effect

    [Header("Missile Settings")]
    public Transform missileSpawnPoint; // Assign in Inspector (empty GameObject at rear of car)
    public GameObject missilePrefab;

    [Header("Collision Recovery")]
    public LayerMask barrierLayer;
    private float maxStuckTime = 3f;
    private float cooldown = 1f;
    private float stuckTimer = 0f;
    private bool isCollidingWithBarrier = false;
    private Vector3 lastSafePosition;

    // Lap count
    private int currentLap = 1;
    private int totalLaps;
    private LapManager lapManager;
    public ParticleSystem leftExhaust;
    public ParticleSystem rightExhaust;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Collect waypoints
        if (waypointsParent != null)
        {
            int count = waypointsParent.childCount;
            waypoints = new Transform[count];
            for (int i = 0; i < count; i++)
                waypoints[i] = waypointsParent.GetChild(i);
        }

        lapManager = LapManager.Instance;
        totalLaps = lapManager.totalLaps;
        lastSafePosition = transform.position;

        SplineLapManager.Instance.RegisterRacer(transform, gameObject.name);
        SplineLapManager.Instance.SetRacerLap(transform, currentLap);
    }

    void FixedUpdate()
    {
        // Movement and waypoint logic unchanged
        if (waypoints == null || (currentWaypoint >= waypoints.Length && currentLap >= totalLaps)) 
        {
            rb.velocity = Vector3.zero;  // Stop at final waypoint
            return;
        }
        Vector3 direction = waypoints[currentWaypoint].position - transform.position;
        direction.y = 0;
        Quaternion look = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, turnSpeed * Time.deltaTime);

        float distanceToWaypoint = direction.magnitude;
        rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
        if (distanceToWaypoint < brakeDistance)
            rb.AddForce(-rb.velocity.normalized * speed * 0.8f, ForceMode.Acceleration);

        if (distanceToWaypoint < 10f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentLap++;
                if (currentLap <= totalLaps)
                {
                    SplineLapManager.Instance.SetRacerLap(transform, currentLap);
                    currentWaypoint = 0;
                }
            }
            lastSafePosition = transform.position;
        }

        // Collision recovery unchanged...
        if (!isCollidingWithBarrier)
        {
            cooldown -= Time.fixedDeltaTime;
            if (cooldown <= 0) stuckTimer = 0;
        }
        else
        {
            cooldown = 1;
            stuckTimer += Time.fixedDeltaTime;
            if (stuckTimer >= maxStuckTime)
                ResetToLastWaypoint();
        }
        
    }

    void OnDrawGizmos()
    {
        if (waypoints != null)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] != null)
                {
                    Gizmos.DrawSphere(waypoints[i].position, 0.5f);
                    if (i < waypoints.Length - 1 && waypoints[i+1] != null)
                    {
                        Gizmos.DrawLine(waypoints[i].position, waypoints[i+1].position);
                    }
                }
            }
        }
    }
    public void ActivatePowerUp(string powerUpName)
    {
        Debug.Log("AI activated power-up: " + powerUpName);
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
    }
    private void FireMissile() {
        if (missilePrefab == null || missileSpawnPoint == null) return;
        Debug.Log("AI fired a missile!");   
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
        Debug.Log("NOS effect ended of AI.");
    }

    void OnCollisionStay(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & barrierLayer) != 0)
        {
            isCollidingWithBarrier = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & barrierLayer) != 0)
        {
            isCollidingWithBarrier = false;
        }
    }

    private void ResetToLastWaypoint()
    {
        transform.position = lastSafePosition;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        if (currentWaypoint > 0)
            currentWaypoint--;
        else
            currentWaypoint = waypoints.Length - 1;
        Debug.Log($"Reset to checkpoint: {currentWaypoint}");
        stuckTimer = 0f;
        isCollidingWithBarrier = false;
        
        Debug.Log("AI car was stuck - reset to last waypoint!");
    }

}