using System;
using UnityEngine;

public class GameSettingStats : ObjectStats
{
    [Header("GAME SETTING STATS")]
    public GameSettingsData _gameSettingData;

    public static event Action<GameSettingsData> _OnDataChange;

    public override void LoadData<T>(T data)
    {
        _gameSettingData = (data as GameData)._gameSettingsData;

        // set properties
        _OnDataChange?.Invoke(_gameSettingData);
    }

    public void SetGameSettingsData(GameSettingsData gameSettingData)
    {
        _gameSettingData = gameSettingData;
    }

    protected override void SaveData()
    {  
        GetGameData()._gameSettingsData = _gameSettingData;
    }
}
