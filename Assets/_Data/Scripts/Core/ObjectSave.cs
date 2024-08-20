using System;
using CuaHang;
using HieuDev;
using UnityEngine;

public abstract class ObjectSave : Singleton<ObjectSave>
{
    public static event Action _OnDataChange;

    [Header("ObjectSave")]
    public SerializationAndEncryption _serializationAndEncryption;

    protected virtual void Start()
    {
        _serializationAndEncryption = SerializationAndEncryption.Instance;

        // khi không bắt được sự kiện của load khi mới bắt đầu vì GameData đã được tải trước đó nên phải load 1 lần khi start
        LoadPlayerData(_serializationAndEncryption.GameData);
    }

    protected virtual void OnEnable()
    {
        // bắt sự kiện load
        SerializationAndEncryption._OnDataLoaded += gameData =>
        {
            if (this != null && transform != null)
            {
                LoadPlayerData(gameData);
            }
        };
    }

    protected virtual void LoadPlayerData(GameData gameData)
    { 
        _OnDataChange?.Invoke();
    }

    protected virtual void SavePlayerData()
    {
        SerializationAndEncryption.Instance.SaveGameData();
    }
}
