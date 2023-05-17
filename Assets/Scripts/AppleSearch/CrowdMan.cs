using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdMan : MonoBehaviour
{
    private NavMeshAgent _agent;
    private LineRenderer _line;
    private Vector3 _destination;
    [SerializeField] float crowdCircleRadius = 12.5f;
    // Start is called before the first frame update
    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _agent = GetComponent<NavMeshAgent>();
        StartCoroutine(GetPath());
    }

    // Update is called once per frame
    void Update()
    {
        if (!_agent.hasPath)
        {
            StartCoroutine(GetPath());
        }
    }

    private IEnumerator GetPath()
    {
        _line.SetPosition(0, transform.position); //set the line's origin
        Vector2 unitCircle = Random.insideUnitCircle * crowdCircleRadius;
        Debug.Log(unitCircle.x + ", " + unitCircle.y);
        _agent.SetDestination(new Vector3(unitCircle.x, 0, unitCircle.y)); //create the path
        yield return new WaitForEndOfFrame(); //wait for the path to generate

        DrawPath(_agent.path);

        //agent.Stop();//add this if you don't want to move the agent
    }

    private void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2) //if the path has 1 or no corners, there is no need
            return;

        _line.positionCount = path.corners.Length; //set the array of positions to the amount of corners

        for (int i = 1; i < path.corners.Length; i++)
        {
            _line.SetPosition(i, path.corners[i]); //go through each corner and set that to the line renderer's position
        }
    }
}
