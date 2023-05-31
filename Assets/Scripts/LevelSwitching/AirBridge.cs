using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBridge : MonoBehaviour
{
    private BoxCollider _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider.enabled = false;
    }

    public void Appear()
    {
        gameObject.SetActive(true);
        _collider.enabled = true;
        Debug.Log("Bridge appears");
    }
}
