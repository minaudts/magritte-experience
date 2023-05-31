using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Key : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private IronGate gate;
    [SerializeField] private AirBridge bridge;
    private Rigidbody _rb;
    private bool _isCollectable = false;
    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }
    public void MakeCollectable(bool shouldDrop)
    {
        _isCollectable = true;
        if(shouldDrop) OnDrop();
    }

    private void OnDrop()
    {
        transform.SetParent(null, true);
        _rb.isKinematic = false;
        _rb.AddForce(new Vector3(0, 100, 0));
        StartCoroutine(EnlargeKeyToScale(3.5f));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isCollectable)
        {
            Debug.Log("Key collected");
            gate.Open();
            bridge.Appear();
            Destroy(gameObject);
        }
    }

    private IEnumerator EnlargeKeyToScale(float scale)
    {
        Vector3 targetScale = new Vector3(scale, scale, scale);
        Vector3 initialScale = transform.localScale;
        // will scale all dimensions equally
        while(transform.localScale.magnitude < targetScale.magnitude)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, 1);
            yield return null;
        }
    }
}
