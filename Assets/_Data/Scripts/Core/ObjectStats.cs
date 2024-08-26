using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CuaHang;
using HieuDev;
using UnityEngine;

public abstract class ObjectStats : HieuBehavior
{
    protected virtual void Start() // root thì mới cần bắt sự kiện
    {
        // LoadData(GetGameData());

        // load
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

    protected GameData GetGameData() => SerializationAndEncryption.Instance.GameData;

    //-----------ABSTRACT------------
    /// <summary> Truyền giá trị save vào _gameData </summary>
    protected abstract void SaveData();
    public abstract void LoadData<T>(T data);

}
