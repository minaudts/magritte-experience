using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] audioClips;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Functie wordt opgeroepen door animation events
    private void PlayRandomSound()
    {
        int randomSoundIndex = Random.Range(0, audioClips.Length);
        _audioSource.clip = audioClips[randomSoundIndex];
        _audioSource.Play();
    }
}
