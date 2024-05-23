using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CuaHang.Pooler;
using OpenCover.Framework.Model;
using System;

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

        private void Awake()
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

        /// <summary> Thêm item vào slot đầu ra là vị trí slot đã thêm </summary>
        public Transform AddItemToSlot(Item item)
        {
            for (int i = 0; i < _listItem.Count; i++)
            {
                if (_listItem[i]._item == null)
                {
                    _listItem[i]._item = item;
                    item.SetPosition(_listItem[i]._slot);
                    return _listItem[i]._slot;
                }
            }
            return null;
        }

        /// <summary> Xoá item ở chỗ point slot và load lại object hiển thị </summary>
        public bool DeleteItem(Item item)
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
    }
}