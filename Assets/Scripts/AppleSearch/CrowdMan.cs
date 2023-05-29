using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdMan : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;
    private float _crowdCircleRadius;
    private FaceObject _faceObject;
    private bool _isWaiting = false;
    [SerializeField, Range(0.0f, 1.0f)] private float chance = 0.3f;
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _faceObject = GetComponentInChildren<FaceObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DidArrive())
        {
            ClearPath();
        }
        if (!_isWaiting && !_agent.hasPath)
        {
            // Decide if want to set new destination or wait for a few seconds.
            DecideNextAction();
        }
        if(!_agent.hasPath) _agent.velocity = Vector3.zero; // om gliches te fixen

        //new
        //playerSpeed = _agent.velocity.magnitude;
        _animator.SetFloat("speed", _agent.velocity.magnitude);
        //Debug.Log(playerSpeed);
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

    private void DecideNextAction()
    {
        bool startWaiting = Random.Range(0f, 1f) < chance;
        Debug.Log(startWaiting);
        if(startWaiting)
        {
            // Wait few seconds
            StartCoroutine(StopAndWait());
        }
        else
        {
            // Pick new path
            SetPath();
        }
        
    }

    private IEnumerator StopAndWait()
    {
        _agent.isStopped = true;
        float waitTime = Random.Range(3f, 5f);
        _isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        _isWaiting = false;
    }

    public void SetCircleRadius(float radius)
    {
        _crowdCircleRadius = radius;
    }

    public void SetAvoidancePriority(int priority)
    {
        _agent.avoidancePriority = priority;
    }
    public void SetFaceObject(GameObject faceObject)
    {
        _faceObject.InstantiateObject(faceObject);
    }

    private void SetPath()
    {
        // Kan eventueel ook gwn met random punt op navMesh
        Vector2 unitCircle = new Vector2(transform.position.x, transform.position.z) + Random.insideUnitCircle * _crowdCircleRadius;
        _agent.SetDestination(new Vector3(unitCircle.x, 0, unitCircle.y)); //create the path
    }
}
