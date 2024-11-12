using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO : Swich to new inputs system

public class PlayerControl : MonoBehaviour
{
    #region Properties

    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 5.2f;
    [SerializeField] private Transform orientation;

    private PlayerInputs playerInputAction;
    private Vector2 inputs;
    private Rigidbody rb;
    private float speed;

    private RaycastHit? objectInSight;

    #endregion

    #region Getters & Setters

    public Transform Orientation { get => orientation; }
    public PlayerInputs InputAction { get => playerInputAction; }

    #endregion

    #region Methods

    private void OnEnable()
    {
        playerInputAction.InGame.Enable();

        playerInputAction.InGame.Sprint.started += (context) => speed = runSpeed;
        playerInputAction.InGame.Sprint.canceled += (context) => speed = walkSpeed;

        /* Not final logic
        playerInputAction.InGame.Interact.started += (context) => StartInteract(context, objectInSight);
        playerInputAction.InGame.Interact.canceled += (context) => StopInteract(context, objectInSight);
        */
    }

    private void OnDisable()
    {
        playerInputAction.InGame.Disable();
    }

    private void Awake()
    {
        playerInputAction = new PlayerInputs();
        rb = GetComponent<Rigidbody>();
        speed = walkSpeed;

        // TODO : Move into gamemanager
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        SetInputs();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        objectInSight = CheckInSight();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void SetInputs()
    {
        inputs = playerInputAction.InGame.Movement.ReadValue<Vector2>();
    }

    private void Move()
    {
        rb.velocity = (orientation.forward * inputs.y + orientation.right * inputs.x).normalized * speed;
    }

    private RaycastHit? CheckInSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10f))
        {
            return hit;
        }

        return null;
    }

    private void StartInteract(InputAction.CallbackContext _context, object _interactable)
    {
        // Stop camera + movement
        // Start object interaction
    }

    private void StopInteract(InputAction.CallbackContext _context, object _interactable)
    {
        // Resume camera + movement
        // Stop object interaction
        // If object is collectable store it
    }

#if UNITY_EDITOR

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Speed : " + rb.velocity.magnitude.ToString("F2"));
        GUI.Label(new Rect(10, 30, 200, 20), "Aiming at : " + (objectInSight != null ? objectInSight.Value.collider.name : "None"));
        GUI.Label(new Rect(10, 50, 200, 20), "Pick-up : " + "No");
    }

#endif

#endregion

}
