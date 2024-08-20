using UnityEngine;

public class GameSettingStats : ObjectStats
{
    [Header("GameSettingStats")]
    public GameSettingsData _settings;

    protected override void LoadData(GameData gameData)
    {
        base.LoadData(gameData);

        _settings = gameData._gameSettingsData;

        // áp dụng trạng thái vào game
        if (_settings != null)
        {
            
        }
    }

    protected override void SaveData()
    {
        GetGameData()._gameSettingsData = new GameSettingsData(
            _settings._fullScreen, 
            _settings._quality, 
            _settings._masterVolume); 
    }
}
