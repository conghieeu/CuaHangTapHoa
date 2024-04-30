using System;
using Unity.VisualScripting;
using UnityEngine;

namespace CuaHang.StaffAI
{
    public class StaffAI : AIBehavior
    {

        // [Header("StaffAI")]

        private void FixedUpdate()
        {
            BehaviorCtrl();
            Movement();
        }

        // Khi đáp ứng sự kiện hãy gọi vào đây nên nó đưa phán đoán hành vi tiếp theo nhân viên cần làm
        void BehaviorCtrl()
        {
            // Tìm parcel
            if (!_parcelHolding)
            {
                _objPlantTarget = BoolingObjPlants.Instance.FindObject(true, "parcel_1");
            }

            // Nhặt parcel
            if (_objPlantTarget && !_parcelHolding && GetObjPlantHit())
            {
                if (GetObjPlantHit()._typeID == "parcel_1")
                {
                    PickUpParcel();
                    _objPlantTarget = null;
                }
            }
            // đang có parcel và parcel còn item trong người nên cần tìm cần cái kệ để đặt item
            else if (GetObjPlantWithTypeID("table_1") && IsAnyItemInParcel())
            {
                Debug.Log("Nhân viên tìm kệ còn trống hoặc cái kho để đặt parcel rỗng");
                StateItemToTable();
            }
            // Đặt ObjectPlant vào kho
            else if (!GetObjPlantWithTypeID("table_1") && _parcelHolding && IsAnyItemInParcel())
            {
                StateDropItemToStorage();
            }
            // Đặt ObjectPlant vào thùng rác
            else if (_parcelHolding && !IsAnyItemInParcel())
            {
                StateDropItemToTrash();
            }
        }

        /// <summary> Đặt item vào cái table </summary>
        protected virtual void StateItemToTable()
        {
            Debug.Log("Nhân viên tìm cái bàn");
            // tìm cái bàn 
            if (GetObjPlantWithTypeID("table_1") != _objPlantTarget)
                _objPlantTarget = GetObjPlantWithTypeID("table_1");

            // Tới điểm cần tới
            if (GetObjPlantHit())
            {
                if (GetObjPlantHit()._typeID == "table_1")
                {
                    ObjectPlant parcel = _parcelHolding.GetComponent<ObjectPlant>();
                    ObjectPlant table = _objPlantTarget.GetComponent<ObjectPlant>();
                    SenderItemsToOtherObjectPlant(parcel, table);
                    _objPlantTarget = null;
                }
            }
        }

        /// <summary> Cần tìm đối tượng để AI có thể đạt parcel xuống </summary>
        protected virtual void StateDropItemToTrash()
        {
            Debug.Log("Nhân viên tìm cái thùng rác: " + GetObjPlantWithTypeID("trash_1"));
            // Tìm thùng rác
            if (GetObjPlantWithTypeID("trash_1") != _objPlantTarget)
                _objPlantTarget = GetObjPlantWithTypeID("trash_1");

            // Tới điểm cần tới
            if (GetObjPlantHit())
            {
                Debug.Log("Chạm vào thùng rác rồi");
                Transform dropPos = _objPlantTarget.GetSlotEmpty();
                if (dropPos && GetObjPlantHit()._typeID == "trash_1")
                {
                    _objPlantTarget.AddDeleteItem(_parcelHolding.transform);
                    DropParcel(dropPos);
                    _objPlantTarget = null;
                }
            }
        }

        /// <summary> Đặt item vào kho </summary>
        protected virtual void StateDropItemToStorage()
        {
            Debug.Log("Nhân viên tìm cái kho " + GetObjPlantWithTypeID("storage_1"));
            // Tìm thùng rác
            if (GetObjPlantWithTypeID("storage_1") != _objPlantTarget)
                _objPlantTarget = GetObjPlantWithTypeID("storage_1");

            // Tới điểm cần tới
            if (GetObjPlantHit())
            {
                Debug.Log("Chạm vào cái kho");
                Transform dropPos = _objPlantTarget.GetSlotEmpty();
                if (dropPos)
                {
                    // _objPlantTarget.AddItem
                    DropParcel(dropPos);
                    _objPlantTarget = null;
                }
            }
        }
    }
}