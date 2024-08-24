using System.Collections.Generic;
using HieuDev;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class ItemPoolerStats : ObjectStats
    {
        [Header("ITEM POOLER STATS")]
        [SerializeField] ItemPooler _itemPooler;

        private void Start()
        {
            _itemPooler = GetComponent<ItemPooler>(); 

            // load
            SerializationAndEncryption._OnDataLoaded += gameData =>
            {
                if (this != null && transform != null)
                {
                    LoadData(gameData._itemsData);
                }
            };

            // save
            SerializationAndEncryption._OnDataSaved += () =>
            {
                if (this != null && transform != null)
                {
                    SaveData();
                }
            };
        }

        // tạo các root đầu tiên
        public override void LoadData<T>(T data)
        {
            List<ItemData> itemsData = data as List<ItemData>;

            // tái tạo items data
            foreach (var itemData in itemsData)
            {
                // ngăn tạo item đã có ID
                if (_itemPooler.IsContentID(itemData._id)) continue;

                // tạo
                ObjectPool reObject = _itemPooler.GetObjectPool(itemData._typeID);
                reObject.GetComponent<ItemStats>().LoadData(itemData);
            }
        }

        protected override void SaveData()
        { 
            GetGameData()._itemsData = GetItemsDataRoot();
        }

        /// <summary> Tìm và lọc item từ root data </summary>
        public List<ItemData> GetItemsDataRoot()
        {
            List<ItemData> data = new List<ItemData>();

            foreach (var pool in _itemPooler._ObjectPools)
            {
                if (pool && pool._ID != "" && !pool.GetComponent<Item>()._thisParent) // kiểm tra có phải là root
                {
                    data.Add(pool.GetComponent<ItemStats>().GetData());
                }
            }

            return data;
        }


    }
}