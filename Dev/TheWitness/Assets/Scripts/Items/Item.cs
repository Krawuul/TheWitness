using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using UnityEngine.Windows;

public class Item : MonoBehaviour, ICollectable
{
    #region Properties

    [SerializeField] private string info;
    private Quaternion nxtQuat;
    private float dampening = 12f;
    private float sensitivity = 0.4f;

    private PlayerControl playerControl;
    private Vector3 offset = new Vector3(0, 1, 0);

    private Vector3 LeftAxis;

    #endregion

    #region Methods

    virtual public void Pickup(PlayerControl _player)
    {
        Debug.Log("Pickup item of name : " + info);
        playerControl = _player;

        Vector3 forward = (transform.position - playerControl.CameraControl.transform.position).normalized;
        
        nxtQuat = transform.rotation;
        LeftAxis = Quaternion.Euler(0, 90, 0) * forward;

        AudioManager.instance.PlayOneShot(FmodEvents.instance.PickUp, this.transform.position);
    }

    virtual public void Show()
    {
        gameObject.SetActive(true);
        Vector3 pos = playerControl.transform.position + offset + playerControl.CameraControl.transform.forward * 0.5f;
        transform.position = Vector3.Lerp(transform.position,  pos, dampening * Time.deltaTime);

        Vector2 inputs = playerControl.InputAction.InGame.Camera.ReadValue<Vector2>();

        Quaternion xQuat = Quaternion.AngleAxis(inputs.x * sensitivity, Vector3.down);
        Quaternion yQuat = Quaternion.AngleAxis(inputs.y * sensitivity, LeftAxis);
        Quaternion fQuat = xQuat * yQuat;   

        nxtQuat *= Quaternion.Inverse(transform.rotation) * fQuat * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, nxtQuat, dampening * Time.deltaTime);
    }

    virtual public void Hide()
    {
        gameObject.SetActive(false);
    }

    virtual public void Store()
    {
        playerControl.Inventory.AddItem(this);
        Hide();
    }

    #endregion
}
