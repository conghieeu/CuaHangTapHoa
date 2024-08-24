using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CuaHang;
using HieuDev;
using UnityEngine;

public abstract class ObjectStats : HieuBehavior
{
    protected GameData GetGameData() => SerializationAndEncryption.Instance.GameData;

    //-----------ABSTRACT------------
    /// <summary> Truyền giá trị save vào _gameData </summary>
    protected abstract void SaveData();
    public abstract void LoadData<T>(T data);

}
