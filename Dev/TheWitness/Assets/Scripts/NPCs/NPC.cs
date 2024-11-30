
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
    [SerializeField] Transform head;
    [SerializeField] Transform playerPos;
    [SerializeField] GameObject lights;
    bool canEnter = false;
    int[,] schedule;
    [SerializeField] int[] dialoguesCheckPoints;
    bool[] dialoguesStates;
    float timer = 0f;
    float comeTime = 2f;
    Vector3 start;
    Vector3 end;
    private void Start()
    {
        schedule = ScheduleManager.instance.GetSchedule(npcName.ToLower());
        dialoguesStates = new bool[dialoguesCheckPoints.Length + 1];
        start = door.transform.position + door.transform.right * 3f - Vector3.right * 0.35f;
        end = door.transform.position + door.transform.right * 0.2f - Vector3.right * 0.35f;
        start.y = GameManager.instance.Player.transform.position.y;
        end.y = GameManager.instance.Player.transform.position.y;
        visual.transform.position = start;
    }

    public void SetDoor(Door _door)
    {
        door = _door;
    }

    public bool OnDoorInteract()
    {
        if (SubtitleManager.instance.subtitlePlaying)
        {
            return false;
        }
        if (door.IsOpen())
        {
            return true;
        }
        if (schedule[(int)GameManager.instance.GetTime().day, (int)GameManager.instance.GetTime().timestep] == 1)
        {

            foreach (Transform light in lights.transform)
            {
                light.gameObject.SetActive(false);
            }
            if (GameManager.instance.GameCheckPoint == 0 && !dialoguesStates.Last())
            {

                StartCoroutine(DelayedDialogue(npcName.ToUpper() + "P"));
                dialoguesStates[dialoguesStates.Length - 1] = true;

            }
            else
            {
                bool importantDialoguePlayed = false;
                for (int i = 0; i < dialoguesCheckPoints.Length; i++)
                {
                    if (GameManager.instance.GameCheckPoint == dialoguesCheckPoints[i] && !dialoguesStates[i])
                    {
                        importantDialoguePlayed = true;
                        dialoguesStates[i] = true;
                        StartCoroutine(DelayedDialogue("E" + GameManager.instance.GameCheckPoint));
                        if (i == 0)
                        {
                            canEnter = true;
                        }
                        else
                        {
                            canEnter = false;
                        }
                        if (i != 0 || npcName.ToLower() == "veuve")
                        {
                            GameManager.instance.OnNextStep();
                        }
                    }
                }
                if (!importantDialoguePlayed)
                {
                    StartCoroutine(DelayedDialogue(npcName.ToUpper() + "NI"));
                }

            }
            visual.SetActive(true);
            timer = 0;
            return true;

        }
        else
        {
            if (canEnter)
            {
                foreach (Transform light in lights.transform)
                {
                    light.gameObject.SetActive(true);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    IEnumerator DelayedDialogue(string subtitleName)
    {
        string completeName = dialoguesStates.Last() == true && subtitleName.Last() != 'P' ? " (" + npcTrueName + ")" : "";
        yield return new WaitForSeconds(comeTime);
        yield return new WaitUntil(door.IsOpen);

        SubtitleManager.instance.InvokeSubTitle(subtitleName, NameTranslate.names[npcName] + completeName);

        // Jouer l'audio du NPC
        PlayNpcDialogueAudio();
    }

    private void Update()
    {
        if (visual != null && visual.activeSelf)
        {
            timer += Time.deltaTime;
            visual.transform.LookAt(new Vector3(GameManager.instance.Player.transform.position.x, visual.transform.position.y, GameManager.instance.Player.transform.position.z));
            timer = Mathf.Clamp(timer, 0f, comeTime);
            visual.transform.position = Vector3.Lerp(start, end, timer / comeTime);
            Camera.main.transform.LookAt(head);
            if (door.IsClosed())
            {
                visual.SetActive(false);
            }
            GameManager.instance.Player.transform.position = Vector3.MoveTowards(GameManager.instance.Player.transform.position, playerPos.position, Time.deltaTime);
        }
    }


    public void PlayNpcDialogueAudio()
    {
        // Utiliser un EventReference pour la gestion des événements FMOD
        FMODUnity.EventReference audioEvent = GetNpcAudioEvent(npcName.ToLower());
        AudioManager.instance.PlayOneShot(audioEvent, this.transform.position);
    }

    // Cette méthode détermine l'audio à jouer en fonction du nom du NPC
    private FMODUnity.EventReference GetNpcAudioEvent(string npcName)
    {
        //pour associer le NPC à un événement FMOD
        switch (npcName)
        {
            case "pretre":
                return FmodEvents.instance.Priest;  //FmodEvents.Priest est un EventReference
            case "veuve":
                return FmodEvents.instance.Widow;  // FmodEvents.Veuve est un EventReference
            case "vieille":
                return FmodEvents.instance.OldWoman;  // Assure-toi que FmodEvents.Veuve est un EventReference
            case "nain":
                return FmodEvents.instance.Dwarf;  // Assure-toi que FmodEvents.Veuve est un EventReference
            case "boucher":
                return FmodEvents.instance.Butcher;  // Assure-toi que FmodEvents.Veuve est un EventReference
            case "athlete":
                return FmodEvents.instance.Athlete;  // Assure-toi que FmodEvents.Veuve est un EventReference

            default:
                return FmodEvents.instance.Null;  // Valeur par défaut
        }
    }

}