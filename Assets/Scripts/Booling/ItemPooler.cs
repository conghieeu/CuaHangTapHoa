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
        public bool IsSameShelf(Item itemA, Item itemB)
        {
            return FindShelfContentItem(itemA) == FindShelfContentItem(itemB);
        }

        /// <summary> Tạo, lấy item theo typeID </summary> 
        public virtual Item GetItemWithTypeID(TypeID typeID)
        {
            Item item = GetItemDisable(typeID);

            // Create New Item
            if (item)
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                foreach (var objPrefab in _listPrefab)
                {
                    Item itemPrefab = objPrefab.GetComponent<Item>();
                    if (itemPrefab)
                    {
                        if (itemPrefab._SO._typeID == typeID)
                        {
                            item = Instantiate(itemPrefab, transform).GetComponent<Item>();
                            _items.Add(item); // thêm item vào kho
                            break;
                        }
                    }
                }
            }

            return item;
        }

        /// <summary> Lấy item đang nhàn rỗi </summary>
        private Item GetItemDisable(TypeID typeID)
        {
            foreach (var item in _items)
            {
                if (item._typeID == typeID && !item.gameObject.activeSelf && !item.GetParent) return item;
            }
            return null;
        }

        /// <summary> Nhân viên tìm bưu kiện </summary>
        public virtual Item FindItemTarget(TypeID typeID, bool activeSelf, Transform whoFindThis)
        {
            // Tìm trong hồ
            foreach (var item in _items)
            {
                if (!item) continue;
                if (item._typeID == typeID && !item.GetParent && item.gameObject.activeSelf == activeSelf) return item;
            }
            return null;
        }

        /// <summary> tìm item với typeID và item này phải còn slot trống </summary>
        public virtual Item FindItemWithTypeID(TypeID typeID)
        {
            foreach (var item in _items)
            {
                if (!item) continue;
                if (!item._itemSlot) continue;
                if (item._typeID == typeID && item.GetParent == null && item._itemSlot.IsHasSlotEmpty()) return item;
            }
            return null;
        }

        /// <summary> tìm item với typeID và item này phải còn slot trống </summary>
        public virtual Item FindRandomItemWithTypeID(TypeID typeID, bool isAnyEmptySlot)
        {
            // Lấy danh sách item thoa mãn
            List<Item> itemsOk = new List<Item>();
            foreach (var item in _items)
            {
                if (!item) continue;
                if (!item._itemSlot) continue;
                if (item._itemSlot.IsHasSlotEmpty() != isAnyEmptySlot) continue;
                if (item._typeID == typeID && !item.GetParent) itemsOk.Add(item);
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
                if (item._isCanSell && item.gameObject.activeSelf && item.GetParent) items.Add(item);
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
                if (!shelf._isCanSell && !shelf.GetParent && shelf._itemSlot.IsAnyItem()) listShelf.Add(shelf);
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