using Manager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class NPC : MonoBehaviour
{
    [SerializeField] string npcName;
    [SerializeField] string npcTrueName;
    [SerializeField] Door door;
    [SerializeField] GameObject visual;
    [SerializeField] Transform head;
    [SerializeField] Transform playerPos;
    [SerializeField] GameObject lights;
    [SerializeField] float inverse = 1f;
    [SerializeField] float inverse2 = 1f;

    Volume volume;
    bool canEnter = false;
    int[,] schedule;
    [SerializeField] int[] dialoguesCheckPoints;
    bool[] dialoguesStates;
    float timer = 0f;
    float comeTime = 2f;
    Vector3 start;
    Vector3 end;

    // Variables pour gérer l'audio et le mouvement
    public FMOD.Studio.EventInstance npcAudioInstance;
    private Vector3 lastPlayerPosition;
    private bool isDialoguePlaying = false;

    private void Start()
    {
        schedule = ScheduleManager.instance.GetSchedule(npcName.ToLower());
        dialoguesStates = new bool[dialoguesCheckPoints.Length + 1];
        if (npcName != "veuve")
        {
            start = door.transform.position + door.transform.right * 3f * inverse - transform.right * 0.35f * inverse2;
        }
        else
        {
            start = door.transform.position + door.transform.right * 2f * inverse - transform.right * 0.35f * inverse2;
        }
        end = door.transform.position + door.transform.right * 0.2f * inverse - transform.right * 0.35f * inverse2;
        start.y = GameManager.instance.Player.transform.position.y;
        end.y = GameManager.instance.Player.transform.position.y;
        visual.transform.position = start;
        volume = GameObject.FindObjectOfType<Volume>();
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
            if (lights != null)
            {
                foreach (Transform light in lights.transform)
                {
                    light.gameObject.SetActive(false);
                }
            }
            Bloom b;
            volume.profile.TryGet(out b);
            b.active = false;

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
                if (lights != null)
                {
                    foreach (Transform light in lights.transform)
                    {
                        light.gameObject.SetActive(true);
                    }
                }
                Bloom b;
                volume.profile.TryGet(out b);
                b.active = true;
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
        isDialoguePlaying = true;  // Indique que le dialogue est en cours
        lastPlayerPosition = GameManager.instance.Player.transform.position;  // Sauvegarde la position actuelle du joueur

        // Attendre que le joueur se déplace ou que le dialogue se termine
        yield return new WaitUntil(() => HasPlayerMoved());
        StopNpcDialogueAudio(); // Arrêter l'audio quand le joueur bouge
    }

    // Vérifie si le joueur a bougé depuis la dernière position
    private bool HasPlayerMoved()
    {
        Vector3 currentPlayerPosition = GameManager.instance.Player.transform.position;
        return Vector3.Distance(lastPlayerPosition, currentPlayerPosition) > 0.1f;
    }

    private void Update()
    {
        if (visual != null && visual.activeSelf)
        {
            timer += Time.deltaTime;
            visual.transform.LookAt(new Vector3(GameManager.instance.Player.transform.position.x, visual.transform.position.y, GameManager.instance.Player.transform.position.z));
            timer = Mathf.Clamp(timer, 0f, comeTime);
            visual.transform.position = Vector3.Lerp(start, end, timer / comeTime);
            Camera.main.transform.parent.LookAt(head);

            if (door.IsClosed())
            {
                visual.SetActive(false);
            }

            GameManager.instance.Player.transform.position = Vector3.MoveTowards(GameManager.instance.Player.transform.position, playerPos.position, Time.deltaTime);
        }
    }

    public void PlayNpcDialogueAudio()
    {
        FMODUnity.EventReference audioEvent = GetNpcAudioEvent(npcName.ToLower());
        npcAudioInstance = FMODUnity.RuntimeManager.CreateInstance(audioEvent);
        npcAudioInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform.position));

        npcAudioInstance.start();
    }

    private void StopNpcDialogueAudio()
    {
        if (npcAudioInstance.isValid())
        {
            npcAudioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            npcAudioInstance.release();
        }
    }

    private FMODUnity.EventReference GetNpcAudioEvent(string npcName)
    {
        switch (npcName)
        {
            case "pretre":
                return FmodEvents.instance.Priest;
            case "veuve":
                return FmodEvents.instance.Widow;
            case "vieille":
                return FmodEvents.instance.OldWoman;
            case "nain":
                return FmodEvents.instance.Dwarf;   
            case "boucher":
                return FmodEvents.instance.Butcher;
            case "sportif":
                return FmodEvents.instance.Athlete;
            default:
                return FmodEvents.instance.Null;
        }
    }
}
