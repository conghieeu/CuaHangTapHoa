using System;
using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang
{
    public class MayTinh : Item
    {

        [Header("MayTinh")]
        public ItemSO _objectPlantSO;
        public Transform _spawnTrans;
        public WaitingLine _waitingLine;

        protected override void Awake()
        {
            base.Awake();
            _waitingLine = GetComponentInChildren<WaitingLine>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                CreateObjectPlant(); In($"Tạo cái bưu kiện");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (_waitingLine._waitingSlots[0]._customer)
                    _waitingLine._waitingSlots[0]._customer.GetComponent<Customer>().PlayerConfirmPay();
            }
        }

        // tạo vật thể với SO mới trùng vs SO mẫu nào đó
        void CreateObjectPlant()
        {
            Item parcel = ItemPooler.Instance.GetItemWithTypeID(TypeID.parcel_1);

            if (parcel)
            {
                parcel.transform.position = _spawnTrans.position;
            }
        }
    }
}