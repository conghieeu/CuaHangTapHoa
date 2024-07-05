using System;
using Unity.VisualScripting;
using UnityEngine;
using CuaHang.Pooler;

namespace CuaHang.AI
{
    public class Staff : AIBehavior
    {
        public Transform _ItemHoldingPoint; // là vị trí mà nhân viên này đang giữ ObjectPlant trong người

        private void FixedUpdate()
        {
            BehaviorCtrl();
            MoveToTarget();
        }

        /// <summary>  Khi đáp ứng sự kiện hãy gọi vào đây nên nó đưa phán đoán hành vi tiếp theo nhân viên cần làm </summary>
        void BehaviorCtrl()
        {
            In("Tìm cái bàn " + _itemPooler.FindItemWithTypeID(TypeID.table_1));

            // Find the parcel
            if (!_itemHolding)
            {
                _itemTarget = _itemPooler.FindItemTarget(TypeID.parcel_1, true, transform);
            }

            bool isParcelHasItem = false;

            if (_itemHolding) isParcelHasItem = _itemHolding._itemSlot.IsAnyItem();

            // Nhặt parcel
            if (_itemTarget && !_itemHolding && IsHitItemTarget())
            {
                In("PickUpParcel");
                PickUpParcel();
            }
            // đang có parcel và parcel còn item trong người nên cần tìm cần cái kệ để đặt item
            else if (_itemPooler.FindItemWithTypeID(TypeID.table_1) && isParcelHasItem)
            {
                In("PlaceItemOnTable");
                PlaceItemOnTable();
            }
            // Đặt ObjectPlant vào kho
            else if (!_itemPooler.FindItemWithTypeID(TypeID.table_1) && _itemHolding && isParcelHasItem)
            {

                PlaceParcelInStorage();
            }
            // Đặt ObjectPlant vào thùng rác
            else if (_itemHolding && !isParcelHasItem)
            {
                PlaceParcelInTrash();
            }
        }

        private void PickUpParcel()
        {
            if (IsHitItemTarget() == _itemTarget)
            {
                _itemHolding = _itemTarget;
                _itemHolding.SetParent(_ItemHoldingPoint, null, true);
                _itemTarget = null;
            }
        }

        /// <summary> Đặt item vào cái table </summary>
        protected virtual void PlaceItemOnTable()
        {
            Item target = _itemPooler.FindItemWithTypeID(TypeID.table_1); // tìm bàn còn trống

            if (_itemTarget != target) _itemTarget = target;

            // Tới được cái bàn chưa
            if (_itemHolding && IsHitItemTarget())
                if (target._itemSlot.IsHasSlotEmpty())
                {
                    target._itemSlot.ReceiverItems(_itemHolding._itemSlot, false);
                    _itemTarget = null;
                    return;
                }
        }

        /// <summary> Cần tìm đối tượng để AI có thể đạt parcel xuống </summary>
        protected virtual void PlaceParcelInTrash()
        {
            Item trash = _itemPooler.FindItemWithTypeID(TypeID.trash_1);

            if (_itemTarget != trash) _itemTarget = trash;

            // Tới điểm cần tới
            if (trash && IsHitItemTarget())
                if (IsHitItemTarget() && trash._itemSlot.IsHasSlotEmpty() && trash.GetComponent<Trash>())
                {
                    trash._itemSlot.TryAddItemToItemSlot(_itemHolding, false);
                    trash.GetComponent<Trash>().AddItemToTrash(_itemHolding);

                    _itemTarget = null;
                    _itemHolding = null;
                }
        }

        /// <summary> Đặt item vào kho </summary>
        protected virtual void PlaceParcelInStorage()
        {
            Item storage = _itemPooler.FindItemWithTypeID(TypeID.storage_1); // tìm kho trống

            if (_itemTarget != storage) _itemTarget = storage;

            // Tới điểm cần tới
            if (storage && IsHitItemTarget()) if (IsHitItemTarget() && storage._itemSlot.IsHasSlotEmpty())
                {
                    storage._itemSlot.TryAddItemToItemSlot(_itemHolding, false);

                    _itemTarget = null;
                    _itemHolding = null;
                }
        }


    }
}