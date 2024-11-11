using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    #region Properties

    [SerializeField] private float sensitivity = 6f;
    [SerializeField] private float dampening = 24f;
    [SerializeField] private float LimitY = 85f;

    private Vector2 inputs;
    private Vector2 rotation = Vector2.zero;
    private PlayerControl playerControl;

    #endregion

    #region Methods

    private void Start()
    {
        playerControl = GetComponentInParent<PlayerControl>();
    }

    private void Update()
    {
        SetInputs();
        Rotate();
    }

    private void SetInputs()
    {
        float x = Input.GetAxisRaw("Mouse X");
        float y = Input.GetAxisRaw("Mouse Y");

        inputs.x = x;
        inputs.y = y;
    }

    private void Rotate()
    {
        rotation.x += inputs.x * sensitivity;
        rotation.y += inputs.y * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -LimitY, LimitY);

        Quaternion xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        Quaternion yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);
        Quaternion fQuat = xQuat * yQuat;

        playerControl.Orientation.rotation = xQuat;
        transform.rotation = Quaternion.Slerp(transform.rotation, fQuat, dampening * Time.deltaTime);
    }

    #endregion
}
