using System.Collections.Generic;
using Core;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class ItemPoolerStats : ObjectStats
    {
        [Header("ITEM POOLER STATS")]
        [SerializeField] ItemPooler _itemPooler;

        protected override void Start()
        {
            base.Start();
            _itemPooler = GetComponent<ItemPooler>();

        }

        // tạo các root đầu tiên
        public override void LoadData<T>(T data)
        {
            _itemPooler = GetComponent<ItemPooler>();
            List<ItemData> itemsData = (data as GameData)._itemsData;

            // tái tạo items data
            foreach (var itemData in itemsData)
            {
                // ngăn tạo item đã có ID
                if (_itemPooler.IsContentID(itemData._id)) continue;

                // tạo
                ObjectPool item = _itemPooler.GetObjectPool(itemData._typeID);
                item.GetComponent<ItemStats>().LoadData(itemData);
            }
        }

        /// <summary> bắt tính hiệu save </summary>
        protected override void SaveData()
        {
            GetGameData()._itemsData = GetItemsDataRoot();
        }

        /// <summary> Tìm và lọc item từ root data </summary>
        public List<ItemData> GetItemsDataRoot()
        {
            List<ItemData> itemsData = new List<ItemData>();

            foreach (var pool in _itemPooler._ObjectPools)
            {
                if (pool && pool._ID != "" && !pool.GetComponent<Item>()._thisParent && pool.gameObject.activeInHierarchy) // kiểm tra có phải là root
                {
                    itemsData.Add(pool.GetComponent<ItemStats>().GetData());
                }
            }

            return itemsData;
        }


    }
}