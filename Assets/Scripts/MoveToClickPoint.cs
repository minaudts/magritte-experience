using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MoveToClickPoint : MonoBehaviour
{
    private NavMeshAgent agent;
    private Camera cam;
    [SerializeField] private InputAction clickAction;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, 100))
            {
                agent.destination = hit.point;
            }
    }

    private void OnEnable() 
    {
        clickAction.Enable();
        clickAction.performed += OnClick;
    }

    private void OnDisable() 
    {
        clickAction.performed -= OnClick;
        clickAction.Disable();
    }
}
