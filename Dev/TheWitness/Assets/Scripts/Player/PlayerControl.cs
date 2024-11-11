using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Swich to new inputs system

public class PlayerControl : MonoBehaviour
{
    #region Properties

    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 5.2f;
    [SerializeField] private Transform orientation;

    private Vector2 inputs;
    private Rigidbody rb;
    private float speed;

    #endregion

    #region Getters & Setters

    public Transform Orientation { get => orientation; }

    #endregion

    #region Methods

    private void Start()
    {
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
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void SetInputs()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        inputs.x = x;
        inputs.y = y;
    }

    private void Move()
    {
        rb.velocity = (orientation.forward * inputs.y + orientation.right * inputs.x).normalized * speed;
    }

    #endregion
}
