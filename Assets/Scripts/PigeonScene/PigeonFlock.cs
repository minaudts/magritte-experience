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
    private int destinationIndex = 0;
    private Pigeon[] _pigeons;
    private MoveToClickPoint _player;
    private bool _keyPigeonHasBeenPicked = false;
    // Start is called before the first frame update
    void Start()
    {
        _pigeons = GetComponentsInChildren<Pigeon>();
        _player = GameObject.FindObjectOfType<MoveToClickPoint>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered pigeon flock");
        // If player is running, pick random key pigeon
        if (!_keyPigeonHasBeenPicked && _player.IsRunning())
        {
            PickRandomKeyPigeon();
            _keyPigeonHasBeenPicked = true;
        }
        // Always fly to next destination in a loop
        Debug.Log("Flying to next destination");
        // Destination is endpoint of the flight
        currentDestination = flyDestinations[destinationIndex];
        StartCoroutine(FlyToDestination(currentDestination.position));
    }

    private void PickRandomKeyPigeon()
    {
        int index = Random.Range(0, _pigeons.Length - 1);
        _pigeons[index].MakeKeyPigeon();
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
        // Update destination
        destinationIndex = (destinationIndex + 1) % flyDestinations.Length;
    }

    private IEnumerator AscendToFlyingHeight(Vector3 target)
    {
        SetPigeonsState(FlockState.TakingOff);
        while (Mathf.Abs(transform.position.y - flyingHeight) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, liftOffSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator FlyAtConstantHeight(Vector3 target)
    {
        SetPigeonsState(FlockState.Flying);
        while (Vector3.Distance(transform.position, target) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, flyingSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator Land(Vector3 target)
    {
        SetPigeonsState(FlockState.Landing);
        while (Vector3.Distance(transform.position, target) > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, descendSpeed * Time.deltaTime);
            yield return null;
        }
        // Land at the target position
        transform.position = target;
        SetPigeonsState(FlockState.Idle);
    }
    private void LogTransform(Transform t)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendJoin(", ", t.position.x, t.position.y, t.position.z);
        sb.AppendLine();
        sb.AppendJoin(", ", t.rotation.x, t.rotation.y, t.rotation.z);
        Debug.Log(sb.ToString());
    }

    private void SetPigeonsState(FlockState state)
    {
        foreach(Pigeon pigeon in _pigeons)
        {
            pigeon.SetState(state);
        }
    }
}

public enum FlockState
{
    Idle,
    TakingOff,
    Flying,
    Landing,
}