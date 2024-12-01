using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Utils;

public class GameManager : Singleton<GameManager>
{
    #region Properties

    private PlayerControl player;
    private Notebook notebookUI;
    private bool inMenu = false;
    [Serializable]
    public struct GameTime
    {
        public ScheduleManager.DAYS day;
        public ScheduleManager.TIMESTEP timestep ;
    }
    [SerializeField] private GameTime gameTime ;
    [SerializeField] private int gameCheckPoint =0;
    private int nbNpcs = 7;
    private int npcVisited = 0;
    Vector3 startPos;
    bool endstarted = false;
    #endregion

    #region Getters & Setters

    public PlayerControl Player { get =>  player; }
    public Notebook Notebook { get => notebookUI; }
    public int GameCheckPoint { get => gameCheckPoint; set => gameCheckPoint = value; }
    public bool InMenu { get => inMenu; }

    #endregion

    #region Methods

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<PlayerControl>(true);
        notebookUI = FindObjectOfType<Notebook>(true);
        EventsManager eventManager = FindObjectOfType<EventsManager>(true);
        UnityEvent presentationEvent = new UnityEvent();
        UnityEvent doorsShutDown = new UnityEvent();
        presentationEvent.AddListener(OnNextStep);
        doorsShutDown.AddListener(() =>
        {
            foreach (var door in GameObject.FindObjectsOfType<Door>())
            {
                if(door.IsOpen())
                {
                    door.Interact();
                }
            };
        });
        doorsShutDown.AddListener(() =>
        {
            if (gameCheckPoint == 16)
            {
                StartCoroutine(EndGame());
            }

        });
        doorsShutDown.AddListener(() =>
        {
            foreach (var npc in GameObject.FindObjectsOfType<NPC>())
            {
                if (npc.npcAudioInstance.isValid())
                {
                    npc.npcAudioInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    npc.npcAudioInstance.release();
                }
            };
        });
        doorsShutDown.AddListener(NextTimeStep);
        eventManager.events.Add("PRESENTATION", presentationEvent);
        eventManager.events.Add("ALL_DIALOGUE", doorsShutDown);
        
        gameTime.day = ScheduleManager.DAYS.SATURDAY;
        startPos = player.transform.position;
        
    }
    IEnumerator EndGame()
    {
        endstarted = true;
        yield return new WaitForSeconds(30f);

        SceneManager.LoadScene(0);
    }


    private void Start()
    {
        SubtitleManager.instance.InvokeSubTitle("INTRO", "Narrator");

    }
    public void OpenCloseInventory()
    {
        if (notebookUI.gameObject.activeSelf)
        {
            notebookUI.gameObject.SetActive(false);
            inMenu = false;
            SetCursorLockState(true);
        } 
        else
        {
            notebookUI.gameObject.SetActive(true);
            inMenu = true;
            SetCursorLockState(false);
        }
    }

    public void SetCursorLockState(bool _state)
    {
        Cursor.lockState = _state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !_state;
    }

    public GameTime GetTime()
    {
        return gameTime;
    }

    public void NextDay()
    {
        gameTime.day = (ScheduleManager.DAYS)(((int)gameTime.day + 1) % (int)ScheduleManager.DAYS.COUNT);
        gameTime.timestep = ScheduleManager.TIMESTEP.MORNING;
        Manager.CanvasManager.instance.Fade();
        StartCoroutine(PlacePlayer());
    }

    IEnumerator PlacePlayer()
    {
        yield return new WaitForSeconds(2.5f);
        player.transform.position = startPos;
    }

    public void NextTimeStep()
    {
        gameTime.timestep = (ScheduleManager.TIMESTEP)(((int)gameTime.timestep + 1) % (int)ScheduleManager.TIMESTEP.COUNT);
        if(gameTime.timestep == ScheduleManager.TIMESTEP.MORNING)
        {
            NextDay();
        }
    }

    private void Update()
    {
        if(gameCheckPoint == 16 && !SubtitleManager.instance.subtitlePlaying && !endstarted)
        {
            SubtitleManager.instance.InvokeSubTitle("E16", "The Phone");
        }
    }

    public void OnNextStep()
    {
        Debug.Log(gameCheckPoint);
        if(gameCheckPoint == 0)
        {
            npcVisited++;
            if(npcVisited == nbNpcs)
            {
                gameCheckPoint++;
                Debug.Log(gameCheckPoint);
            }
        }else
        {
            gameCheckPoint++;
        }
    }
    #endregion
}
