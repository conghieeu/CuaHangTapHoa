using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace CuaHang
{
    public class PlayerPlanting : MonoBehaviour
    {
        // public Transform _posHoldParcel; // vị trí đặt cái parcel này trên tay
        public Item _itemHolding; // bưu kiện đang giữ
        PlayerCtrl _ctrl;
        // khi dragging object temp thì thằng này sẽ hiện model của object đang drag nớ ra

        private void Awake()
        {
            _ctrl = GetComponent<PlayerCtrl>();
        }

        private void FixedUpdate()
        {
            if (_ctrl._temp._isDragging) TempAiming();
        }

        private void Update()
        {
            // gửi item trong parcel vào kệ
            if (Input.GetKeyUp(KeyCode.T))
            {
                // SenderItems();
                SenderItemParcelToTable();

            }
        }

        /// <summary> Khi mà drag object Temp thì player sẽ hướng về object Temp </summary>
        void TempAiming()
        {
            var direction = _ctrl._temp.transform.position - transform.position;
            direction.y = 0;
            transform.forward = direction;
        }

        /// <summary> chạm vào kệ, người chơi có thể truyền item từ parcel sang table đó </summary>
        void SenderItemParcelToTable()
        {
            Item receiver = _ctrl._sensor.GetObjectPlantHit();

            if (_itemHolding && receiver)
            {
                // chuyển item
                for (int i = _itemHolding._itemSlot._itemsSlots.Count - 1; i >= 0; i--)
                {
                    Item item = _itemHolding._itemSlot._itemsSlots[i]._item;
                    if (item != null)
                    {
                        receiver._itemSlot.AddItemWithTypeID(item._typeID);
                        _itemHolding._itemSlot.RemoveItemInWorld(item);
                    }
                }
            }
        }


    }
}
