using UnityEngine;

public class PortalManager : SingletonPersistent<PortalManager>
{
    private bool _pigeonPortalComplete = false;
    public void OnPigeonPortalComplete()
    {
        Debug.Log("Setting pigeon portal to true");
        _pigeonPortalComplete = true;
    }
    public bool IsPigeonPortalComplete()
    {
        return _pigeonPortalComplete;
    }
}
