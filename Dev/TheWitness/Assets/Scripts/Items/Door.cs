using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    #region Properties

    [SerializeField] private float closeAngle;
    [SerializeField] private float openAngle;

    private bool closed = false;

    #endregion

    #region Methods

    private void Update()
    {
        Quaternion rot = Quaternion.AngleAxis(closed != false ? openAngle : closeAngle, transform.right);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 10f);
    }

    public void Interact()
    {
        closed = !closed;
    }

    #endregion
}
