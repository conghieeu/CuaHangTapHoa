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

        protected override void Start()
        {
            base.Start();
            _objectPooler = GetComponent<ObjectPooler>();
        }

        /// <summary> Tái sử dụng hoặc tạo ra item mới từ item có sẵn </summary>
        protected override void LoadData(GameData gameData)
        {
            base.LoadData(gameData);

            _customersData = gameData._customersData;

            // tái tạo 
            foreach (var data in _customersData)
            {
                // ngăn tạo item đã có ID
                if (_objectPooler.IsContentID(data._id)) continue;

                // tạo 
                ObjectPool reObject = _objectPooler.GetObjectPool(data._typeID);
                if (!reObject)
                {
                    Debug.LogWarning($"Item {data._name} Này Tạo từ pool không thành công");
                    continue;
                }

                reObject.GetComponent<CustomerStats>().LoadData(data);
            }
        }

        protected override void SaveData()
        {
            _customersData.Clear();

            foreach (var poolObject in _objectPooler._ObjectPools)
            {
                CustomerStats stats = poolObject.GetComponent<CustomerStats>();

                if (stats && stats.gameObject.activeInHierarchy)
                {
                    CustomerData data = stats.GetData();

                    if (data._id != "") _customersData.Add(data);
                }
            }

            GetGameData()._customersData = _customersData;
        }
    }
}