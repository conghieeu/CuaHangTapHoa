using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang
{
    public class StaffPoolerStats : ObjectStats
    {
        [Header("STAFF POOLER STATS")]
        [SerializeField] ObjectPooler _objectPooler;
        [SerializeField] List<StaffData> _listData;

        protected override void Start()
        {
            base.Start();
            _objectPooler = GetComponent<ObjectPooler>();
        }

        /// <summary> Tái sử dụng hoặc tạo ra item mới từ item có sẵn </summary>
        protected override void LoadData(GameData gameData)
        {
            base.LoadData(gameData);

            _listData = gameData._staffsData;

            // tái tạo 
            foreach (var data in _listData)
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

                reObject.GetComponent<StaffStats>().LoadData(data);
            }
        }

        protected override void SaveData()
        {
            _listData.Clear();

            foreach (var poolObject in _objectPooler._ObjectPools)
            {
                StaffStats stats = poolObject.GetComponent<StaffStats>();

                if (stats && stats.gameObject.activeInHierarchy)
                {
                    StaffData data = stats.GetData();

                    if (data._id != "") _listData.Add(data);
                }
            }

            GetGameData()._staffsData = _listData;
        }
    }
}