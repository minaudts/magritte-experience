using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCollider : MonoBehaviour
{
    public bool isInLight;

    private void OnTriggerStay(Collider other)
    {
        isInLight = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isInLight = false;
    }
}
