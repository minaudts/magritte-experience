using UnityEngine;
using System.Text;
using System.Collections;

public class PigeonFlock : MonoBehaviour
{
    [SerializeField] private Transform[] flyDestinations;
    private Transform currentDestination;
    [SerializeField] private float flyingSpeed;
    [SerializeField] private float flyingHeight;
    [SerializeField] private float liftOffSpeed;
    [SerializeField] private float liftOffDistance;
    [SerializeField] private float descendSpeed;
    [SerializeField] private float stoppingDistance;
    private Transform initialPosition;
    private FlockState currentState;
    private int destinationIndex = 0;
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

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered pigeon flock");
        // Second to last flight, pick 1 pigeon to unparent so it stays where it is
        if (destinationIndex == flyDestinations.Length - 1)
        {
            PickRandomKeyPigeon();
        }
        if (destinationIndex < flyDestinations.Length)
        {
            // Start flying when player enters trigger
            Debug.Log("Flying to next destination");
            //currentState = FlockState.TakingOff;
            // Destination is endpoint of the flight
            currentDestination = flyDestinations[destinationIndex];
            StartCoroutine(FlyToDestination(currentDestination.position));
        }
    }

    private void PickRandomKeyPigeon()
    {
        int index = Random.Range(0, pigeons.Length - 1);
        pigeons[index].MakeKeyPigeon();
    }

    private IEnumerator FlyToDestination(Vector3 target)
    {
        // Look at target
        transform.LookAt(target, Vector3.up);
        // Get the starting position of the flock
        Vector3 startPos = transform.position;
        // Calculate the direction and distance to the target
        Vector3 direction = (target - startPos).normalized;
        float distance = Vector3.Distance(startPos, target);
        // Set target to a point defined by height, diection to target and liftoff distance
        Vector3 currentTarget = new Vector3(transform.position.x, flyingHeight, transform.position.z) + (direction * liftOffDistance);
        // Ascend to takeoff height
        yield return StartCoroutine(AscendToFlyingHeight(currentTarget));
        // Target is now actual destination but at flying height
        currentTarget = new Vector3(target.x, flyingHeight, target.z);
        // Fly at flying height
        yield return StartCoroutine(FlyAtConstantHeight(currentTarget));
        // Target is the actual target, with height set to 0 to make sure flock will go to the floor
        currentTarget = new Vector3(target.x, 0f, target.z);
        // Descend to landing distance
        yield return StartCoroutine(Land(currentTarget));
        destinationIndex++;
    }

    private IEnumerator AscendToFlyingHeight(Vector3 target)
    {
        while (Mathf.Abs(transform.position.y - flyingHeight) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, liftOffSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator FlyAtConstantHeight(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, flyingSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator Land(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, descendSpeed * Time.deltaTime);
            yield return null;
        }
        // Land at the target position
        transform.position = target;
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

public enum FlockState
{
    Idle,
    TakingOff,
    Flying,
    Landing,
}