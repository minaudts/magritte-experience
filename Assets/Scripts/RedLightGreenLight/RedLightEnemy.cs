using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLightEnemy : MonoBehaviour
{
    [SerializeField] private Magritte magritte; // Reference to player
    [Header("Rotation Settings")]
    [SerializeField] private float rotationDuration = 2f; // Time required to rotate
    [SerializeField] private float minTurnDelay = 2f; // Minimum delay before enemy turns around
    [SerializeField] private float maxTurnDelay = 5f; // Maximum delay before enemy turns around
    [SerializeField] private float minLookDuration = 1f; // Minimum duration for enemy to look at player
    [SerializeField] private float maxLookDuration = 3f; // Maximum duration for enemy to look at player
    private float orangeLightDuration = 1.383f; // Orange light duration
    //[SerializeField] private Material orangeLightMaterial; // Quick visualizer when enemy is about to turn
    //private Material _originalMaterial;
    //private MeshRenderer _mesh;
    private float _angleBetweenEnemyAndPlayer;
    private bool _isLooking = false;
    private bool _canStartNewRotation = true;
    private Animator _animator;
    private void Start() 
    {
        //_mesh = GetComponent<MeshRenderer>();
        _animator = GetComponent<Animator>();
        //_originalMaterial = _mesh.material;
    }

    // Update is called once per frame
    void Update()
    {
        // -90f om angle the normaliseren
        //_angleBetweenEnemyAndPlayer = Vector3.SignedAngle(transform.forward, magritte.transform.position, Vector3.up) - 90f;
        //Debug.Log(_angleBetweenEnemyAndPlayer);
        if(_isLooking && !magritte.IsIdle()) {
            Debug.Log("Player in sight and moving!");
            Respawn();
        }
        if(_canStartNewRotation)
        {
            StartCoroutine(RedLightGreenLight());
        }
    }

    private IEnumerator RotateForDegrees(float degrees, bool turnLeft = true) {
        _animator.SetBool("IsSwimming", true);
        float targetAngle = transform.eulerAngles.y + (turnLeft ? -degrees : degrees);
        float startAngle = transform.eulerAngles.y;
        float time = 0f;
        while(time < rotationDuration) {
            float currentAngle = Mathf.Lerp(startAngle, targetAngle, time / rotationDuration);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentAngle, transform.eulerAngles.z);
            time += Time.deltaTime;
            yield return null;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
        _animator.SetBool("IsSwimming", false);
    }

    private IEnumerator RedLightGreenLight()
    {
        _canStartNewRotation = false;
        Debug.Log("Can not start new rotation");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(OrangeLight());
        yield return StartCoroutine(RedLight());
        // Look at player for random time
        float lookAtPlayerFor = Random.Range(minLookDuration, maxLookDuration);
        Debug.Log("Looking at player for " + lookAtPlayerFor.ToString("F2") + "s");
        yield return new WaitForSeconds(lookAtPlayerFor);
        yield return StartCoroutine(GreenLight());
        // Wait before next turnaround
        float waitForNextRedLight = Random.Range(minTurnDelay, maxTurnDelay);
        Debug.Log("Turn around again in " + waitForNextRedLight.ToString("F2") + "s");
        yield return new WaitForSeconds(waitForNextRedLight);
        Debug.Log("Can start a new rotation");
        _canStartNewRotation = true;
    }

    private IEnumerator OrangeLight()
    {
        
        //_mesh.material = orangeLightMaterial;
        _animator.SetTrigger("Warning");
        yield return new WaitForSeconds(orangeLightDuration);
        //_mesh.material = _originalMaterial;
    }

    private IEnumerator RedLight() 
    {
        yield return StartCoroutine(RotateForDegrees(180f, true));
        Debug.Log("Started looking");
        _isLooking = true;
    }
    private IEnumerator GreenLight()
    {
        _isLooking = false;
        Debug.Log("Not looking anymore");
        yield return StartCoroutine(RotateForDegrees(180f, false));
    }
    private void Respawn()
    {
        magritte.Respawn();
    }
}
