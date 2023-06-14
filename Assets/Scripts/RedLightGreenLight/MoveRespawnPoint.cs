using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRespawnPoint : MonoBehaviour
{
    public GameObject magritte;
    public GameObject respawnBall;
    private bool isFirstEnter = true;
    private AudioSource audioSource;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        magritte.GetComponent<Magritte>()._respawnPoint = transform.position;
        respawnBall.transform.position = transform.position;
        if(isFirstEnter && audioSource)
        {
            audioSource.Play();
            isFirstEnter = false;
        }
    }
}
