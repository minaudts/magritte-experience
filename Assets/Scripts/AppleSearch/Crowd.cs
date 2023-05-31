using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crowd : MonoBehaviour
{
    [SerializeField] private GameObject crowdManPrefab;
    [SerializeField] private int crowdSize = 20;
    [SerializeField] private float crowdRadius = 12.5f;
    [SerializeField] private GameObject[] possibleFaceObjects;
    // Start is called before the first frame update
    void Start()
    {
        SpawnCrowd();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnCrowd()
    {
        for (int i = 0; i < crowdSize; i++)
        {
            Vector3 randomDirection = Random.insideUnitCircle * crowdRadius;
            randomDirection += transform.position;
            Vector3 finalPosition = randomDirection;
            Debug.Log(randomDirection.ToString());
            // Zorgen dat man niet naast navMesh spawnt.
            NavMeshHit hit;
            if(NavMesh.SamplePosition(randomDirection, out hit, crowdRadius, 1)) 
            {
                finalPosition = hit.position;
            }
            Debug.Log(finalPosition.ToString());

            GameObject res = Instantiate(crowdManPrefab, new Vector3(finalPosition.x, 0, finalPosition.z), Quaternion.identity, transform);
            CrowdMan crowdMan = res.GetComponent<CrowdMan>();
            crowdMan.SetCircleRadius(crowdRadius);
            crowdMan.SetAvoidancePriority(Random.Range(40, 60));
            crowdMan.SetFaceObject(possibleFaceObjects[Random.Range(0, possibleFaceObjects.Length)]);
        }
    }
}
