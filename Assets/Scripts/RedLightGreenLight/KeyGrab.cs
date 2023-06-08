using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGrab : MonoBehaviour
{
    public GameObject gate;
    public GameObject collisionGate;
    public GameObject fish;
    private Animator animatorGate;

    private void Start()
    {
        animatorGate = gate.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        animatorGate.enabled = true;
        fish.GetComponent<RedLightEnemy>().keyGrabbed = true;
        gameObject.SetActive(false);
        collisionGate.SetActive(false);
    }
}
