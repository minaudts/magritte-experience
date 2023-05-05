using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MoveToClickPoint : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActionAsset;
    private NavMeshAgent agent;
    private Camera cam;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        touchPositionAction = inputActionAsset.FindActionMap("InGame").FindAction("TouchPosition");
        touchPressAction = inputActionAsset.FindActionMap("InGame").FindAction("TouchPress");
        cam = Camera.main;
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        Vector2 touchPos = touchPositionAction.ReadValue<Vector2>();
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(touchPos), out hit, 100))
        {
            agent.destination = hit.point;
        }
    }

    private void OnEnable() 
    {
        touchPressAction.Enable();
        touchPositionAction.Enable();
        touchPressAction.performed += OnPress;
    }

    private void OnDisable() 
    {
        touchPressAction.performed -= OnPress;
        touchPressAction.Disable();
        touchPositionAction.Disable();
    }
}
