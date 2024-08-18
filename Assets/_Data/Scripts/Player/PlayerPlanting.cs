using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace CuaHang
{
    public class PlayerPlanting : HieuBehavior
    {
        PlayerCtrl _ctrl;

        private void Awake()
        {
            _ctrl = GetComponent<PlayerCtrl>();
        }

        private void FixedUpdate()
        {
            if (_ctrl._objectDrag._isDragging) TempAiming();

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                SenderParcel();
                SenderItemSell();
            }
        }

        /// <summary> chạm vào kệ, người chơi có thể truyền item từ parcel sang table đó </summary>
        void SenderItemSell()
        {
            // có item ở cảm biến
            Item shelf = _ctrl._sensorForward.GetItemTypeHit(Type.Shelf);
            Item itemHold = _ctrl._objectDrag._itemDragging;

            if (shelf && itemHold && !itemHold._isCanSell) // gửi các apple từ bưu kiện sang kệ
            {
                shelf._itemSlot.ReceiverItems(itemHold._itemSlot, false);
            }
            else if (shelf && itemHold && itemHold._isCanSell) // để apple lênh kệ
            {
                In($"Player để quá táo lênh kệ");
                _ctrl._objectDrag.OnDropItem();
                shelf._itemSlot.TryAddItemToItemSlot(itemHold, false);
            }
        }

        /// <summary> Parcel đưa parcel vào thùng rác </summary>
        void SenderParcel()
        {
            Item trash = _ctrl._sensorForward.GetItemTypeHit(Type.Storage);
            Item parcel = _ctrl._objectDrag._itemDragging;

            if (trash && parcel)
            {
                if (parcel._type == Type.Parcel)
                {
                    In($"Player thêm item {parcel} vào trash  {trash}");
                    _ctrl._objectDrag.OnDropItem();
                    trash._itemSlot.TryAddItemToItemSlot(parcel, true);
                }
            }
        }

        /// <summary> Khi mà drag object Temp thì player sẽ hướng về object Temp </summary>
        void TempAiming()
        {
            var direction = _ctrl._objectDrag.transform.position - transform.position;
            direction.y = 0;
            transform.forward = direction;
        }



    }
}
