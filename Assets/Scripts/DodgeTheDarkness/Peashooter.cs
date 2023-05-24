using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Peashooter : MonoBehaviour
{
    [SerializeField] private Transform[] points; // Patrol points
    private int destPoint = 0; // Current destination point
    private NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        _agent.autoBraking = false;

        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        _agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }

    // Called by child object when player enters its collider.
    public void OnTriggerEnterWrapper(Collider other)
    {
        Debug.Log("Collider of peashooter " + gameObject.name + " activated");
    }
    public void OnTriggerExitWrapper(Collider other)
    {
        Debug.Log("Collider of peashooter " + gameObject.name + " deactivated");
    }
}
