using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CuaHang.Pooler;
using System;
using Unity.VisualScripting;
using System.Xml.Schema;
using TMPro;

namespace CuaHang
{
    public class ItemSlot : MonoBehaviour
    {
        /// <summary> đại diện cho mỗi phần tử của danh sách slot </summary>
        [Serializable]
        public class ItemsSlots
        {
            public Item _item; // object đang gáng trong boolingObject đó
            public Transform _slot;

            // Constructor với tham số
            public ItemsSlots(Item item, Transform slot)
            {
                _item = item;
                _slot = slot;
            }
        }


        [Header("Item Slots")]
        public Item _item;
        public List<ItemsSlots> _itemsSlots;

        void Awake()
        {
            _item = GetComponentInParent<Item>();
            LoadSlots();
        }

        private void LoadSlots()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                ItemsSlots newSlot = new ItemsSlots(null, transform.GetChild(i));
                if (_itemsSlots.Count < transform.childCount) _itemsSlots.Add(newSlot);
            }
        }

        public Item GetItemWithTypeID(TypeID typeID)
        {
            foreach (var item in _itemsSlots)
            {
                if (item._item) if (item._item._typeID == typeID) return item._item;
            }
            return null;
        }

        public Item GetItemWithType(Type type)
        {
            foreach (var item in _itemsSlots)
            {
                if (item._item) if (item._item._type == type) return item._item;
            }
            return null;
        }

        /// <summary> Trong List item slot thì chỉnh tất cả các bool drag item </summary>
        public void SetItemsDrag(bool value)
        {
            foreach (var i in _itemsSlots)
            {
                if (i._item)
                {
                    i._item._isCanDrag = value;
                    i._item.gameObject.SetActive(value);
                }
            }
        }

        /// <summary> set tất cả các giá tiền trong item slot </summary>
        public void SetPriceInItemSlot(float value)
        {
            foreach (var i in _itemsSlots)
            {
                if (i._item) i._item.SetPrice(value);
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
            for (int i = 0; i < _itemsSlots.Count; i++)
            {
                if (_itemsSlots[i]._item == null)
                {
                    return _itemsSlots[i]._slot;
                }
            }
            return null;
        }

        /// <summary> Item Slot này có chứa item typeIDProduct hay không </summary>
        public bool IsContentItem(TypeID typeIDProduct)
        {
            foreach (var i in _itemsSlots)
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
            foreach (var i in _itemsSlots) if (i._item == item) return true;
            return false;
        }

        /// <summary> Có slot nào đang trống không </summary>
        public bool IsHasSlotEmpty()
        {
            foreach (var slot in _itemsSlots)
            {
                if (slot._item == null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary> Có item nào có trong slot không </summary>
        public bool IsAnyItem()
        {
            foreach (var i in _itemsSlots) if (i._item != null) return true;
            return false;
        }

        /// <summary> Thêm 1 item vào danh sách </summary>
        public bool TryAddItemToItemSlot(Item item, bool isCanDrag)
        {
            foreach (var i in _itemsSlots)
            {
                if (i._item == null)
                {
                    i._item = item;
                    item.SetParent(i._slot, GetComponentInParent<Item>(), isCanDrag);
                    return true;
                }
            }
            return false;
        }

        /// <summary> Xoá item ra khỏi danh sách và ẩn item nó ở thế giới </summary>
        public bool RemoveItemInWorld(Item item)
        {
            for (int i = _itemsSlots.Count - 1; i >= 0; i--)
            {
                if (_itemsSlots[i]._item == item && _itemsSlots[i]._item != null)
                {
                    ItemPooler.Instance.RemoveObject(item);
                    _itemsSlots[i]._item = null;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveItemInList(Item item)
        {
            for (int i = _itemsSlots.Count - 1; i >= 0; i--)
            {
                if (_itemsSlots[i]._item == item && _itemsSlots[i]._item != null)
                {
                    _itemsSlots[i]._item = null;
                    return true;
                }
            }
            return false;
        }

        /// <summary> Delete ALL items </summary> 
        public void RemoveAllItems()
        {
            for (int i = 0; i < _itemsSlots.Count; i++)
            {
                if (_itemsSlots[i]._item != null)
                {
                    RemoveItemInWorld(_itemsSlots[i]._item);
                }
            }
        }

        /// <summary> Ẩn các item trong slot </summary>
        public void HideAllItems()
        {
            foreach (ItemsSlots item in _itemsSlots)
            {
                if (item._item) item._item.gameObject.SetActive(false);
            }
        }

        /// <summary> Lấy toàn bộ item từ sender đang có nạp vào _itemSlot này </summary>
        public virtual void ReceiverItems(ItemSlot sender, bool isCanDrag)
        {
            for (int i = 0; i < sender._itemsSlots.Count; i++)
            {
                if (sender._itemsSlots[i]._item && IsHasSlotEmpty())
                {
                    TryAddItemToItemSlot(sender._itemsSlots[i]._item, isCanDrag);
                    sender.RemoveItemInList(sender._itemsSlots[i]._item);
                }
            }
        }

    }
}