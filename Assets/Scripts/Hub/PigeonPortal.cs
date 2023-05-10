using UnityEngine;
using UnityEngine.Events;

public class PigeonPortal : Portal
{
    private void OnEnable() 
    {
        Debug.Log("OnEnable pigeonPortal");
        bool isComplete = PortalManager.Instance.IsPigeonPortalComplete();
        Debug.Log("IsComplete: "+ isComplete);
        _renderer.material = isComplete ? _materialWhenComplete : _materialWhenIncomplete;
    }
}
