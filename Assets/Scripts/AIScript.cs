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
    }

    void FixedUpdate()
    {
        if (waypoints == null || currentWaypoint >= waypoints.Length) 
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
}