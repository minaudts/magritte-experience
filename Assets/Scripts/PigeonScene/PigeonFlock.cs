using UnityEngine;
using System.Text;


public class PigeonFlock : MonoBehaviour
{
    [SerializeField] private Transform[] flyDestinations;
    private Transform currentDestination;
    private Vector3 currentTarget;
    [SerializeField] private float flyingSpeed;
    [SerializeField] private float flyingHeight;
    [SerializeField] private float liftOffSpeed;
    [SerializeField] private float liftOffDistance;
    [SerializeField] private float descendSpeed;
    [SerializeField] private float stoppingDistance;
    private Transform initialPosition; 
    private FlockState currentState;
    private int flightsDone = 0;
    private Pigeon[] pigeons;
    // Start is called before the first frame update
    void Start()
    {
        // Initialize starting position
        initialPosition = transform;
        currentState = FlockState.Idle;
        pigeons = GetComponentsInChildren<Pigeon>();
    }

    private void Update()
    {
        if(currentState == FlockState.TakingOff)
        {
            // Start
            AscendToFlyingHeight();
        }
        else if(currentState == FlockState.Flying)
        {
            FlyToDestination();
        }
        else if(currentState == FlockState.Landing)
        {
            Land();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log(other.name + " entered pigeon flock");
        // Second to last flight, pick 1 pigeon to unparent so it stays where it is
        if(flightsDone == flyDestinations.Length - 1)
        {
            PickRandomKeyPigeon();
        }
        if(flightsDone < flyDestinations.Length)
        {
            Debug.Log("Flying to next destination");
            // Start flying when player enters trigger
            currentState = FlockState.TakingOff;
            // Destination is endpoint of the flight
            currentDestination = flyDestinations[flightsDone];
            // Calculate direction of current transform to destination
            Vector3 directionToDestination = Vector3.Normalize(currentDestination.position - transform.position);
            // Target is where flock is flying towards depending on current state
            currentTarget = new Vector3(transform.position.x, flyingHeight, transform.position.z) + (directionToDestination * liftOffDistance);
            // Rotate to look at destination
            transform.LookAt(currentDestination, Vector3.up);
        }
    }

    private void PickRandomKeyPigeon()
    {
        int index = Random.Range(0, pigeons.Length - 1);
        pigeons[index].MakeKeyPigeon();
    }

    private void AscendToFlyingHeight()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, liftOffSpeed * Time.deltaTime);
        if(Mathf.Abs(transform.position.y - flyingHeight) < 0.01f)
        {
            currentState = FlockState.Flying;
            currentTarget = new Vector3(currentDestination.position.x, flyingHeight, currentDestination.position.z);
        }
    }

    private void FlyToDestination()
    {
        // Update position to move towards target
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, flyingSpeed * Time.deltaTime);
        // If destination reached
        if(Vector3.Distance(transform.position, currentTarget) <= stoppingDistance)
        {
            currentState = FlockState.Landing;
            currentTarget = new Vector3(currentDestination.position.x, 0f, currentDestination.position.z);
        }
    }

    private void Land()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, descendSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, currentTarget) <= 0)
        {
            currentState = FlockState.Idle;
            flightsDone++;
        }
        
    }
    private void LogTransform(Transform t)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendJoin(", ", t.position.x, t.position.y, t.position.z);
        sb.AppendLine();
        sb.AppendJoin(", ", t.rotation.x, t.rotation.y, t.rotation.z);
        Debug.Log(sb.ToString());
    }
}

public enum FlockState {
    Idle,
    TakingOff,
    Flying,
    Landing,
}