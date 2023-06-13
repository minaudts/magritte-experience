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
    public AudioClip pickupKey;
    bool pickedUp = false;

    private void Start()
    {
        animatorGate = gate.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(KeyGrabbed());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(KeyGrabbed());
    }
    
    private IEnumerator KeyGrabbed()
    {
        if (!pickedUp)
        {
            pickedUp = true;
            animatorGate.enabled = true;
            _audioSource.PlayOneShot(pickupKey, 1f);
            airBridge.Appear();
            fish.GetComponent<RedLightEnemy>().keyGrabbed = true;
            collisionGate.SetActive(false);
            keyParticles.SetActive(false);
            gateLights.SetActive(true);

            yield return new WaitForSeconds(.5f);
            gameObject.SetActive(false);
        }
        
    }
}
