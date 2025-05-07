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
    private float maxStuckTime = 3f; // Time before resetting
    private float cooldown = 1f;
    
    private float stuckTimer = 0f;
    private bool isCollidingWithBarrier = false;
    private Vector3 lastSafePosition;

    // Lap count
    private int currentLap = 0;
    private int totalLaps;
    private LapManager lapManager;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Auto-collect waypoints from parent's children
        if (waypointsParent != null)
        {
            waypoints = new Transform[waypointsParent.childCount];
            for (int i = 0; i < waypointsParent.childCount; i++)
            {
                waypoints[i] = waypointsParent.GetChild(i);
            }
        }
        else
        {
            Debug.LogError("No waypoints parent assigned!", this);
        }

        lapManager = FindObjectOfType<LapManager>();
        totalLaps = lapManager.totalLaps;
        lastSafePosition = transform.position;
    }

    void FixedUpdate()
    {
        if (waypoints == null || (currentWaypoint >= waypoints.Length && currentLap >= totalLaps)) 
        {
            rb.velocity = Vector3.zero;  // Stop at final waypoint
            return;
        }

        // Calculate direction to waypoint
        Vector3 direction = waypoints[currentWaypoint].position - transform.position;
        direction.y = 0;  // Ignore vertical difference

        // Steering (rotate toward waypoint)
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);//To be changed 

        // Adaptive speed (slow down for sharp turns)
        float distanceToWaypoint = direction.magnitude;
        
        // Physics-based movement
        rb.AddForce(transform.forward * speed, ForceMode.Acceleration);

        // Progress to next waypoint
        if (distanceToWaypoint < 10f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentLap++;
                if (currentLap < totalLaps)
                {    
                    currentWaypoint = 0;
                    Debug.Log($"AI in Lap: {currentLap}");
                }
            }
            lastSafePosition = transform.position;
        }

        // check if stuck
        if (!isCollidingWithBarrier) 
        {
            cooldown -= Time.fixedDeltaTime;
            if (cooldown <= 0f) 
            {
                stuckTimer = 0f;
            }
        }
        else 
        {
            cooldown = 1f; // Reset cooldown if touching barriers
            stuckTimer += Time.fixedDeltaTime;
            if (stuckTimer >= maxStuckTime) 
            {
                ResetToLastWaypoint();
            }
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