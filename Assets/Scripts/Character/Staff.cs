using System;
using Unity.VisualScripting;
using UnityEngine;
using CuaHang.Pooler;

namespace CuaHang.AI
{
    public class Staff : AIBehavior
    {
        public Transform _itemHoldingPoint; // là vị trí mà nhân viên này đang giữ ObjectPlant trong người
        
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
                _ItemTarget = _itemPooler.FindItemTarget("parcel_1", true, transform);
            }

            bool isParcelHasItem = false;

            if (_itemHolding) isParcelHasItem = _itemHolding._itemSlot.IsAnyItem();

            // Debug.Log("Nhân viên chạm vào " + GetItemHit(), transform);
            // Nhặt parcel
            if (_ItemTarget && !_itemHolding && GetItemHit())
            {
                PickUpParcel();
            }
            // đang có parcel và parcel còn item trong người nên cần tìm cần cái kệ để đặt item
            else if (FindItemWithTypeID("table_1") && isParcelHasItem)
            {
                Debug.Log("Nhân viên tìm kệ còn trống hoặc cái kho để đặt parcel rỗng");
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
            if (GetItemHit() == _ItemTarget)
            {
                _itemHolding = _ItemTarget;
                _itemHolding.SetThisParent(_itemHoldingPoint);
                _ItemTarget = null;
            }
        }

        /// <summary> Đặt item vào cái table </summary>
        protected virtual void PlaceItemOnTable()
        {
            Item target = FindItemWithTypeID("table_1");
            if (_ItemTarget != target) _ItemTarget = target;

            // Tới được cái bàn chưa
            if (target && GetItemHit()) if (GetItemHit().transform == target.transform && target._itemSlot.IsAnyEmptyItem() && _itemHolding != null)
                {
                    target.GetComponent<Item>()._itemSlot.ReceiverItems(_itemHolding.GetComponent<Item>()._itemSlot);
                    _ItemTarget = null;
                    return;
                }
        }

        /// <summary> Cần tìm đối tượng để AI có thể đạt parcel xuống </summary>
        protected virtual void PlaceParcelInTrash()
        {
            Debug.Log("Nhân viên tìm cái thùng rác: " + FindItemWithTypeID("trash_1"));
            Item target = FindItemWithTypeID("trash_1");
            if (_ItemTarget != target) _ItemTarget = target;

            // Tới điểm cần tới
            if (target && GetItemHit())
                if (GetItemHit().transform == target.transform && target._itemSlot.IsAnyEmptyItem() && target.GetComponent<Trash>())
                {
                    Debug.Log("Chạm vào cái thùng rác");
                    target._itemSlot.AddItemToSlot(_itemHolding);
                    target.GetComponent<Trash>().AddItemToTrash(_itemHolding);

                    _itemHolding = null;
                    _ItemTarget = null;
                }
        }

        /// <summary> Đặt item vào kho </summary>
        protected virtual void PlaceParcelInStorage()
        {
            Debug.Log("Nhân viên tìm cái kho " + FindItemWithTypeID("storage_1"));
            Item target = FindItemWithTypeID("storage_1");
            if (_ItemTarget != target) _ItemTarget = target;

            // Tới điểm cần tới
            if (target && GetItemHit()) if (GetItemHit().transform == target.transform && target._itemSlot.IsAnyEmptyItem())
                {
                    Debug.Log("Chạm vào cái kho");
                    target._itemSlot.AddItemToSlot(_itemHolding);

                    _itemHolding = null;
                    _ItemTarget = null;
                }
        }

        
    }
}