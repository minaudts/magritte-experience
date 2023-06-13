using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGrab : MonoBehaviour
{
    public GameObject gate;
    public GameObject collisionGate;
    public GameObject fish;
    public GameObject keyParticles;
    public GameObject gateLights;
    public AirBridge airBridge;
    private Animator animatorGate;
    private AudioSource _audioSource;

    private void Start()
    {
        animatorGate = gate.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            KeyGrabbed();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        KeyGrabbed();
    }

    private void KeyGrabbed()
    {
        animatorGate.enabled = true;
        _audioSource.Play();
        airBridge.Appear();
        fish.GetComponent<RedLightEnemy>().keyGrabbed = true;
        gameObject.SetActive(false);
        collisionGate.SetActive(false);
        keyParticles.SetActive(false);
        gateLights.SetActive(true);
    }
}
