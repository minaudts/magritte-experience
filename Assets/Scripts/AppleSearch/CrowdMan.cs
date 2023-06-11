using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CrowdMan : Person
{
    //private float _crowdCircleRadius;
    [SerializeField] InputActionAsset inputActionAsset;
    private InputAction _tapAction;
    private InputAction _position;
    private FaceObject _faceObject;
    const string SPEED = "speed";
    //private bool _isWaiting = false;
    [SerializeField] private CrowdManBehaviour behaviour;
    //[SerializeField, Range(0.0f, 1.0f)] private float chanceToWait = 0.3f;
    //[SerializeField] private float minimalWalkingDistance = 10f;
    [SerializeField] private PatrolPath patrolPath;
    [SerializeField] private bool reversePath;
    [SerializeField] private int startAtPathIndex = 0;
    [SerializeField] private CrowdManAnimationState[] sitAnimations;
    [SerializeField] private CrowdManAnimationState[] talkAnimations;
    [SerializeField] private CrowdManAnimationState turnLeftAnimation;
    [SerializeField] private CrowdManAnimationState turnRightAnimation;
    [SerializeField] private bool canHaveKey;
    private bool _isTalking = false;
    private bool _isTurning = false;
    private bool _hoverOverUI = false;
    private List<Transform> _points;
    private int destPoint = 0;
    protected override void Awake()
    {
        base.Awake();
        _faceObject = GetComponentInChildren<FaceObject>();
        destPoint = startAtPathIndex;
        _tapAction = inputActionAsset.FindActionMap("InGame").FindAction("Tap");
        _position = inputActionAsset.FindActionMap("Ingame").FindAction("Position");
        if (behaviour == CrowdManBehaviour.WalkAround)
        {
            _points = patrolPath.GetPath();
            if (reversePath) _points.Reverse();
            _agent.autoBraking = false;
            GoToNextPoint();
        }
        if (behaviour != CrowdManBehaviour.WalkAround && behaviour != CrowdManBehaviour.Sit)
        {
            _animator.SetFloat(SPEED, 0);
            // Set random idle offset
            float offset = Random.Range(0f, 1f);
            _animator.Play("idle", 0, offset);
            //Debug.Log(offset);
            // lowest priority
            SetAvoidancePriority(1);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        _hoverOverUI = EventSystem.current.IsPointerOverGameObject();
        if (!_isTurning && behaviour == CrowdManBehaviour.WalkAround)
        {
            WalkAroundBehaviour();
        }
        else if (!_isTurning && behaviour == CrowdManBehaviour.Wait)
        {
            WaitBehaviour();
        }
        else if (!_isTurning && behaviour == CrowdManBehaviour.Talk)
        {
            TalkBehaviour();
        }
        else if (!_isTurning && behaviour == CrowdManBehaviour.Sit)
        {
            SitBehaviour();
        }
    }

    void WalkAroundBehaviour()
    {
        if (DidArrive())
        {
            ClearPath();
        }
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            // Choose the next destination point when the agent gets
            // close to the current one.
            GoToNextPoint();
        }
        if (_currentVelocity > 0 && _currentVelocity <= walkSpeed)
        {
            _currentState = MovementState.Walking;
        }
        if (!_agent.hasPath) _agent.velocity = Vector3.zero; // om glitches te fixen
        _animator.SetFloat(SPEED, _agent.velocity.magnitude);
    }

    void WaitBehaviour()
    {

    }

    void TalkBehaviour()
    {
        if (!_isTalking && !_animator.IsInTransition(0))
        {
            // Pick random talk animation
            int talkAnimationIndex = Random.Range(0, talkAnimations.Length);
            _animator.CrossFade(talkAnimations[talkAnimationIndex].ToString(), 0.05f, 0);
            _isTalking = true;
            float stateDuration = _animator.GetCurrentAnimatorStateInfo(0).length;
            float nextTalkDelay = Random.Range(stateDuration + 0.5f, stateDuration + 4.5f);
            StartCoroutine(WaitForSeconds(nextTalkDelay));
        }
    }

    void SitBehaviour()
    {
        // Cycle through available sit animations
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !_animator.IsInTransition(0))
        {
            // Select random idle state
            int sitAnimationIndex = Random.Range(0, sitAnimations.Length);
            // Set a random part of the animation to start from
            float randomOffset = Random.Range(0f, 1f);
            _animator.CrossFade(sitAnimations[sitAnimationIndex].ToString(), 0.05f, 0, randomOffset);
        }
    }

    void GoToNextPoint()
    {
        // Returns if no points have been set up
        if (_points.Count == 0)
            return;

        // Set the agent to go to the currently selected destination.
        _agent.destination = _points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % _points.Count;
    }

    private bool DidArrive()
    {
        bool didArrive = _agent.remainingDistance <= _agent.stoppingDistance;
        return didArrive;
    }
    public bool CanHaveKey()
    {
        return canHaveKey;
    }
    private void ClearPath()
    {
        _agent.ResetPath();
    }
    public void SetFaceObject(GameObject faceObject)
    {
        _faceObject.InstantiateObject(faceObject, behaviour == CrowdManBehaviour.Sit);
    }

    private void OnEnable()
    {
        _tapAction.performed += OnTap;
        _tapAction.Enable();
        _position.Enable();
    }
    private void OnDisable()
    {
        _tapAction.performed -= OnTap;
    }

    private IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _isTalking = false;
    }



    public void OnTap(InputAction.CallbackContext context)
    {
        if(_hoverOverUI)
        {
            return;
        }
        Vector2 screenPos = _position.ReadValue<Vector2>();
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPos), out hit, 100))
        {
            if (hit.collider == GetComponent<Collider>() && !_isTurning && behaviour != CrowdManBehaviour.Sit && behaviour != CrowdManBehaviour.WalkAround)
            {
                StartCoroutine(LookAtCameraAndBack());
            }
        }
    }

    private IEnumerator LookAtCameraAndBack()
    {
        Vector3 directionToCam = Camera.main.transform.position - transform.position;
        Vector3 initialForward = transform.forward;

        Vector3 directionY = Vector3.ProjectOnPlane(directionToCam, Vector3.up).normalized;
        
        _isTurning = true;
        _animator.SetFloat(SPEED, 0);
        Debug.Log(Vector3.Dot(transform.forward, directionY));
        Debug.Log("Turning left");
        yield return StartCoroutine(Turn(directionY, true));
        Debug.Log("Facing camera");
        SetAnimatorToIdle();
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Turning right");
        yield return StartCoroutine(Turn(initialForward, false));
        SetAnimatorToIdle();
        Debug.Log("Done");
        _isTurning = false;
    }

    private IEnumerator Turn(Vector3 targetRot, bool turnLeft)
    {
        float timeOutTimer = 0;
        string animationState = turnLeft ? turnLeftAnimation.ToString() : turnRightAnimation.ToString();
        _animator.CrossFade(animationState, 0.15f, 0);
        while(Vector3.Dot(transform.forward, targetRot) < 0.95f && timeOutTimer <= 4f)
        {
            Debug.Log(timeOutTimer);
            timeOutTimer += Time.deltaTime;
            yield return null;
        }
    }

    private void SetAnimatorToIdle()
    {
        _animator.CrossFade("idle", 0.2f, 0);
    }
}
public enum CrowdManBehaviour
{
    WalkAround,
    Wait,
    Talk,
    Sit,
}

public enum CrowdManAnimationState
{
    sit,
    sit1,
    sit2,
    talking,
    talking1,
    talking2,
    talking3,
    leftTurn,
    leftTurn1,
    rightTurn,
    rightTurn1,
}
