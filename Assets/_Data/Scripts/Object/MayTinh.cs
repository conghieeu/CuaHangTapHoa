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

        private void Start()
        {
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
            if (other.tag == "Player")
            {
                if (_waitingLine._waitingSlots[0]._customer)
                { 
                    _waitingLine._waitingSlots[0]._customer.GetComponent<Customer>().PlayerConfirmPay();
                }
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

        /// <summary> Đặt lại toạ độ trục Z = 0 để nó khớp với sàn </summary>
        public override void DropItem(Transform location)
        {
            base.DropItem(location);

            for (int i = 0; i < _waitingLine._waitingSlots.Count; i++)
            {
                Vector3 iPos = _waitingLine._waitingSlots[i]._slot.transform.position;
                
                iPos.y = 0;

                _waitingLine._waitingSlots[i]._slot.transform.position = iPos;
            }
        }
    }
}