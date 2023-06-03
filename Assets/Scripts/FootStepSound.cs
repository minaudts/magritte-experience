using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSound : MonoBehaviour
{
    // 28 frames tussen voetstap bij standaard animatie (dus speed = 3)
    // 28 frames => 
    private AudioSource _audioSource;
    // make generic class for Magritte and men and add it as parameter here!
    private Person _person;
    [SerializeField] private AudioClip[] footStepClips;
    private float _stepTimer;
    // TEST: elke seconde een voetstap
    private float _stepInterval = 28f/60f;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _person = GetComponent<Person>();
        // So a sound is made immediately when person starts walking
        _stepTimer = _stepInterval;
    }

    void Update()
    {
        if(/*_person is Magritte && */!_person.IsIdle())
        {
            //PlayFootStepSound();
        }
    }
    // Functie wordt opgeroepen door animation events
    private void PlayFootStepSound()
    {
        /*if(_stepTimer >= _stepInterval) {
            // Pick random footstep sound and play it.
            int randomSoundIndex = Random.Range(0, footStepClips.Length);
            _audioSource.clip = footStepClips[randomSoundIndex];
            _audioSource.Play();
            _stepTimer = 0;
            Debug.Log("Playing sound for " + gameObject.name);
        }
        _stepTimer += Time.deltaTime;*/
        int randomSoundIndex = Random.Range(0, footStepClips.Length);
        _audioSource.clip = footStepClips[randomSoundIndex];
        _audioSource.Play();
    }
}
