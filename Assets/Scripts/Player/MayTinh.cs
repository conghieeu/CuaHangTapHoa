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

        private void Awake()
        {
            _waitingLine = GetComponentInChildren<WaitingLine>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player đã chạm máy tính: Tạo 1 vật phẩm");
                CreateObjectPlant();

                if (_waitingLine._waitingSlots[0]._customer)
                    _waitingLine._waitingSlots[0]._customer.GetComponent<Customer>().SetPlayerConfirmPay();
            }
        }

        // tạo vật thể với SO mới trùng vs SO mẫu nào đó
        [ContextMenu("CreateObjectPlant")]
        void CreateObjectPlant()
        {
            ItemPooler.Instance.CreateItem("parcel_1", null, _spawnTrans);
        }
    }
}