using System;
using Unity.VisualScripting;
using UnityEngine;
using CuaHang.Pooler;
using System.Collections.Generic;

namespace CuaHang.AI
{
    public class Staff : AIBehavior
    {
        public Transform _ItemHoldingPoint; // là vị trí mà nhân viên này đang giữ ObjectPlant trong người 

        public Item _parcelHold; // Parcel đã nhặt và đang giữ trong người

        private void FixedUpdate()
        {
            Behavior();
            Animation();
        }

        /// <summary>  Khi đáp ứng sự kiện hãy gọi vào đây nên nó đưa phán đoán hành vi tiếp theo nhân viên cần làm </summary>
        void Behavior()
        {
            // Find the parcel
            Item parcel = null;
            if (!_parcelHold) parcel = GetParcel();

            // Nhặt parcel
            if (parcel)
            {
                if (MoveToTarget(parcel.transform))
                {
                    parcel.SetParent(_ItemHoldingPoint, null, true);
                    parcel._isCanDrag = false;
                    _parcelHold = parcel;
                    return;
                }
            }

            // Parcel có item không
            bool parcelHasItem = false;
            if (_parcelHold)
            {
                parcelHasItem = _parcelHold._itemSlot.IsAnyItem();
            }

            if (_parcelHold == null) return;

            // Đưa item lênh kệ
            Item shelf = GetItem(TypeID.shelf_1);
            if (shelf && parcelHasItem)
            {
                if (MoveToTarget(shelf._waitingPoint.transform))
                {
                    shelf._itemSlot.ReceiverItems(_parcelHold._itemSlot, true);
                }
                return;
            }

            // Đặt ObjectPlant vào kho
            Item storage = GetItem(TypeID.storage_1);
            if (storage && parcelHasItem)
            {
                if (MoveToTarget(storage.transform))
                {
                    storage._itemSlot.TryAddItemToItemSlot(_parcelHold, true);
                    _parcelHold._isCanDrag = true;
                    _parcelHold = null;
                }
                return;
            }

            // Đặt ObjectPlant vào thùng rác
            Item trashItem = GetItem(TypeID.trash_1);
            Trash trash = trashItem.GetComponent<Trash>();
            if (!parcelHasItem && trash)
            {
                if (MoveToTarget(trash.transform))
                {
                    trash._itemSlot.TryAddItemToItemSlot(_parcelHold, true);
                    trash.AddItemToTrash(_parcelHold);
                    _parcelHold._isCanDrag = true;
                    _parcelHold = null;
                }
                return;
            }
        }

        Item parcel;
        void Animation()
        {
            float velocity = _navMeshAgent.velocity.sqrMagnitude;

            // Idle
            if (velocity == 0 && _stageAnim != STATE_ANIM.Idle || velocity == 0 && parcel != _parcelHold)
            {
                if (_parcelHold) _stageAnim = STATE_ANIM.Idle_Carrying;
                else _stageAnim = STATE_ANIM.Idle;
                parcel = _parcelHold;
                SetAnim();
                return;
            }

            // Walk
            if (velocity > 0.1f && _stageAnim != STATE_ANIM.Walk || velocity > 0.1f && parcel != _parcelHold)
            {
                if (_parcelHold) _stageAnim = STATE_ANIM.Walk_Carrying;
                else _stageAnim = STATE_ANIM.Walk;
                parcel = _parcelHold;
                SetAnim();
                return;
            }

        }

        /// <summary> Tìm item có item Slot và còn chỗ trống </summary>
        Item GetItem(TypeID typeID)
        {
            foreach (var item in ItemPooler.Instance.GetPoolItem)
            {
                if (!item) continue;
                if (!item._itemSlot) continue;
                if (item._typeID == typeID && item._itemSlot.IsHasSlotEmpty()) return item;
            }
            return null;
        }

        /// <summary> Tìm item có thể kéo drag </summary>
        Item GetParcel()
        {
            foreach (var item in ItemPooler.Instance.GetPoolItem)
            {
                if (!item) continue;
                if (item._typeID == TypeID.parcel_1 && !item._thisParent && item.gameObject.activeSelf) return item;
            }

            return null;
        }
    }
}