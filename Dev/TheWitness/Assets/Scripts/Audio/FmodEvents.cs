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



    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Found one more Fmod Event script in the scene.");

        }

        instance = this;
    }
}
