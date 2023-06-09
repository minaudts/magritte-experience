using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.EventSystems;

public class Key : MonoBehaviour
{
    [SerializeField] private IronGate gate;
    [SerializeField] private AirBridge bridge;
    [SerializeField] InputActionAsset inputActionAsset;
    private PlayableDirector _keyCollectedTimeline;
    private AudioSource _audioSource;
    private InputAction _tapAction;
    private InputAction _position;
    private Rigidbody _rb;
    private SphereCollider _collider;
    private MeshRenderer _mesh;
    private Outline _outline;
    private bool _isCollectable = false;
    private bool _hoverOverUI = false;

    public GameObject gateLights;
    public GameObject keyParticles;

    private GameObject magritte;
    float distanceToPlayer;
    public float minDistance = 3f;
    private void Awake()
    {
        _tapAction = inputActionAsset.FindActionMap("InGame").FindAction("Tap");
        _position = inputActionAsset.FindActionMap("Ingame").FindAction("Position");
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _keyCollectedTimeline = GameObject.FindObjectOfType<PlayableDirector>();
        _collider = GetComponent<SphereCollider>();
        _mesh = GetComponentInChildren<MeshRenderer>();
        _outline = GetComponentInChildren<Outline>();
        if (!gate) gate = GameObject.FindObjectOfType<IronGate>();
        if (!bridge) bridge = GameObject.FindObjectOfType<AirBridge>();

        gateLights = GameObject.Find("GateLights");
        gateLights.SetActive(false);
        keyParticles = GameObject.Find("KeyParticles");

        magritte = GameObject.Find("Magritte");
    }
    private void Update()
    {
        _hoverOverUI = EventSystem.current.IsPointerOverGameObject();
        if (Input.GetKeyDown(KeyCode.K))
        {
            OnKeyCollected();
        }
        distanceToPlayer = Vector3.Distance(transform.position, magritte.transform.position);
    }
    public void MakeCollectable(bool shouldDrop)
    {
        _isCollectable = true;
        if (shouldDrop) OnDrop();
    }

    public void MakeUncollectable()
    {
        _isCollectable = false;
    }
    private void OnDrop()
    {
        transform.SetParent(null, true);
        _rb.isKinematic = false;
        _rb.AddForce(new Vector3(0, 100, 0));
        StartCoroutine(EnlargeKeyToScale(3.5f));
    }

    private IEnumerator EnlargeKeyToScale(float scale)
    {
        Vector3 targetScale = new Vector3(scale, scale, scale);
        Vector3 initialScale = transform.localScale;
        // will scale all dimensions equally
        while (transform.localScale.magnitude < targetScale.magnitude)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, 1);
            yield return null;
        }
    }

    public void OnTap(InputAction.CallbackContext context)
    {
        if(_hoverOverUI)
        {
            return;
        }
        Vector2 screenPos = _position.ReadValue<Vector2>();
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPos), out hit, 100))
        {
            if (hit.collider == GetComponent<Collider>() && _isCollectable)
            {
                if(distanceToPlayer < minDistance)
                {
                    OnKeyCollected();
                }               
            }
        }
    }

    private void OnKeyCollected()
    {
        gateLights.SetActive(true);
        Destroy(keyParticles.gameObject);
        _audioSource.Play();
        _mesh.enabled = false;
        _collider.enabled = false;
        _outline.enabled = false;
        Debug.Log("Key collected");
        if (_keyCollectedTimeline) _keyCollectedTimeline.Play();
        gate.Open();
        bridge.Appear();
        //gameObject.SetActive(false);        
    }

    private void OnEnable()
    {
        _tapAction.performed += OnTap;
        _tapAction.Enable();
        _position.Enable();
    }
    private void OnDisable()
    {
        _tapAction.performed -= OnTap;
    }
}
