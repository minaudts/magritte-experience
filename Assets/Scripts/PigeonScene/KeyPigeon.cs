using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyPigeon : Pigeon
{
    private Key _key;
    private void Awake() 
    {
        _key = GetComponentInChildren<Key>();
    }
    public void DropKey()
    {
        _key.MakeCollectable(true);
    }
}
