using UnityEngine;

public abstract class Portal : MonoBehaviour
{
    // Mss enum met type meegeven?
    protected MeshRenderer _renderer;
    [SerializeField] protected Material _materialWhenIncomplete;
    [SerializeField] protected Material _materialWhenComplete;

    private void Awake() 
    {
        _renderer = GetComponent<MeshRenderer>();
    }
}
