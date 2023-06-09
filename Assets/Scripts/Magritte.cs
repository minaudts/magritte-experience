using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Magritte : Person
{
    public LayerMask ignoreTheseLayers;
    [SerializeField] InputActionAsset inputActionAsset;
    private InputAction _walkAction;
    private InputAction _runAction;
    private InputAction _position;
    private AudioSource _audioSource;
    public AudioClip respawnAudio;
    private Camera _cam;
    public Vector3 _respawnPoint;
    //private float _velocityPreviousFrame = 0f;

    //new
    private float playerSpeed;
    private bool _hoverOverUI = false;

    protected override void Awake()
    {
        base.Awake();
        _cam = Camera.main;
        _walkAction = inputActionAsset.FindActionMap("InGame").FindAction("Tap");
        _runAction = inputActionAsset.FindActionMap("InGame").FindAction("DoubleTap");
        _position = inputActionAsset.FindActionMap("Ingame").FindAction("Position");
        _respawnPoint = transform.position;
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void Update() {
        base.Update();
        _hoverOverUI = EventSystem.current.IsPointerOverGameObject();
    }

    private void OnWalk(InputAction.CallbackContext context)
    {
        // If running, ignore this walk trigger unless there is no double tap.
        // Otherwise player will start walking for a bit when giving a run input while running
        if(_currentState == MovementState.Running)
        {
            // TODO
        }
        _currentState = MovementState.Walking;
        OnMove(false);
        //Debug.Log("walk");
    }
    private void OnRun(InputAction.CallbackContext context)
    {
        _currentState = MovementState.Running;
        OnMove(true);
        //Debug.Log("run");
    }

    private void OnMove(bool doublePress)
    {
        if(_hoverOverUI)
        {
            return;
        }
        Vector2 screenPosition = _position.ReadValue<Vector2>();
        SetAgentSpeed(doublePress ? runSpeed : walkSpeed);
        RaycastHit hit;
        if (Physics.Raycast(_cam.ScreenPointToRay(screenPosition), out hit, 100, ~ignoreTheseLayers))
        {

            if(CheckIfShouldMove(hit.collider))
            {
                _agent.destination = hit.point;
            }
        }
    }
    public bool IsRunning()
    {
        return _currentState == MovementState.Running;
    }

    private void OnEnable() 
    {
        _walkAction.performed += OnWalk;
        _walkAction.Enable();
        _runAction.performed += OnRun;
        _runAction.Enable();
        _position.Enable();
    }
    private void OnDisable() 
    {
        _walkAction.performed -= OnWalk;
        _walkAction.Disable();
        _runAction.performed -= OnRun;
        _runAction.Disable();
        _position.Disable();
    }

    public void Respawn()
    {
        _audioSource.clip = respawnAudio;
        _audioSource.Play();
        _agent.ResetPath();
        // Warp _agent back to initial point
        _agent.Warp(_respawnPoint);
        // Set rotation to face forward
        _agent.updateRotation = false;
        transform.eulerAngles = new Vector3(0, 90, 0);
        _agent.updateRotation = true;
    }

    // If collider is Crowdman, Pigeon, Key or ... return false
    private bool CheckIfShouldMove(Collider collider)
    {
        //return true;
        return !collider.GetComponent<CrowdMan>();
    }
}

