using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region Properties

    private PlayerControl player;

    #endregion

    #region Getters & Setters

    public PlayerControl Player { get =>  player; } 

    #endregion

    #region Methods

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<PlayerControl>();
    }

    #endregion
}
