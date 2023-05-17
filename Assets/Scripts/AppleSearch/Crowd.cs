using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        for(int i = 0; i < crowdSize; i++)
        {
            Vector2 unitCircle = Random.insideUnitCircle * crowdRadius;
            GameObject res = Instantiate(crowdManPrefab, new Vector3(unitCircle.x, 0, unitCircle.y), Quaternion.identity, transform);
            CrowdMan crowdMan = res.GetComponent<CrowdMan>();
            crowdMan.SetCircleRadius(crowdRadius);
            crowdMan.SetAvoidancePriority(Random.Range(40, 60));
            crowdMan.SetFaceObject(possibleFaceObjects[Random.Range(0, possibleFaceObjects.Length)]);
        }
    }
}
