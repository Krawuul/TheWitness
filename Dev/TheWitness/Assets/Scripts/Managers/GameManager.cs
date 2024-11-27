using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region Properties

    private PlayerControl player;
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
    public int GameCheckPoint { get => gameCheckPoint; set => gameCheckPoint = value; }

    #endregion

    #region Methods

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<PlayerControl>();
    }

    public GameTime GetTime()
    {
        return gameTime;
    }

    public void NextDay()
    {
        gameTime.day = (ScheduleManager.DAYS)(((int)gameTime.day + 1) % (int)ScheduleManager.DAYS.COUNT);
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
