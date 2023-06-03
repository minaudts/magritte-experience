using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdMan : Person
{
    //private float _crowdCircleRadius;
    private FaceObject _faceObject;
    //private bool _isWaiting = false;
    [SerializeField] private CrowdManBehaviour behaviour;
    //[SerializeField, Range(0.0f, 1.0f)] private float chanceToWait = 0.3f;
    //[SerializeField] private float minimalWalkingDistance = 10f;
    [SerializeField] private PatrolPath patrolPath;
    [SerializeField] private bool reversePath;
    [SerializeField] private int startAtPathIndex = 0;
    private List<Transform> _points;
    private int destPoint = 0;
    protected override void Awake()
    {
        base.Awake();
        _faceObject = GetComponentInChildren<FaceObject>();
        destPoint = startAtPathIndex;
        if (behaviour == CrowdManBehaviour.WalkAround)
        {
            _points = patrolPath.GetPath();
            if (reversePath) _points.Reverse();
            _agent.autoBraking = false;
            GoToNextPoint();
        }
        if (behaviour == CrowdManBehaviour.Wait || behaviour == CrowdManBehaviour.Talk)
        {
            _animator.SetFloat("speed", 0);
            // Set random idle offset
            float offset = Random.Range(0f, 1f);
            _animator.Play("idle", 0, offset);
            Debug.Log(offset);
            // lowest priority
            SetAvoidancePriority(1);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (behaviour == CrowdManBehaviour.WalkAround)
        {
            WalkAroundBehaviour();
        }
        else if (behaviour == CrowdManBehaviour.Wait)
        {
            WaitBehaviour();
        }
        else if (behaviour == CrowdManBehaviour.Talk)
        {
            TalkBehaviour();
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
        if (!_agent.hasPath) _agent.velocity = Vector3.zero; // om glitches te fixen
        _animator.SetFloat("speed", _agent.velocity.magnitude);
    }

    void WaitBehaviour()
    {

    }

    void TalkBehaviour()
    {

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
    private void ClearPath()
    {
        _agent.ResetPath();

    }
    public void SetFaceObject(GameObject faceObject, Material mat)
    {
        _faceObject.InstantiateObject(faceObject, mat);
    }
    public void SetBehaviour(CrowdManBehaviour behaviour)
    {
        this.behaviour = behaviour;
    }
}

public enum CrowdManBehaviour
{
    WalkAround,
    Wait,
    Talk,
}
