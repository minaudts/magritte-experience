using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MoveToClickPoint : MonoBehaviour
{
    //[SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private NavMeshAgent agent;
    private Camera cam;
    //private InputAction touchPositionAction;
    //private InputAction touchPressAction;
    private Mouse _mouse;
    private Touchscreen _touchscreen;
    private PlayerStates _currentState;
    private float _velocityPreviousFrame = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //touchPositionAction = inputActionAsset.FindActionMap("InGame").FindAction("TouchPosition");
        //touchPressAction = inputActionAsset.FindActionMap("InGame").FindAction("TouchPress");
        cam = Camera.main;
        _mouse = Mouse.current;
        _touchscreen = Touchscreen.current;
        _currentState = PlayerStates.Idle;
    }

    private void Update()
    {
        // check for mouse input
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnTouch(Mouse.current.position.ReadValue(), Mouse.current.clickCount.ReadValue() != 1);
        }
        // check for touchscreen input
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.tap.isPressed)
        {
            OnTouch(Touchscreen.current.position.ReadValue(), Touchscreen.current.primaryTouch.tapCount.ReadValue() != 1);
        }
        if (agent.isStopped && _velocityPreviousFrame > 0f) _currentState = PlayerStates.Idle;
        _velocityPreviousFrame = agent.velocity.magnitude;
    }

    private void OnTouch(Vector2 screenPosition, bool doublePress)
    {
        //Debug.Log("Pressed position " + screenPosition.x + ", " + screenPosition.y);
        _currentState = doublePress ? PlayerStates.Running : PlayerStates.Walking;
        agent.speed = doublePress ? runSpeed : walkSpeed;
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(screenPosition), out hit, 100))
        {
            agent.destination = hit.point;
        }
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
