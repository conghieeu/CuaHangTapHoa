using UnityEngine;

public class GameSettingStats : ObjectStats
{
    [Header("GameSettingStats")]
    public GameSettingsData _data;

    public override void LoadData<T>(T data)
    {
        _data = GetGameData()._gameSettingsData;
    }

    protected override void SaveData()
    {
        GetGameData()._gameSettingsData = new GameSettingsData(
            _data._fullScreen,
            _data._quality,
            _data._masterVolume);
    }
}
