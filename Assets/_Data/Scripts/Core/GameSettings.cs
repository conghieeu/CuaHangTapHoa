using System;
using System.Collections;
using System.Collections.Generic;
using CuaHang;
using Core;
using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    [Header("GameSettings")]
    public GameSettingStats _gameSettingStats;

    protected override void Awake()
    {
        base.Awake();

        _gameSettingStats = GetComponentInChildren<GameSettingStats>();
    }
}
