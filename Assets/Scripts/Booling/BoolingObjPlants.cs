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
    public virtual ObjectPlant GetObject(String typeID, Transform parentSet, Transform parentFind, Transform spawnPos)
    {
        // Tìm/tạo đối tượng
        ObjectPlant o = null;
        o = FindObject(typeID, parentFind);
        if (!o) o = CreateObject(typeID, parentSet, spawnPos);
        if (o) OnGetObject(parentSet, o);
        return o;
    }

    /// <summary> Tạo objectPlant mới </summary>
    /// <returns> Điểm spawn, kiểu objectPlant muốn tạo </returns>
    public virtual ObjectPlant CreateObject(String typeID, Transform parentSet, Transform spawnPos)
    {
        ObjectPlant objNew = null;
        // Tìm objectPlant có trong kho
        objNew = GetObjectDisable(typeID);

        // Tìm kiểu prefabs muốn tạo có trong kho
        if (objNew == null)
        {
            foreach (var i in _objectsPrefab)
            {
                ObjectPlant oPlant = i.GetComponent<ObjectPlant>();
                if (!oPlant) continue;
                if (oPlant._SO._typeID == typeID)
                {
                    objNew = Instantiate(oPlant, transform).GetComponent<ObjectPlant>();
                    break;
                }
            }
        }
        else
        {
            objNew.gameObject.SetActive(true);
            if (spawnPos)
            {
                objNew.transform.position = spawnPos.position;
                objNew.transform.rotation = spawnPos.rotation;
            }
            return objNew;
        }

        if (objNew) OnGetObject(parentSet, objNew);

        return objNew;
    }

    private ObjectPlant GetObjectDisable(String typeID)
    {
        foreach (var i in _objects)
        {
            if (i._typeID == typeID && i.gameObject.activeSelf == false) return i;
        }
        return null;
    }

    private void OnGetObject(Transform parentSet, ObjectPlant objNew)
    {
        // Đặt lại giá trị 
        if (!objNew) return;

        if (parentSet)
        {
            objNew.SetThisParent(parentSet);
            objNew.transform.localPosition = Vector3.zero;
            objNew.transform.localRotation = Quaternion.identity;
        }

        // thêm vào kho
        if (_objects.Contains(objNew) == false)
        {
            _objects.Add(objNew);
        }

    }

    /// <summary> Tìm đối tượng có trong ao </summary>
    /// <returns> cha của đối tượng đang giữ, kiểu objectPlant muốn tìm</returns>
    public virtual ObjectPlant FindObject(bool activeSelf, String typeID)
    {
        // Tìm trong hồ
        foreach (var i in _objects)
        {
            if (!i) continue;
            Debug.Log(i.gameObject.activeSelf);
            if (i._typeID == typeID && i._Parent == null && i.gameObject.activeSelf == activeSelf) return i;
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
