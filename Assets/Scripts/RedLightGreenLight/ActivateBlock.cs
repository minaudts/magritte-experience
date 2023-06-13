using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;


public class ActivateBlock : MonoBehaviour
{
    public GameObject fingers;
    public GameObject magritte;
    public AudioClip raiseFingers;
    private AudioSource audioSource;
    public bool enteredFirstTime;
    
    private void Start()
    {
        enteredFirstTime = true;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (enteredFirstTime)
        {
            audioSource.PlayOneShot(raiseFingers, .6f);
            fingers.SetActive(true);
            enteredFirstTime = false;
            magritte.GetComponent<NavMeshAgent>().SetDestination(magritte.transform.position);
        }
    }
}
