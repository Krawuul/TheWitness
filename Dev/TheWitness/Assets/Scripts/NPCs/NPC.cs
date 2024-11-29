
using Manager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] string npcName;
    [SerializeField] string npcTrueName;
    [SerializeField] Door door;
    [SerializeField] GameObject visual;
    bool canEnter =false;
    int[,] schedule;
    [SerializeField] int[] dialoguesCheckPoints;
    bool[] dialoguesStates;
    private void Start()
    {
        schedule = ScheduleManager.instance.GetSchedule(npcName.ToLower());
        dialoguesStates = new bool[dialoguesCheckPoints.Length +1];
        
    }

    public void SetDoor(Door _door)
    {
        door = _door;
    }

    public bool OnDoorInteract()
    {
        if(SubtitleManager.instance.subtitlePlaying)
        {
            return false;
        }
        if(door.IsOpen())
        {
            return true;
        }
        if(schedule[(int)GameManager.instance.GetTime().day, (int)GameManager.instance.GetTime().timestep] == 1)
        {        
            if(GameManager.instance.GameCheckPoint == 0 && !dialoguesStates.Last())
            {
               
                StartCoroutine(DelayedDialogue(npcName.ToUpper() + "P"));
                dialoguesStates[dialoguesStates.Length - 1] = true;
            }
            else
            {
                bool importantDialoguePlayed = false;
                for(int i =0; i < dialoguesCheckPoints.Length;i++)
                {
                    if(GameManager.instance.GameCheckPoint == dialoguesCheckPoints[i] && !dialoguesStates[i])
                    {
                        importantDialoguePlayed = true;
                        dialoguesStates[i] = true;
                        StartCoroutine(DelayedDialogue("E" + GameManager.instance.GameCheckPoint));
                        if(i ==0)
                        {
                            canEnter = true;
                        }else
                        {
                            canEnter = false;
                        }
                        if(i != 0 || npcName.ToLower() =="veuve")
                        {
                            GameManager.instance.OnNextStep();
                        }
                    }
                }
                if(!importantDialoguePlayed)
                {
                    StartCoroutine(DelayedDialogue(npcName.ToUpper() + "NI"));
                }
            }
            return true;

        }
        else
        {
            if(canEnter)
            {
                return true;
            }else
            {
                return false;
            }
        }
    }


    IEnumerator DelayedDialogue(string subtitleName)
    {
        string completeName = dialoguesStates.Last() == true && subtitleName.Last() != 'P' ? " (" + npcTrueName + ")" : "";
        yield return new WaitUntil(door.IsOpen);
        
        SubtitleManager.instance.InvokeSubTitle(subtitleName, NameTranslate.names[npcName] + completeName);
    }
}
