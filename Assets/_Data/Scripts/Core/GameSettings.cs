using System;
using System.Collections;
using System.Collections.Generic;
using HieuDev;
using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    public static event Action<GameSettingsData> _OnSettingsChanged;
    public GameSettingsData _gameSettingsData;

    private void Start()
    {
        SerializationAndEncryption._OnDataLoaded += LoadSettings;
    }

    public void LoadSettings(GameData gameData)
    { 
        _gameSettingsData = gameData._gameSettingsData;
        _OnSettingsChanged?.Invoke(gameData._gameSettingsData);
    }
}
