using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameTime gameTime;
    private int gameCheckPoint;
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
    #endregion
}
