using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronGate : MonoBehaviour
{
    private Animator _animator;
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    public void Open()
    {
        // Will open gate as it is the default state
        Debug.Log("Opening gate");
        _animator.enabled = true;
    }
}
