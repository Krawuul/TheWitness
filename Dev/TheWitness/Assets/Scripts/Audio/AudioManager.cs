using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Runtime.CompilerServices;

public class AudioManager : MonoBehaviour
{
   public static AudioManager instance {get; private set;}

    private List<EventInstance> eventInstances;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene");
        }

        instance = this;

        eventInstances = new List<EventInstance>();
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound,worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance= RuntimeManager.CreateInstance(eventReference); 

        eventInstances.Add(eventInstance);
        return eventInstance;
    }


    private void CleanUp()
    {
        //Stop and release any created instances
        foreach(EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();

        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
