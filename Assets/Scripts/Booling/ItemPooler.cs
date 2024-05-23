using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class ItemPooler : ObjectPooler
    {
        [Header("BoolingObjPlants")]
        [SerializeField] protected List<Item> _items;

        public static ItemPooler Instance;

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }
            
            LoadItems();
        }

        private void LoadItems()
        {
            foreach (Transform child in transform)
            {
                _items.Add(child.GetComponent<Item>());
            }
        }

        /// <summary> Lấy đối tượng có trong ao, nếu không có thì tạo ra </summary>
        public virtual Item GetItemWithTypeID(String typeID, Transform setParent, Transform thisParent, Transform spawnPoint)
        {
            // Tìm/tạo đối tượng
            Item i = null;
            i = FindItemWithTypeID(typeID, thisParent);
            if (!i) i = CreateObject(typeID, setParent, spawnPoint);
            if (i) OnGetObject(spawnPoint, i);
            return i;
        }

        public virtual Item FindItemInPooler(Item item, Transform setParent, Transform thisParent, Transform spawnPoint)
        {
            Item x = null;
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].transform == item.transform && item.gameObject.activeSelf == false)
                {
                    x = _items[i];
                    x.gameObject.SetActive(true);
                }
            }
            if (x) OnGetObject(spawnPoint, x);
            return x;
        }

        /// <summary> Tạo objectPlant mới </summary>
        /// <returns> Điểm spawn, kiểu objectPlant muốn tạo </returns>
        public virtual Item CreateObject(String name, Transform parentSet, Transform spawnPoint)
        {
            Item objNew = null;
            // Tìm objectPlant có trong kho
            objNew = GetItemDisable(name);

            // Tìm kiểu prefabs muốn tạo có trong kho
            if (objNew == null)
            {
                foreach (var i in _objectsPrefab)
                {
                    Item oPlant = i.GetComponent<Item>();
                    if (!oPlant) continue;
                    if (oPlant._SO._typeID == name)
                    {
                        objNew = Instantiate(oPlant, transform).GetComponent<Item>();
                        break;
                    }
                }
            }
            else
            {
                objNew.gameObject.SetActive(true);
                if (spawnPoint)
                {
                    objNew.transform.position = spawnPoint.position;
                    objNew.transform.rotation = spawnPoint.rotation;
                }
                return objNew;
            }

            if (objNew) OnGetObject(spawnPoint, objNew);

            return objNew;
        }

        /// <summary> Lấy item đang nhàn rỗi </summary>
        private Item GetItemDisable(String name)
        {
            foreach (var i in _items)
            {
                if (i.name == name && i.gameObject.activeSelf == false) return i;
            }
            return null;
        }

        /// <summary> Item sẽ được set lại vị trí và góc xoay tại đây </summary>
        private void OnGetObject(Transform spawnPoint, Item item)
        {
            // Đặt lại giá trị 
            if (!item) return;

            if (spawnPoint)
            {
                item.SetPosition(spawnPoint);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
            }

            // thêm vào kho
            if (_items.Contains(item) == false)
            {
                _items.Add(item);
            }
        }

        /// <summary> Nhân viên tìm bưu kiện </summary>
        public virtual Item FindItemTarget(String typeID, bool activeSelf, Transform whoFindThis)
        {
            // Tìm trong hồ
            foreach (var item in _items)
            {
                if (!item) continue;
                if (item._objFollowedThis) if (item._objFollowedThis != whoFindThis) continue; // tránh việc 2 nhân viên đều muốn nhặt 1 bưu kiện 
                if (item._typeID == typeID && item._thisParent == null && item.gameObject.activeSelf == activeSelf) return item;
            }
            return null;
        }

        /// <summary> tìm item với typeID và item này phải còn slot trống </summary>
        public virtual Item FindItemWithTypeID(String typeID, bool isAnyEmptySlot)
        {
            foreach (var item in _items)
            {
                if (!item) continue;
                if (!item._itemSlot) continue;
                if (item._itemSlot.IsAnyEmptyItem() != isAnyEmptySlot) continue;
                if (item._typeID == typeID && item._thisParent == null) return item;
            }
            return null;
        }
    }

}