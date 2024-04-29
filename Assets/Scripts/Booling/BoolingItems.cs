using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{

    public class BoolingItems : BoolingObjects
    {
        [Header("BoolingObjPlants")]
        [SerializeField] protected List<ObjectSell> _objects;

        public static BoolingItems Instance;

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }
        }

        public virtual void RemoveObject(ObjectSell itemRemove)
        {
            base.RemoveObject(itemRemove.transform);
            itemRemove.SetThisParent(transform);
        }

        /// <summary> Lấy đối tượng có trong ao, nếu không có thì tạo ra </summary>
        /// <returns> điểm spawn , cha của đối tượng, kiểu objectPlant muốn lấy </returns>
        public virtual ObjectSell GetObject(String typeID, Transform setParent)
        {
            // Tìm đối tượng
            ObjectSell objNew = null;

            // Thử tìm đối tượng có thể tái sử dụng
            objNew = FindObject(typeID, null);

            // Không có đối tượng tái sử dụng thì tạo mới
            if (!objNew) objNew = CreateObject(typeID, setParent);
            else objNew.gameObject.SetActive(true);

            if (objNew) OnCreateNewObjSell(objNew, setParent);

            return objNew;
        }

        /// <summary> Tạo objectPlant từ list prefabs </summary>
        /// <returns> Điểm spawn, kiểu objectPlant muốn tạo </returns>
        public virtual ObjectSell CreateObject(String typeID, Transform parentSet)
        {
            ObjectSell objNew = null;

            // Tìm kiểu prefabs muốn tạo có trong kho
            foreach (var item in _objectsPrefab)
            {
                ObjectSell newObjectSell = item.GetComponent<ObjectSell>();
                if (!newObjectSell) continue;
                if (newObjectSell._typeID == typeID)
                {
                    objNew = Instantiate(item).GetComponent<ObjectSell>();
                }
            }

            if (objNew) OnCreateNewObjSell(objNew, this.transform);

            return objNew;
        }


        /// <summary> Tìm đối tượng có trong ao </summary>
        /// <returns> cha của đối tượng đang giữ, kiểu objectPlant muốn tìm</returns>
        public virtual ObjectSell FindObject(String typeID, Transform parentFind)
        {
            // Tìm trong hồ
            foreach (var i in _objects)
            {
                if (!i) continue;
                if (i._typeID == typeID && i._Parent == parentFind)
                {
                    return i;
                }
            }
            return null;
        }

        private void OnCreateNewObjSell(ObjectSell objNew, Transform parentSet)
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
    }

}