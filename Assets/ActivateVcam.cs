using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ActivateVcam : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _vcamToActivate;
    private void OnTriggerEnter(Collider other) {
        _vcamToActivate.Priority = 99;
    }
}
