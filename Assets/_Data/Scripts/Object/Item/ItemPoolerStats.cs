using System.Collections.Generic;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class ItemPoolerStats : ObjectStats
    {
        [Header("ItemPoolerStats")]
        [SerializeField] ObjectPooler _objectPooler;
        [SerializeField] List<ItemData> _listData;

        protected override void Start()
        {
            base.Start();
            _objectPooler = GetComponent<ObjectPooler>();
        }

        /// <summary> Tái sử dụng hoặc tạo ra item mới từ item có sẵn </summary>
        protected override void LoadData(GameData gameData)
        {
            base.LoadData(gameData);

            _listData = gameData._itemsData;

            // tái tạo 
            foreach (var data in _listData)
            {
                // ngăn tạo item đã có ID
                if(_objectPooler.IsContentID(data._id)) continue;

                // tạo 
                ObjectPool reObject = _objectPooler.GetObjectPool(data._typeID);
                if (!reObject)
                {
                    Debug.LogWarning($"Item {data._typeID} Này Tạo từ pool không thành công");
                    continue;
                }

                reObject.GetComponent<ItemStats>().LoadData(data);
            }
        }

        protected override void SaveData()
        {
            _listData.Clear();

            foreach (var poolObject in _objectPooler._ObjectPools)
            {
                ItemStats stats = poolObject.GetComponent<ItemStats>();

                if (stats && stats.gameObject.activeInHierarchy)
                {
                    ItemData data = stats.GetData();

                    if (data._id != "") _listData.Add(data); 
                }
            }

            GetGameData()._itemsData = _listData;
        }

    }
}
