using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MoveToClickPoint : MonoBehaviour
{
    //[SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private NavMeshAgent agent;
    private Camera cam;
    //private InputAction touchPositionAction;
    //private InputAction touchPressAction;
    private Mouse _mouse;
    private Touchscreen _touchscreen;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //touchPositionAction = inputActionAsset.FindActionMap("InGame").FindAction("TouchPosition");
        //touchPressAction = inputActionAsset.FindActionMap("InGame").FindAction("TouchPress");
        cam = Camera.main;
        _mouse = Mouse.current;
        _touchscreen = Touchscreen.current;
    }

    private void Update() {
        // check for mouse input
        if(_mouse != null && _mouse.leftButton.wasPressedThisFrame) 
        {
            OnTouch(_mouse.position.ReadValue(), _mouse.clickCount.ReadValue() != 1);
        }
        // check for touchscreen input
        else if (_touchscreen != null && _touchscreen.primaryTouch.tap.isPressed)
        {
            OnTouch(_touchscreen.position.ReadValue(), _touchscreen.primaryTouch.tapCount.ReadValue() != 1);
        }
    }

    private void OnTouch(Vector2 screenPosition, bool doublePress)
    {
        //Debug.Log("Pressed position " + screenPosition.x + ", " + screenPosition.y);
        agent.speed = doublePress ? runSpeed : walkSpeed;
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(screenPosition), out hit, 100))
        {
            agent.destination = hit.point;
        }
    }

    /*public void OnPress(InputAction.CallbackContext context)
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
    }*/
}
