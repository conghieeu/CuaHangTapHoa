using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using Core;
using UnityEngine;

namespace CuaHang
{
    public class CustomerPoolerStats : ObjectStats
    {
        [Header("CUSTOMER POOLER STATS")]
        [SerializeField] CustomerPooler _customerPooler;

        protected override void Start()
        {
            base.Start();
            _customerPooler = GetComponent<CustomerPooler>();
        }

        protected override void SaveData()
        {
            List<CustomerData> cussData = new List<CustomerData>();

            foreach (var pool in _customerPooler._ObjectPools)
            {
                if (pool && pool._ID != "" && pool.gameObject.activeInHierarchy)
                {
                    cussData.Add(pool.GetComponent<CustomerStats>().GetData());
                }
            }

            GetGameData()._customersData = cussData;
        }

        public override void LoadData<T>(T data)
        {
            List<CustomerData> customersData = (data as GameData)._customersData;

            // tái tạo items data
            foreach (var cusData in customersData)
            {
                // ngăn tạo item đã có ID
                if (_customerPooler.IsContentID(cusData._id)) continue;

                // tạo
                ObjectPool customer = _customerPooler.GetObjectPool(cusData._typeID);
                customer.GetComponent<CustomerStats>().LoadData(cusData);
            }

        }


    }
}