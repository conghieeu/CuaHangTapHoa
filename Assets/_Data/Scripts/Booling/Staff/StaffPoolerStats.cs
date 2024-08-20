using System.Collections.Generic;
using UnityEngine;

public class StaffPoolerStats : ObjectStats
{
    [Header("StaffPoolerStats")]
    public List<StaffData> _staffs;

    protected override void LoadData(GameData gameData)
    {
        base.LoadData(gameData);

        _staffs = gameData._staffs;

        if (_staffs.Count > 0)
        { 

        }
    }

    protected override void SaveData()
    {
        GetGameData()._staffs = _staffs;
    }
}
