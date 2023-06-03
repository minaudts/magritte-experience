using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crowd : MonoBehaviour
{
    //[SerializeField] private GameObject crowdManPrefab;
    //[SerializeField] private int crowdSize = 20;
    //[SerializeField] private float crowdRadius = 12.5f;
    private CrowdMan[] _crowd;
    [SerializeField] private GameObject[] possibleFaceObjects;
    [SerializeField] private Material[] possibleMaterials;
    [SerializeField] private Key keyFaceObject;
    [SerializeField] private Material keyMaterial;
    // Start is called before the first frame update
    void Start()
    {
        // Get ref to all men in the scene
        _crowd = GetComponentsInChildren<CrowdMan>();
        // Assign random object to some of them, and to 1, assign the green apple.
        AssignFaceObjects();
        //SpawnCrowd();
    }

    void AssignFaceObjects()
    {
        // Choose random man with key object
        int keyManIndex = GetRandomIndexInArray(_crowd);
        for(int i = 0; i < _crowd.Length; i++)
        {
            CrowdMan man = _crowd[i];
            if(i == keyManIndex)
            {
                man.SetFaceObject(keyFaceObject.gameObject, keyMaterial);
            }
            else
            {
                // Choose random object and color (or no object?)
                int faceObjectIndex = GetRandomIndexInArray(possibleFaceObjects);
                int materialIndex = GetRandomIndexInArray(possibleMaterials);
                man.SetFaceObject(possibleFaceObjects[faceObjectIndex].gameObject, possibleMaterials[materialIndex]);
            }
        }
    }

    int GetRandomIndexInArray<T>(T[] array)
    {
        return Random.Range(0, array.Length);
    }

    /*private void SpawnCrowd()
    {
        for (int i = 0; i < crowdSize; i++)
        {
            Vector3 randomDirection = Random.insideUnitCircle * crowdRadius;
            randomDirection += transform.position;
            Vector3 finalPosition = randomDirection;
            //Debug.Log(randomDirection.ToString());
            // Zorgen dat man niet naast navMesh spawnt.
            NavMeshHit hit;
            if(NavMesh.SamplePosition(randomDirection, out hit, crowdRadius, 1)) 
            {
                finalPosition = hit.position;
            }
            //Debug.Log(finalPosition.ToString());

            GameObject res = Instantiate(crowdManPrefab, new Vector3(finalPosition.x, 0, finalPosition.z), Quaternion.identity, transform);
            CrowdMan crowdMan = res.GetComponent<CrowdMan>();
            //crowdMan.SetCircleRadius(crowdRadius);
            crowdMan.SetAvoidancePriority(Random.Range(40, 60));
            crowdMan.SetFaceObject(possibleFaceObjects[Random.Range(0, possibleFaceObjects.Length)]);
            crowdMan.SetBehaviour(CrowdManBehaviour.WalkAround);
        }
    }*/
}
