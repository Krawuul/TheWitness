using FMOD.Studio;
using Manager;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Phone : MonoBehaviour, IInteractable
{
    [SerializeField] private int[] checkPoints;
    private bool ring = false;

    //Audio
    private EventInstance PhoneRing;

    private EventInstance PhoneVoice;

    private void Start()
    {
        //audio
        PhoneRing = AudioManager.instance.CreateInstance(FmodEvents.instance.PhoneRing);
    }

    public void Interact()
    {
        if (ring && !SubtitleManager.instance.subtitlePlaying)
        {
            PhoneRing.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform.position));
            PhoneRing.stop(STOP_MODE.ALLOWFADEOUT);
            ring = false;
            SubtitleManager.instance.InvokeSubTitle("E" + GameManager.instance.GameCheckPoint, "The Phone");
            GameManager.instance.OnNextStep();

            FMODUnity.EventReference audioEvent = FmodEvents.instance.PhoneVoice;
            PhoneVoice = FMODUnity.RuntimeManager.CreateInstance(audioEvent);
            PhoneVoice.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform.position));

            PhoneVoice.start();
            for (int i = 0; i < checkPoints.Length; i++)
            {
                if (GameManager.instance.GameCheckPoint == checkPoints[i])
                {
                    checkPoints[i] = -10;
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!ring)
        {
            if (checkPoints.Contains(GameManager.instance.GameCheckPoint))
            {
                ring = true;

                PhoneRing.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform.position));
                PhoneRing.start();
            }
            else
            {
                if (PhoneVoice.isValid() && !SubtitleManager.instance.subtitlePlaying)
                {
                    PhoneVoice.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    PhoneVoice.release();
                }
            }
        }
    }
}