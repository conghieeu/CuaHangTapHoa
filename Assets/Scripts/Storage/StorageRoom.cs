using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public class StorageRoom : ObjectPlant
    {
        public Transform _objectPlantHolder;
        public int _maxSlot = 3; // số lượng tối đa mỗi slot mà AI có thể chất hàng lênh

        public Transform GetSlotEmpty()
        {
            return _slots.Find(child => child.childCount < _maxSlot);
        }
    }
}