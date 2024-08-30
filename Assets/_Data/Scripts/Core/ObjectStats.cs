using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CuaHang;
using Core;
using UnityEngine;

public abstract class ObjectStats : HieuBehavior
{
    protected virtual void Start() // root thì mới cần bắt sự kiện
    {
        // có file save thì load
        if (SerializationAndEncryption._isExistsSaveFile)
        {
            Debug.Log("Log file save");
            LoadData(GetGameData());
        }

        // load event
        SerializationAndEncryption._OnDataLoaded += gameData =>
        {
            if (this != null && transform != null)
            {
                LoadData(gameData);
            }
        };

        // save event
        SerializationAndEncryption._OnDataSaved += () =>
        {
            if (this != null && transform != null)
            {
                SaveData();
            }
        };
    }

    protected GameData GetGameData() => SerializationAndEncryption.Instance.GameData;

    //-----------ABSTRACT------------
    /// <summary> Truyền giá trị save vào _gameData </summary>
    protected abstract void SaveData();
    public abstract void LoadData<T>(T data);

}
