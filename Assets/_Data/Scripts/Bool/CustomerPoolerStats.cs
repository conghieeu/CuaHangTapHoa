using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang
{
    public class CustomerPoolerStats : ObjectStats
    {
        [Header("CUSTOMER POOLER STATS")]
        [SerializeField] ObjectPooler _objectPooler;
        public List<CustomerData> _customersData;

        private void Start()
        {
            _objectPooler = GetComponent<ObjectPooler>();
        }

        /// <summary> Tìm và lọc item từ root data </summary>
        public List<CustomerData> GetRootCustomerData()
        {
            List<CustomerData> data = new List<CustomerData>();

            return data;
        }

        protected override void SaveData()
        {
            _customersData.Clear();

            foreach (var poolObject in _objectPooler._ObjectPools) {
                CustomerStats stats = poolObject.GetComponent<CustomerStats>();
                if (stats && stats.gameObject.activeInHierarchy)
                {
                    CustomerData data = stats.GetData();
                    if (data._id != "") _customersData.Add(data);
                }
            }

            GetGameData()._customersData = _customersData;
        }

        public override void LoadData<T>(T data)
        {


        }


    }
}