using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System;

public class Pigeon : MonoBehaviour, IPointerClickHandler
{
    private bool _isKeyPigeon = false;
    [SerializeField] private TMP_Text keyPressedText;
    [SerializeField] private float noiseMagnitude = 0.01f; // the amount of vertical noise to add
    [SerializeField] private float noiseFrequency = 1f; // the frequency of the noise
    private float _timeOffset; // a random time offset for each pigeon
    private float _initialHeight;
    private FlockState _currentState = FlockState.Idle;

    private void Start() 
    {
        _timeOffset = UnityEngine.Random.Range(0f, 10f);
        _initialHeight = transform.localPosition.y;
    }
    public void MakeKeyPigeon()
    {
        transform.SetParent(null, true);
        _isKeyPigeon = true;
    }

    private void Update() 
    {
        if(_currentState == FlockState.Flying)
        {
            AddNoiseToHeight();
        }
    }

    public void SetState(FlockState state)
    {
        _currentState = state;
    }

    private void AddNoiseToHeight()
    {
        // Calculate the noise value based on time and position
        float noise = Mathf.PerlinNoise(Time.time * noiseFrequency + _timeOffset, transform.position.y * noiseFrequency) * 2f - 1f;
        noise *= noiseMagnitude;

        // Add the noise to the local position
        transform.localPosition = new Vector3(transform.localPosition.x, _initialHeight + noise, transform.localPosition.z);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Eventueel implementatie aanpassen
        if(_isKeyPigeon)
        {
            Debug.Log("Pigeon clicked");
            keyPressedText.gameObject.SetActive(true);
            PortalManager.Instance.OnPigeonPortalComplete();
            Destroy(this.gameObject);
        }
        else
        {
            // Geluidje maken?
            // Animatietje spelen?
        }
    }
}
