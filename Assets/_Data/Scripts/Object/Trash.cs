using System;
using System.Collections;
using System.Collections.Generic;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang
{
    [Serializable]
    public class ParcelTrash
    {
        public float _time = 0;
        public Item _item;
    }

    public class Trash : Item
    {
        [Header("TRASH")]
        public float _timeDelete; // thời gian để xoá đi đối tượng bênh trong kho
        public List<ParcelTrash> _listTrash; // thời gian để xoá đi đối tượng bênh trong kho

        private void Start() {
            for (int i = 0; i < _itemSlot._itemsSlots.Count; i++) _listTrash.Add(new ParcelTrash());
        }

        private void FixedUpdate()
        {
            CountDownRemove();
        }

        /// <summary> thùng rác đếm ngược về 0 sẽ xoá item </summary>
        private void CountDownRemove()
        {
            for (int i = 0; i < _listTrash.Count; i++)
            {
                // đếm ngược
                if (_listTrash[i]._time > 0f)
                {
                    _listTrash[i]._time -= Time.fixedDeltaTime;
                }

                Item item = _listTrash[i]._item;

                // xoá item
                if (_listTrash[i]._time <= 0f && item)
                {
                    item.SetParent(null, null, false);
                    _itemSlot.RemoveItemInWorld(_listTrash[i]._item);
                    _listTrash[i]._item = null;
                }
            }
        }

        /// <summary> Thêm item rác vào thùng rác </summary>
        public void AddItemToTrash(Item item)
        {
            foreach (var trash in _listTrash)
            {
                if (trash._item == null)
                {
                    trash._time = _timeDelete;
                    trash._item = item;
                    break;
                }
            }
        }
    }
}