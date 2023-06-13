using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronGate : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;
    [SerializeField] private float _timeDelay;
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _animator.enabled = false;
    }

    public void Open()
    {
        StartCoroutine(OpenGate());
    }

    private IEnumerator OpenGate()
    {
        yield return new WaitForSeconds(_timeDelay);
        Debug.Log("Opening gate");
        // Will open gate as it is the default state
        _animator.enabled = true;
        _audioSource.Play();
    }
}
