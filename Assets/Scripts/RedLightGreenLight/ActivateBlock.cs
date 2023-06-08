using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class ActivateBlock : MonoBehaviour
{
    public GameObject fingers;
    public bool enteredFirstTime;
    
    private void Start()
    {
        enteredFirstTime = true;
    }
    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (enteredFirstTime)
        {
            fingers.SetActive(true);
            enteredFirstTime = false;
        }
    }
}
