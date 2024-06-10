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
            // Find the parcel
            if (!_itemHolding)
            {
                _itemTarget = _itemPooler.FindItemTarget("parcel_1", true, transform);
            }

            bool isParcelHasItem = false;

            if (_itemHolding) isParcelHasItem = _itemHolding._itemSlot.IsAnyItem();

            // Nhặt parcel
            if (_itemTarget && !_itemHolding && IsHitItemTarget())
            {
                PickUpParcel();
            }
            // đang có parcel và parcel còn item trong người nên cần tìm cần cái kệ để đặt item
            else if (FindItemWithTypeID("table_1") && isParcelHasItem)
            { 
                PlaceItemOnTable();
            }
            // Đặt ObjectPlant vào kho
            else if (!FindItemWithTypeID("table_1") && _itemHolding && isParcelHasItem)
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
                _itemHolding._ThisParent = _ItemHoldingPoint;
                _itemTarget = null;
            }
        }

        /// <summary> Đặt item vào cái table </summary>
        protected virtual void PlaceItemOnTable()
        {
            Item target = FindItemWithTypeID("table_1");
            if (_itemTarget != target) _itemTarget = target;

            // Tới được cái bàn chưa
            if (target && IsHitItemTarget()) if (IsHitItemTarget() && target._itemSlot.IsAnyEmptyItem() && _itemHolding != null)
                {
                    target.GetComponent<Item>()._itemSlot.ReceiverItems(_itemHolding.GetComponent<Item>()._itemSlot);
                    _itemTarget = null;
                    return;
                }
        }

        /// <summary> Cần tìm đối tượng để AI có thể đạt parcel xuống </summary>
        protected virtual void PlaceParcelInTrash()
        { 
            Item target = FindItemWithTypeID("trash_1");
            if (_itemTarget != target) _itemTarget = target;

            // Tới điểm cần tới
            if (target && IsHitItemTarget())
                if (IsHitItemTarget() && target._itemSlot.IsAnyEmptyItem() && target.GetComponent<Trash>())
                { 
                    target._itemSlot.AddItemToSlot(_itemHolding);
                    target.GetComponent<Trash>().AddItemToTrash(_itemHolding);

                    _itemTarget = null;
                    _itemHolding = null;
                }
        }

        /// <summary> Đặt item vào kho </summary>
        protected virtual void PlaceParcelInStorage()
        {
            Print("Nhân viên tìm cái kho " + FindItemWithTypeID("storage_1"));
            Item target = FindItemWithTypeID("storage_1");
            if (_itemTarget != target) _itemTarget = target;

            // Tới điểm cần tới
            if (target && IsHitItemTarget()) if (IsHitItemTarget() && target._itemSlot.IsAnyEmptyItem())
                {
                    Debug.Log("Chạm vào cái kho");
                    target._itemSlot.AddItemToSlot(_itemHolding);

                    _itemTarget = null;
                    _itemHolding = null;
                }
        }


    }
}