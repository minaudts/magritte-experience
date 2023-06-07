using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRespawnPoint : MonoBehaviour
{
    public GameObject magritte;
    public GameObject respawnBall;
    
    private void OnTriggerEnter(Collider other)
    {
        magritte.GetComponent<Magritte>()._respawnPoint = transform.position;
        respawnBall.transform.position = transform.position;
    }
}
