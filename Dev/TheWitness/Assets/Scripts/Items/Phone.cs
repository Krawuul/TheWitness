using Manager;
using System.Linq;
using UnityEngine;

public class Phone : MonoBehaviour,IInteractable
{
    [SerializeField] int[] checkPoints;
    bool ring = false; 
    public void Interact()
    {
        if(ring && !SubtitleManager.instance.subtitlePlaying)
        {
            ring = false;
            SubtitleManager.instance.InvokeSubTitle("E" + GameManager.instance.GameCheckPoint, "The Phone");
            GameManager.instance.OnNextStep();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(!ring && checkPoints.Contains( GameManager.instance.GameCheckPoint))
        {
            ring = true;

            AudioManager.instance.PlayOneShot(FmodEvents.instance.PhoneRing, this.transform.position);

        }
    }
}
