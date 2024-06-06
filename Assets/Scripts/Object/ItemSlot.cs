using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CuaHang.Pooler;
using OpenCover.Framework.Model;
using System;
using Unity.VisualScripting;

namespace CuaHang
{
    public class ItemSlot : MonoBehaviour
    {
        /// <summary> đại diện cho mỗi phần tử của danh sách slot </summary>
        [Serializable]
        public class SlotI
        {
            public Item _item; // object đang gáng trong boolingObject đó
            public Transform _slot;

            // Constructor với tham số
            public SlotI(Item item, Transform slot)
            {
                _item = item;
                _slot = slot;
            }
        }

        [Header("Item Slots")]
        public List<SlotI> _listItem;

        void Awake()
        {
            LoadSlots();
        }

        private void LoadSlots()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                SlotI newSlot = new SlotI(null, transform.GetChild(i));
                if (_listItem.Count < transform.childCount) _listItem.Add(newSlot);
            }
        }

        /// <summary> Items đầu vào có những item giống với this._items </summary>
        public bool ItemsSequenceEqual(List<Item> items)
        {
            foreach (var item in items)
            {
                if (!IsContentItem(item)) return false;
            }
            return true;
        }

        /// <summary> Lấy 1 slot đang rỗng </summary>
        public virtual Transform GetSlotEmpty()
        {
            for (int i = 0; i < _listItem.Count; i++)
            {
                if (_listItem[i]._item == null)
                {
                    return _listItem[i]._slot;
                }
            }
            return null;
        }

        /// <summary> Item Slot này có chứa item typeIDProduct hay không </summary>
        public bool IsContentItem(String typeIDProduct)
        {
            foreach (var i in _listItem)
            {
                if (i._item)
                    if (i._item._typeID == typeIDProduct)
                    {
                        return true;
                    }
            }
            return false;
        }

        public bool IsContentItem(Item item)
        {
            foreach (var i in _listItem) if (i._item == item) return true;
            return false;
        }

        /// <summary> Có slot nào đang trống không </summary>
        public bool IsAnyEmptyItem()
        {
            foreach (var i in _listItem) if (i._item == null) return true;
            return false;
        }

        /// <summary> Có item nào có trong slot không </summary>
        public bool IsAnyItem()
        {
            foreach (var i in _listItem) if (i._item != null) return true;
            return false;
        }

        /// <summary> Thêm item vào slot với TypeID của Item </summary>
        public bool AddItemWithTypeID(string typeID)
        {
            for (int i = 0; i < _listItem.Count; i++)
            {
                // tạo object đưa vào slot
                if (_listItem[i]._item == null && typeID != "")
                {
                    // thêm item vào vị trí đặt sẵn trong slot
                    Item item = ItemPooler.Instance.GetItemWithTypeID(typeID, _listItem[i]._slot, null, _listItem[i]._slot);
                    if (item)
                    {
                        _listItem[i]._item = item;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary> Thêm item vào this._itemSLot </summary>
        public Transform AddItemToSlot(Item item)
        {
            for (int i = 0; i < _listItem.Count; i++)
            {
                if (_listItem[i]._item == null)
                {
                    _listItem[i]._item = item;
                    item._ThisParent = _listItem[i]._slot; 
                    return _listItem[i]._slot;
                }
            }
            return null;
        }

        /// <summary> Xoá item ra khỏi danh sách và ẩn item nó ở thế giới </summary>
        public bool RemoveItemInWorld(Item item)
        {
            for (int i = _listItem.Count - 1; i >= 0; i--)
            {
                if (_listItem[i]._item == item && _listItem[i]._item != null)
                {
                    ItemPooler.Instance.DeleteObject(item.transform);
                    _listItem[i]._item = null;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveItemInList(Item item)
        {
            for (int i = _listItem.Count - 1; i >= 0; i--)
                if (_listItem[i]._item == item && _listItem[i]._item != null)
                {
                    _listItem[i]._item = null;
                    return true;
                }
            return false;
        }

        /// <summary> Delete ALL items </summary> 
        public void RemoveAllItems()
        {
            for (int i = 0; i < _listItem.Count; i++)
            {
                if (_listItem[i]._item != null)
                {
                    RemoveItemInWorld(_listItem[i]._item);
                }
            }
        }

        /// <summary> Ẩn các item trong slot </summary>
        public void HideAllItems()
        {
            foreach (SlotI item in _listItem)
            {
                if (item._item) item._item.gameObject.SetActive(false);
            }
        }

        /// <summary> Lấy toàn bộ item từ sender đang có nạp vào _itemSlot này </summary>
        public virtual void ReceiverItems(ItemSlot sender)
        {
            for (int i = sender._listItem.Count - 1; i >= 0; i--)
            {
                if (sender._listItem[i]._item != null)
                {
                    AddItemToSlot(sender._listItem[i]._item);
                    sender.RemoveItemInList(sender._listItem[i]._item);
                }
            }
        }
        
    }
}