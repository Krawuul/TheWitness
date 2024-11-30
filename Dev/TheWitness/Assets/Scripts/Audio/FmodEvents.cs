using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FmodEvents : MonoBehaviour
{
    public static FmodEvents instance {get; private set;}

    [field : Header("PickUp SFX")] 
    [field : SerializeField] public EventReference PickUp { get; private set; }

    [field: Header("PlayerFootSteps SFX")]
    [field: SerializeField] public EventReference PlayerFootSteps { get; private set; }
    
    [field: Header("PlayerFootStepsSprint SFX")]
    [field: SerializeField] public EventReference PlayerFootStepsSprint { get; private set; }

    [field: Header("OpenDoor SFX")]
    [field: SerializeField] public EventReference OpenDoor { get; private set; }

    [field: Header("CloseDoor SFX")]
    [field: SerializeField] public EventReference CloseDoor { get; private set; }

    [field: Header("KnockDoor SFX")]
    [field: SerializeField] public EventReference KnockDoor { get; private set; }

    //Dialogue
    [field: Header("Priest SFX")]
    [field: SerializeField] public EventReference Priest { get; private set; }

    [field: Header("Dwarf SFX")]
    [field: SerializeField] public EventReference Dwarf { get; private set; }

    [field: Header("Butcher SFX")]
    [field: SerializeField] public EventReference Butcher { get; private set; }

    [field: Header("Athlete SFX")]
    [field: SerializeField] public EventReference Athlete { get; private set; }

    [field: Header("Widow SFX")]
    [field: SerializeField] public EventReference Widow { get; private set; }

    [field: Header("OldWoman SFX")]
    [field: SerializeField] public EventReference OldWoman { get; private set; }


    //INSTANCE NULL
    [field: Header("NULL SFX")]
    [field: SerializeField] public EventReference Null { get; private set; }




    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Found one more Fmod Event script in the scene.");

        }

        instance = this;
    }
}
