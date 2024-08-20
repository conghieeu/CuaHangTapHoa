using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CuaHang;
using HieuDev;
using UnityEngine;

public abstract class ObjectStats : HieuBehavior
{
    public static event Action _OnDataChange;

    protected virtual void Start()
    {
        LoadData(GetGameData()); // load khi khoi dong
    }

    protected virtual void OnEnable()
    {
        // bắt sự kiện load
        SerializationAndEncryption._OnDataLoaded += gameData =>
        {
            if (this != null && transform != null)
            {
                LoadData(gameData);
            }
        };

        // save
        SerializationAndEncryption._OnDataSaved += () =>
        {
            if (this != null && transform != null)
            {
                SaveData();
            }
        };
    }

    //-----------PUBLIC--------------
    public string GenerateIdentifier => Guid.NewGuid().ToString();

    //-----------ABSTRACT------------
    /// <summary> Truyền giá trị save vào _gameData </summary>
    protected abstract void SaveData();

    //-----------PROTECTED-VIRTUAL------------

    protected virtual void LoadData(GameData gameData)
    {
        _OnDataChange?.Invoke();
    }

    protected GameData GetGameData() => SerializationAndEncryption.Instance.GameData;
}
