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
    private float _velocityPreviousFrame = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
        _currentState = PlayerStates.Idle;
        _walkAction = inputActionAsset.FindActionMap("InGame").FindAction("Walk");
        _runAction = inputActionAsset.FindActionMap("InGame").FindAction("Run");
    }

    private void Update()
    {
        if (agent.isStopped && _velocityPreviousFrame > 0f) _currentState = PlayerStates.Idle;
        _velocityPreviousFrame = agent.velocity.magnitude;
    }

    private void OnWalk(InputAction.CallbackContext context)
    {
        OnMove(Mouse.current.position.ReadValue(), false);
        Debug.Log("walk");
    }
    private void OnRun(InputAction.CallbackContext context)
    {
        OnMove(Mouse.current.position.ReadValue(), true);
        Debug.Log("run");
    }

    private void OnMove(Vector2 screenPosition, bool doublePress)
    {
        _currentState = doublePress ? PlayerStates.Running : PlayerStates.Walking;
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
}
public enum PlayerStates
{
    Idle,
    Walking,
    Running
}
