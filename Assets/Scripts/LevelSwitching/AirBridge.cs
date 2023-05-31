using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AirBridge : MonoBehaviour
{
    public void Appear()
    {
        gameObject.SetActive(true);
        Debug.Log("Bridge appears");
    }
}
