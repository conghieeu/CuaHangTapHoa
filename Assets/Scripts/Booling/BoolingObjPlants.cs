using System;
using System.Collections;
using System.Collections.Generic;
using CuaHang;
using Unity.VisualScripting;
using UnityEngine;

public class BoolingObjPlants : BoolingObjects
{
    [Header("BoolingObjPlants")]
    [SerializeField] protected List<ObjectPlant> _objects;

    public static BoolingObjPlants Instance;

    private void Awake()
    {
        if (Instance) Destroy(this); else { Instance = this; }
    }

    /// <summary> Lấy đối tượng có trong ao, nếu không có thì tạo ra </summary>
    /// <returns> điểm spawn , cha của đối tượng, kiểu objectPlant muốn lấy </returns>
    public virtual ObjectPlant GetObject(String typeID, Transform parentSet, Transform parentFind)
    {
        // Tìm/tạo đối tượng
        ObjectPlant objNew = null;
        objNew = FindObject(typeID, parentFind);
        if (!objNew) objNew = CreateObject(typeID, parentSet);

        OnCreateNewObjSell(parentSet, objNew);

        return objNew;
    }

    /// <summary> Tạo objectPlant mới </summary>
    /// <returns> Điểm spawn, kiểu objectPlant muốn tạo </returns>
    public virtual ObjectPlant CreateObject(String typeID, Transform parentSet)
    {
        ObjectPlant objNew = null;

        // Tìm kiểu prefabs muốn tạo có trong kho
        foreach (var i in _objectsPrefab)
        {
            ObjectPlant oPlant = i.GetComponent<ObjectPlant>();
            if (!oPlant) continue;
            if (oPlant._SO._type == typeID)
            {
                objNew = Instantiate(oPlant).GetComponent<ObjectPlant>();
                break;
            }
        }

        OnCreateNewObjSell(parentSet, objNew);

        return objNew;
    }

    private void OnCreateNewObjSell(Transform parentSet, ObjectPlant objNew)
    {
        // Đặt lại giá trị 
        if (objNew)
        {
            objNew.SetThisParent(parentSet);
            objNew.transform.localPosition = Vector3.zero;
            objNew.transform.localRotation = Quaternion.identity;

            // thêm vào kho
            if (_objects.Contains(objNew) == false)
            {
                _objects.Add(objNew);
            }
        }
    }

    /// <summary> Tìm đối tượng có trong ao </summary>
    /// <returns> cha của đối tượng đang giữ, kiểu objectPlant muốn tìm</returns>
    public virtual ObjectPlant FindObject(String typeID)
    {
        // Tìm trong hồ
        foreach (var i in _objects)
        {
            if (!i) continue;
            if (i._typeID == typeID && i._Parent == null)
                return i;
        }
        return null;
    }

    /// <summary> Tìm ObjectPlant đang có slot trống </summary>
    /// <returns> cha của đối tượng đang giữ, đối tượng này có chỗ trống không</returns>
    public virtual ObjectPlant FindObject(String typeID, bool isAnyEmptySlot)
    {
        foreach (var i in _objects)
        {
            if (!i) continue;
            if (i.IsAnyEmptyItem() != isAnyEmptySlot) continue;
            if (i._typeID == typeID && i._Parent == null)
                return i;
        }
        return null;
    }

    /// <summary> Tìm ObjectPlant với kiểu tên và type trùng </summary>
    /// <returns> tên, loại  (có thể để null nếu ko muốn dùng) </returns>
    public virtual ObjectPlant FindObjectType(String type, bool isAnyEmptySlot)
    {
        foreach (var i in _objects)
        {
            if (!i) continue;
            if (i.IsAnyEmptyItem() != isAnyEmptySlot) continue;
            if (i._type == type && i._Parent == null)
                return i;
        }
        return null;
    }

}
