using System;
using System.Collections;
using System.Collections.Generic;
using HieuDev;
using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    public static event Action<GameSettings> _OnSettingsChanged;

    public bool _fullScreen;
    public string _quality;
    public float _masterVolume;

    private void Start()
    {
        SerializationAndEncryption._OnDataLoaded += LoadSettings;
    }

    public void LoadSettings(GameData gameData)
    {
        _fullScreen = gameData.gameSettings._fullScreen;
        _quality = gameData.gameSettings._quality;
        _masterVolume = gameData.gameSettings._masterVolume;

        _OnSettingsChanged?.Invoke(this);
    }
}
