using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeashooterCollider : MonoBehaviour
{
    private Peashooter _parent;
    // Start is called before the first frame update
    void Start()
    {
        _parent = gameObject.GetComponentInParent<Peashooter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        _parent.OnTriggerEnterWrapper(other);
    }
    private void OnTriggerExit(Collider other) {
        _parent.OnTriggerExitWrapper(other);
    }
}
