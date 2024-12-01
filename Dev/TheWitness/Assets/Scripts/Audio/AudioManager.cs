using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Runtime.CompilerServices;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private List<EventInstance> eventInstances;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError("Found more than one Audio Manager in the scene");
            Destroy(gameObject);
            return;
        }

        instance = this;

        eventInstances = new List<EventInstance>();

    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);

        eventInstances.Add(eventInstance);
        return eventInstance;
    }


    private void CleanUp()
    {
        //Stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();

        }
    }

    public void StopSound(FMODUnity.EventReference soundEvent)
    {
        FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(soundEvent);
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);  // Utilisez ALLOWFADEOUT si vous voulez un arrêt progressif.
        instance.release();
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
