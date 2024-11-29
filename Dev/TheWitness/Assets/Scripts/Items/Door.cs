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
    [SerializeField] NPC npc;

    private float timer;
    private Quaternion closedRot;
    private Quaternion openedRot;
    
    public bool IsOpen() { return timer == interpTime; }


    private bool closed = false;

    #endregion

    #region Methods

    private void Start()
    {
        closedRot = Quaternion.AngleAxis(closeAngle, Vector3.up);
        openedRot = Quaternion.AngleAxis(openAngle, Vector3.up);

        if (pivot == null)
        {
            pivot = transform;
        }
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

        pivot.transform.localRotation = Quaternion.Lerp(closedRot, openedRot, timer / interpTime);

        if (pivot.transform.localRotation == closedRot || pivot.transform.localRotation == openedRot)
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
        if (npc == null)
        {
            closed = !closed;
        }else
        {
            if(npc.OnDoorInteract())
            {
                closed = !closed;
            }
        }
    }

    #endregion
}
