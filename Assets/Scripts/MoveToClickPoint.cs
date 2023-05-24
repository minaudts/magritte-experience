using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MoveToClickPoint : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActionAsset;
    private InputAction _walkAction;
    private InputAction _runAction;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private NavMeshAgent agent;
    private Camera cam;
    private InputAction pressAction;
    private PlayerStates _currentState;
    private Vector3 _respawnPoint;
    //private float _velocityPreviousFrame = 0f;

    //new
    private float playerSpeed;
    private Animator animator;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();        
        cam = Camera.main;
        _currentState = PlayerStates.Idle;
        _walkAction = inputActionAsset.FindActionMap("InGame").FindAction("Walk");
        _runAction = inputActionAsset.FindActionMap("InGame").FindAction("Run");
        _respawnPoint = transform.position;

        //new
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && _currentState != PlayerStates.Idle) {
            //Debug.Log("Back to idle");
            _currentState = PlayerStates.Idle;
        }
        if(!agent.hasPath) agent.velocity = Vector3.zero; // om gliches te fixen

        //new
        playerSpeed = agent.velocity.magnitude;
        animator.SetFloat("speed", playerSpeed);
        //Debug.Log(playerSpeed);
    }

    private void OnWalk(InputAction.CallbackContext context)
    {
        _currentState = PlayerStates.Walking;
        OnMove(Mouse.current.position.ReadValue(), false);
        //Debug.Log("walk");
    }
    private void OnRun(InputAction.CallbackContext context)
    {
        _currentState = PlayerStates.Running;
        OnMove(Mouse.current.position.ReadValue(), true);
        //Debug.Log("run");
    }

    private void OnMove(Vector2 screenPosition, bool doublePress)
    {
        agent.speed = doublePress ? runSpeed : walkSpeed;
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(screenPosition), out hit, 100))
        {
            agent.destination = hit.point;
        }
    }

    private void OnEnable() 
    {
        _walkAction.performed += OnWalk;
        _walkAction.Enable();
        _runAction.performed += OnRun;
        _runAction.Enable();
    }
    private void OnDisable() 
    {
        _walkAction.performed -= OnWalk;
        _walkAction.Disable();
        _runAction.performed -= OnRun;
        _runAction.Disable();
    }

    public bool IsIdle()
    {
        return _currentState == PlayerStates.Idle;
    }
    public bool IsWalking()
    {
        return _currentState == PlayerStates.Walking;
    }
    public bool IsRunning()
    {
        return _currentState == PlayerStates.Running;
    }
    public void Respawn()
    {
        agent.ResetPath();
        // Warp agent back to initial point
        agent.Warp(_respawnPoint);
        // Set rotation to face forward
        agent.updateRotation = false;
        transform.eulerAngles = new Vector3(0, 90, 0);
        agent.updateRotation = true;
    }
}
public enum PlayerStates
{
    Idle,
    Walking,
    Running
}
