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
            Item item = null;
            item = FindItemWithTypeID(typeID, thisParent);
            if (!item) item = CreateItem(typeID, setParent, spawnPoint);
            if (item) item.SetThisParent(setParent);
            return item;
        }

        public virtual Item FindItemInPooler(Item item, Transform setParent, Transform thisParent, Transform spawnPoint)
        {
            Item rItem = null;
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].transform == item.transform && item.gameObject.activeSelf == false)
                {
                    rItem = _items[i];
                    rItem.gameObject.SetActive(true);
                }
            }
            if (item) item.SetThisParent(setParent);
            return rItem;
        }

        /// <summary> Tạo objectPlant mới </summary>
        /// <returns> Điểm spawn, kiểu objectPlant muốn tạo </returns>
        public virtual Item CreateItem(String typeID, Transform setParent, Transform spawnPoint)
        {
            Item item = GetItemDisable(typeID);

            if (item == null)
            {
                foreach (var objP in _objectsPrefab)
                {
                    Item i = objP.GetComponent<Item>();
                    if (i) if (i._SO._typeID == typeID)
                        {
                            item = Instantiate(i).GetComponent<Item>();
                            break;
                        }
                }
            }

            if (!_items.Contains(item)) _items.Add(item); // thêm item vào kho
            if (item) item.SetThisParent(setParent);

            return item;
        }

        /// <summary> Lấy item đang nhàn rỗi </summary>
        private Item GetItemDisable(String name)
        {
            foreach (var i in _items)
            {
                if (i.name == name && i.gameObject.activeSelf == false && i._thisParent == null) return i;
            }
            return null;
        }

        /// <summary> Lấy item trong _items đk là không có follower (item trùng) </summary>
        public Item GetItem(Item itemI)
        {
            foreach (var item in _items)
            {
                if (item._follower == false && itemI == item) return item;
            }
            return null;
        }


        /// <summary> Nhân viên tìm bưu kiện </summary>
        public virtual Item FindItemTarget(String typeID, bool activeSelf, Transform whoFindThis)
        {
            // Tìm trong hồ
            foreach (var item in _items)
            {
                if (!item) continue;
                if (item._follower) if (item._follower != whoFindThis) continue; // tránh việc 2 nhân viên đều muốn nhặt 1 bưu kiện 
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

        /// <summary> tìm item với typeID và item này phải còn slot trống </summary>
        public virtual Item FindRandomItemWithTypeID(String typeID, bool isAnyEmptySlot)
        {
            // Lấy danh sách item thoa mãn
            List<Item> itemsOk = new List<Item>();
            foreach (var item in _items)
            {
                if (!item) continue;
                if (!item._itemSlot) continue;
                if (item._itemSlot.IsAnyEmptyItem() != isAnyEmptySlot) continue;
                if (item._typeID == typeID && item._thisParent == null) itemsOk.Add(item);
            }

            int randomIndex = UnityEngine.Random.Range(0, itemsOk.Count);
            return itemsOk[randomIndex];
        }

        /// <summary> Tìm item có itemSlot có chứa item cần lấy </summary>
        public virtual Item FindItemContentProduct(Item itemProduct)
        {
            foreach (var item in _items)
            {
                if (!item) continue;
                if (!item._itemSlot) continue;
                if (item._itemSlot.IsContentItem(itemProduct)) return item;
            }
            return null;
        }

        /// <returns> Danh sách item có thể bán </returns>
        public List<Item> GetAllItemsCanSell()
        {
            List<Item> items = new List<Item>();
            foreach (var item in _items)
            {
                if (!item) continue;
                if (item._isCanSell && item.gameObject.activeSelf && item._thisParent) items.Add(item);
            }
            return items;
        }


    }
}