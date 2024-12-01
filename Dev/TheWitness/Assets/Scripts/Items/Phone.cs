using FMOD.Studio;
using Manager;
using System.Linq;
using UnityEngine;

public class Phone : MonoBehaviour,IInteractable
{
    [SerializeField] int[] checkPoints;
    bool ring = false;

    //Audio
    private EventInstance PhoneRing;

    private void Start()
    {
        //audio
        PhoneRing = AudioManager.instance.CreateInstance(FmodEvents.instance.PhoneRing);

    }
    public void Interact()
    {
        if(ring && !SubtitleManager.instance.subtitlePlaying)
        {
            PhoneRing.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform.position));
            PhoneRing.stop(STOP_MODE.ALLOWFADEOUT);
            ring = false;
            SubtitleManager.instance.InvokeSubTitle("E" + GameManager.instance.GameCheckPoint, "The Phone");
            GameManager.instance.OnNextStep();

            AudioManager.instance.PlayOneShot(FmodEvents.instance.PhoneVoice, this.transform.position);

            for (int i = 0; i < checkPoints.Length; i++) 
            {
                if(GameManager.instance.GameCheckPoint == checkPoints[i])
                {
                    checkPoints[i] = -10;
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(!ring && checkPoints.Contains( GameManager.instance.GameCheckPoint) )
        {
            ring = true;

            PhoneRing.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform.position));
            PhoneRing.start();

        }
    }
}
