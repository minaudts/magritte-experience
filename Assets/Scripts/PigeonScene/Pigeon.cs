using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Pigeon : MonoBehaviour
{  
    [SerializeField] private float noiseMagnitude = 0.01f; // the amount of vertical noise to add
    [SerializeField] private float noiseFrequency = 1f; // the frequency of the noise
    private float _timeOffset; // a random time offset for each pigeon
    private float _initialHeight;
    private FlockState _currentState = FlockState.Idle;
    private string _idleToTakeOff = "IdleToTakeOff";
    private string _flyToLand = "FlyToLand";
    [SerializeField] private PigeonIdleState[] idleStates;
    private int currentIdleStateIndex;
    private Animator _animator;

    private void Start()
    {
        _timeOffset = UnityEngine.Random.Range(0f, 10f);
        _initialHeight = transform.localPosition.y;
        _animator = GetComponent<Animator>();
        _animator.SetFloat("Offset", UnityEngine.Random.Range(0f, 1f));
    }

    private void Update()
    {
        if (_currentState == FlockState.Flying)
        {
            AddNoiseToHeight();
        }
        if (_currentState == FlockState.Idle && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !_animator.IsInTransition(0))
        {
            // Select random idle state
            currentIdleStateIndex = Random.Range(0, idleStates.Length);
            // Set a random part of the animation to start from
            float randomOffset = Random.Range(0f, 1f);
            _animator.CrossFade(idleStates[currentIdleStateIndex].ToString(), 0.05f, 0, randomOffset);
        }
    }

    public void SetAnimationSpeed(float speed)
    {
        _animator.speed = speed;
    }

    public void SetState(FlockState state)
    {
        _currentState = state;
        if (state == FlockState.TakingOff)
        {
            _animator.SetTrigger(_idleToTakeOff);
        }
        else if (state == FlockState.Landing)
        {
            _animator.SetTrigger(_flyToLand);
        }
    }

    private void AddNoiseToHeight()
    {
        // Calculate the noise value based on time and position, remap from [0,1] to [-1,1]
        float noise = Mathf.PerlinNoise(Time.time * noiseFrequency + _timeOffset, transform.position.y * noiseFrequency) * 2f - 1f;
        noise *= noiseMagnitude;

        // Add the noise to the local position
        transform.localPosition = new Vector3(transform.localPosition.x, _initialHeight + noise, transform.localPosition.z);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Geluidje maken?
        // Animatietje spelen?
    }
}

public enum PigeonIdleState
{
    Idle0,
    Idle1,
    Idle2
}