using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLightEnemy : MonoBehaviour
{
    [SerializeField] private MoveToClickPoint magritte; // Reference to player
    [SerializeField] private float lineOfSightAngle = 45f; // If player is in this angle and moving, he is busted
    [Header("Rotation Settings")]
    [SerializeField] private float rotationDuration = 2f; // Time required to rotate
    [SerializeField] private float minTurnDelay = 2f; // Minimum delay before enemy turns around
    [SerializeField] private float maxTurnDelay = 5f; // Maximum delay before enemy turns around
    [SerializeField] private float minLookDuration = 1f; // Minimum duration for enemy to look at player
    [SerializeField] private float maxLookDuration = 3f; // Maximum duration for enemy to look at player
    [SerializeField] private float orangeLightDuration = 0.5f; // Orange light duration
    [SerializeField] private Material orangeLightMaterial; // Quick visualizer when enemy is about to turn
    private Material _originalMaterial;
    private MeshRenderer _mesh;
    private float _angleBetweenEnemyAndPlayer;
    private bool _isLooking = false;
    private void Start() 
    {
        _mesh = GetComponent<MeshRenderer>();
        _originalMaterial = _mesh.material;
    }

    // Update is called once per frame
    void Update()
    {
        // -90f om angle the normaliseren
        _angleBetweenEnemyAndPlayer = Vector3.SignedAngle(transform.forward, magritte.transform.position, Vector3.up) - 90f;
        Debug.Log(_angleBetweenEnemyAndPlayer);
        if(Mathf.Abs(_angleBetweenEnemyAndPlayer) < lineOfSightAngle && !magritte.IsIdle()) {
            Debug.Log("Player in sight and moving!");
            Respawn();
        }
        if(!_isLooking)
        {
            StartCoroutine(RedLightGreenLight());
        }
    }

    private IEnumerator RotateForDegrees(float degrees, bool turnLeft = true) {
        float targetAngle = transform.eulerAngles.y + (turnLeft ? -degrees : degrees);
        float startAngle = transform.eulerAngles.y;
        float time = 0f;
        while(time < rotationDuration) {
            float currentAngle = Mathf.LerpAngle(startAngle, targetAngle, time / rotationDuration);
            if (turnLeft) transform.eulerAngles = new Vector3(transform.eulerAngles.x, -currentAngle, transform.eulerAngles.z);
            else transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentAngle, transform.eulerAngles.z);
            time += Time.deltaTime;
            yield return null;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
    }

    private IEnumerator RedLightGreenLight()
    {
        _isLooking = true;
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
        _isLooking = false;
    }

    private IEnumerator OrangeLight()
    {
        _mesh.material = orangeLightMaterial;
        yield return new WaitForSeconds(orangeLightDuration);
        _mesh.material = _originalMaterial;
    }

    private IEnumerator RedLight() 
    {
        yield return StartCoroutine(RotateForDegrees(180f, true));
    }
    private IEnumerator GreenLight()
    {
        yield return StartCoroutine(RotateForDegrees(180f, false));
    }
    private void Respawn()
    {
        magritte.Respawn();
    }
}
