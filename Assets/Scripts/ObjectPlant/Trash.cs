using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    [Serializable]
    public class ItemNeedDelete
    {
        public float _time = 0;
        public Transform _item;
    }

    public class Trash : StorageRoom
    {
        [Header("           Trash")]
        public float _timeDelete; // thời gian để xoá đi đối tượng bênh trong kho
        public List<ItemNeedDelete> _itemsDelete; // thời gian để xoá đi đối tượng bênh trong kho

        private void FixedUpdate()
        {
            for (int i = 0; i < _itemsDelete.Count; i++)
            {
                _itemsDelete[i]._time -= Time.fixedDeltaTime;

                if (_itemsDelete[i]._time <= 0f)
                {
                    if (_itemsDelete[i]._item) Destroy(_itemsDelete[i]._item.gameObject);
                    _itemsDelete.RemoveAt(i);
                }
            }
        }

        public void AddDeleteItem(Transform objectPlant)
        {
            _itemsDelete.Add(new ItemNeedDelete() { _time = _timeDelete, _item = objectPlant });
        }

    }

}