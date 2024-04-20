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
                _targetTransform = FindParcel();
            }
            // Nhặt parcel
            if (_targetTransform && !_parcelHolding && GetObjPlantHit())
            {
                if (GetObjPlantHit().GetComponent<ObjectPlant>()._name == "Parcel")
                {
                    PickUpParcel();
                    _parcelHolding = _targetTransform;
                    _targetTransform = null;
                }
            }
            // đang có parcel và parcel còn item trong người nên cần tìm cần cái kệ để đặt item
            else if (FindObjectPlantWithName("Table") && IsItemInParcel())
            {
                Debug.Log("Nhân viên tìm kệ còn trống hoặc cái kho để đặt parcel rỗng");
                StateItemToTable();
            }
            // Đặt parcel vào kho hoặc thùng rác
            else if (!FindObjectPlantWithName("Table") || !IsItemInParcel())
            {
                Debug.Log("Tìm thùng rác hoặc kho để đặt parcel rỗng");
                StateDropParcel();
            }
        }




    }
}