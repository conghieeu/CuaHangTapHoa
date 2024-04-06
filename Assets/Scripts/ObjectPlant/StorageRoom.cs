using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public class StorageRoom : ObjectPlant
    {
        [Header("           StorageRoom")]
        public int _maxSlot = 3; // số lượng tối đa mỗi slot mà AI có thể chất hàng lênh

        public virtual Transform GetSlotEmpty()
        {
            return _slots.Find(child => child.childCount < _maxSlot);
        }
    }
}