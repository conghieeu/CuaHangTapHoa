using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using HieuDev;
using UnityEngine;

namespace CuaHang 
{
    public class StaffPoolerStats : ObjectStats
    {
        [Header("STAFF POOLER STATS")]
        [SerializeField] StaffPooler _staffPooler; 

        protected override void Start()
        {
            base.Start();
            _staffPooler = GetComponent<StaffPooler>();
        }

        public override void LoadData<T>(T data)
        {
            List<StaffData> staffsData = (data as GameData)._staffsData;

            // tái tạo items data
            foreach (var staffData in staffsData)
            {
                // ngăn tạo item đã có ID
                if (_staffPooler.IsContentID(staffData._id)) continue;

                // tạo
                ObjectPool staff = _staffPooler.GetObjectPool(staffData._typeID);
                staff.GetComponent<StaffStats>().LoadData(staffData);
            }

        }

        protected override void SaveData()
        {
            List<StaffData> staffsData = new List<StaffData>();

            foreach (var pool in _staffPooler._ObjectPools)
            {
                if (pool && pool._ID != "" && pool.gameObject.activeInHierarchy)
                {
                    staffsData.Add(pool.GetComponent<StaffStats>().GetData());
                }
            }

            GetGameData()._staffsData = staffsData;
        }


    }
}
