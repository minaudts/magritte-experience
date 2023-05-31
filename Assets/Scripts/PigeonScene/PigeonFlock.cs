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
    private KeyPigeon _keyPigeon;
    private Magritte _player;
    // Start is called before the first frame update
    void Start()
    {
        _pigeons = GetComponentsInChildren<Pigeon>();
        _keyPigeon = GetComponentInChildren<KeyPigeon>();
        _player = GameObject.FindObjectOfType<Magritte>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Magritte>())
        {
            Debug.Log(other.name + " entered pigeon flock");
            // If player is running, pick random key pigeon
            if (_player.IsRunning())
            {
                _keyPigeon.DropKey();
            }
            // Always fly to next destination in a loop
            Debug.Log("Flying to next destination");
            // Destination is endpoint of the flight
            currentDestination = flyDestinations[destinationIndex];
            StartCoroutine(FlyToDestination(currentDestination.position));
        }
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
        // Make pigeons play take off animation
        SetPigeonsState(FlockState.TakingOff);
        // wait until animation is complete, then start ascending, time is animation #frames / fps / animationSpeed
        SetPigeonsAnimationSpeed(2f);
        yield return new WaitForSeconds(49f / 60f / 2f);
        SetPigeonsState(FlockState.Flying);
        SetPigeonsAnimationSpeed(1.2f);
        while (Mathf.Abs(transform.position.y - flyingHeight) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, liftOffSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator FlyAtConstantHeight(Vector3 target)
    {
        SetPigeonsAnimationSpeed(0.6f);
        while (Vector3.Distance(transform.position, target) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, flyingSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator Land(Vector3 target)
    {
        SetPigeonsAnimationSpeed(0.4f);
        // below this float the land animation will take over 
        float startLandAnimationHeight = 0.04f;
        while (Vector3.Distance(transform.position, target) > startLandAnimationHeight)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, descendSpeed * Time.deltaTime);
            yield return null;
        }
        SetPigeonsState(FlockState.Landing);
        SetPigeonsAnimationSpeed(1f);
        // duration of land animation
        yield return new WaitForSeconds(0.75f);
        // Land at the target position
        transform.position = target;
        SetPigeonsState(FlockState.Idle);
    }

    private void SetPigeonsState(FlockState state)
    {
        foreach (Pigeon pigeon in _pigeons)
        {
            pigeon.SetState(state);
        }
    }
    private void SetPigeonsAnimationSpeed(float speed)
    {
        foreach (Pigeon pigeon in _pigeons)
        {
            if (pigeon) pigeon.SetAnimationSpeed(speed);
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