using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class GameManager : Singleton<GameManager>
{
    #region Properties

    private PlayerControl player;
    private InventoryUI inventoryUI;
    private bool inMenu = false;
    public struct GameTime
    {
        public ScheduleManager.DAYS day;
        public ScheduleManager.TIMESTEP timestep ;
    }
    private GameTime gameTime ;
    [SerializeField] private int gameCheckPoint =0;
    private int nbNpcs = 6;
    private int npcVisited = 0;
    #endregion

    #region Getters & Setters

    public PlayerControl Player { get =>  player; }
    public InventoryUI InventoryUI { get => inventoryUI; }
    public int GameCheckPoint { get => gameCheckPoint; set => gameCheckPoint = value; }
    public bool InMenu { get => inMenu; }

    #endregion

    #region Methods

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<PlayerControl>(true);
        inventoryUI = FindObjectOfType<InventoryUI>(true);
        EventsManager eventManager = FindObjectOfType<EventsManager>(true);
        UnityEvent presentationEvent = new UnityEvent();
        presentationEvent.AddListener(OnNextStep);
        eventManager.events.Add("PRESENTATION", presentationEvent);

        gameTime.day = ScheduleManager.DAYS.SATURDAY;
    }

    public void OpenCloseInventory()
    {
        if (inventoryUI.gameObject.activeSelf)
        {
            inventoryUI.gameObject.SetActive(false);
            inMenu = false;
            SetCursorLockState(true);
        } 
        else
        {
            inventoryUI.gameObject.SetActive(true);
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
    }

    public void NextTimeStep()
    {
        gameTime.timestep = (ScheduleManager.TIMESTEP)(((int)gameTime.timestep + 1) % (int)ScheduleManager.TIMESTEP.COUNT);
        if(gameTime.timestep == ScheduleManager.TIMESTEP.MORNING)
        {
            NextDay();
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
