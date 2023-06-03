using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Person : MonoBehaviour
{
    protected NavMeshAgent _agent;
    protected Animator _animator;
    [SerializeField] protected float walkSpeed = 3;
    [SerializeField] protected float runSpeed = 6;
    protected MovementState _currentState;
    protected float _currentVelocity;

    protected virtual void Awake() 
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _currentState = MovementState.Idle;
    }

    protected virtual void Update() {
        if(!_agent.hasPath) _agent.velocity = Vector3.zero; // om gliches te fixen
        _currentVelocity = GetVelocity();
        _animator.SetFloat("speed", _currentVelocity);
        if ((!_agent.hasPath || _agent.remainingDistance <= 1.5f) && _currentState != MovementState.Idle) {
            //Debug.Log("Back to idle");
            _currentState = MovementState.Idle;
        }
        if(_currentVelocity > 0 && _currentVelocity <= walkSpeed)
        {
            _currentState = MovementState.Walking;
        }
        if(_currentVelocity > walkSpeed && _currentVelocity <= runSpeed) 
        {
            _currentState = MovementState.Running;
        }
    }
    public float GetVelocity() 
    {
        return _agent.velocity.magnitude;
    }
    public bool IsIdle()
    {
        return _currentState == MovementState.Idle;
    }
    public bool IsWalking()
    {
        return _currentState == MovementState.Walking;
    }
    public bool IsRunning()
    {
        return _currentState == MovementState.Running;
    }
    public void SetAvoidancePriority(int priority)
    {
        _agent.avoidancePriority = priority;
    }
    protected void SetAgentSpeed(float speed)
    {
        _agent.speed = speed;
    }
}

public enum MovementState
{
    Idle,
    Walking,
    Running
}