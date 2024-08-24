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
        [SerializeField] List<StaffData> _listData;


        protected virtual void Start()
        {
            _staffPooler = GetComponent<StaffPooler>();

        }

        public override void LoadData<T>(T data)
        {
            throw new System.NotImplementedException();
        }

        protected override void SaveData()
        {
            throw new System.NotImplementedException();
        }


    }
}
