using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    #region Properties

    [SerializeField] private float closeAngle;
    [SerializeField] private float openAngle;
    [SerializeField] private float interpTime;
    [SerializeField] private Transform pivot;

    private float timer;
    private Quaternion closedRot;
    private Quaternion openedRot;


    private bool closed = false;

    #endregion

    #region Methods

    private void Start()
    {
        closedRot = Quaternion.AngleAxis(closeAngle, Vector3.up);
        openedRot = Quaternion.AngleAxis(openAngle, Vector3.up);
    }

    private void Update()
    {
        if (closed)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }
        timer = Mathf.Clamp(timer, 0, interpTime);

        pivot.transform.rotation = Quaternion.Lerp(closedRot, openedRot, timer / interpTime);

        if (pivot.transform.rotation == closedRot || pivot.transform.rotation == openedRot)
        {
            GetComponent<Collider>().enabled = true;
        } 
        else
        {
            GetComponent<Collider>().enabled = false;
        }
    }

    public void Interact()
    {
        closed = !closed;
    }

    #endregion
}
