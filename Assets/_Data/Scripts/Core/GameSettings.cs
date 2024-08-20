using System;
using System.Collections;
using System.Collections.Generic;
using CuaHang;
using HieuDev;
using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    [Header("GameSettings")]
    public GameSettingStats _gametStats;

    protected override void Awake()
    {
        base.Awake();

        _gametStats = GetComponentInChildren<GameSettingStats>();
    }
}
