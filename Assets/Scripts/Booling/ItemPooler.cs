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

        /// <summary> 2 item đầu vào có cùng 1 shelf không ? </summary> 
        public bool IsSameShelf(Item itemA, Item itemB) {
            return FindShelfContentItem(itemA) == FindShelfContentItem(itemB);
        }

        /// <summary> Lấy đối tượng có trong ao, nếu không có thì tạo ra </summary>
        public virtual Item GetItemWithTypeID(String typeID, Transform setParent, Transform thisParent, Transform spawnPoint)
        {
            // Tìm/tạo đối tượng
            Item item = null;
            item = FindItemWithTypeID(typeID, thisParent);
            if (!item) item = CreateItem(typeID, setParent, spawnPoint);
            if (item) item._ThisParent = setParent;
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
            if (item) item._ThisParent = setParent;
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
            if (item) item._ThisParent = setParent;

            return item;
        }

        /// <summary> Lấy item đang nhàn rỗi </summary>
        private Item GetItemDisable(String name)
        {
            foreach (var i in _items)
            {
                if (i.name == name && i.gameObject.activeSelf == false && i._ThisParent == null) return i;
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
                if (item._typeID == typeID && item._ThisParent == null && item.gameObject.activeSelf == activeSelf) return item;
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
                if (item._typeID == typeID && item._ThisParent == null) return item;
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
                if (item._typeID == typeID && item._ThisParent == null) itemsOk.Add(item);
            }

            int randomIndex = UnityEngine.Random.Range(0, itemsOk.Count);
            return itemsOk[randomIndex];
        }

        /// <summary> Tìm item có itemSlot có chứa itemProduct cần lấy </summary>
        public virtual Item FindShelfContentItem(Item itemProduct)
        {
            foreach (var item in _items)
            {
                if (!item) continue;
                if (!item._itemSlot) continue;
                if (item._itemSlot.IsContentItem(itemProduct)) return item;
            }
            return null;
        }

        /// <summary> Danh sách item có thể bán </summary>
        public List<Item> GetAllItemsCanSell()
        {
            List<Item> items = new List<Item>();
            foreach (var item in _items)
            {
                if (item == null) continue;
                if (item._isCanSell && item.gameObject.activeSelf && item._ThisParent) items.Add(item);
            }
            return items;
        }

        /// <summary> Lấy danh sách shelf có chứa item </summary>
        public List<Item> GetAllShelfContentItem()
        {
            List<Item> listShelf = new List<Item>();
            foreach (var shelf in _items)
            {
                if (shelf == null) continue;
                if (!shelf._itemSlot) continue;
                if (!shelf._isCanSell && !shelf._ThisParent && shelf._itemSlot.IsAnyItem()) listShelf.Add(shelf);
            }
            return listShelf;
        }

        /// <summary> Lấy danh sách ngẫu nhiên shelf có chứa item </summary>
        public List<Item> GetRandomShelfContentItem()
        {
            List<Item> listShelf = GetAllShelfContentItem();
            int maxShelf = 9;

            // chọn ngẫu nhiên shelf giới hạn từ 1 -> 6
            int shelfCount = UnityEngine.Random.Range(3, maxShelf);

            // Xoá ngẫu nhiên tới khi đặt số lượng shelfCount và phải nhỏ hơn số lượng max
            for (int i = 0; i < listShelf.Count && i < shelfCount; i++)
            {
                listShelf.RemoveAt(UnityEngine.Random.Range(0, listShelf.Count - 1));
            }

            return listShelf;
        }
    }
}