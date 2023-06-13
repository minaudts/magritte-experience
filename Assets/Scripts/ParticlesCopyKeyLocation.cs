using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesCopyKeyLocation : MonoBehaviour
{
    private GameObject key;
    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        key = GameObject.FindGameObjectWithTag("Key");
        transform.position = key.transform.position;
        //Debug.Log(key.transform.position);
    }
}
