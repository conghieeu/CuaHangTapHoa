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

        public override bool Interact(Interactor interactor)
        {
            return CreateObjectPlant();
        }

        /// <summary> Đặt lại toạ độ trục Y = 0 để nó khớp với sàn </summary>
        public override void DropItem(Transform location)
        {
            base.DropItem(location);

            // Set Y
            for (int i = 0; i < _waitingLine._waitingSlots.Count; i++)
            {
                Vector3 iPos = _waitingLine._waitingSlots[i]._slot.transform.position;

                iPos.y = 0;

                _waitingLine._waitingSlots[i]._slot.transform.position = iPos;
            }
        }

        // tạo vật thể với SO mới trùng vs SO mẫu nào đó
        private bool CreateObjectPlant()
        {
            Item parcel = ItemPooler.Instance.GetObjectPool(TypeID.parcel_1).GetComponent<Item>();

            if (parcel)
            {
                parcel.transform.position = _spawnTrans.position;
                return true;
            }
            return false;
        }
 

    }
}