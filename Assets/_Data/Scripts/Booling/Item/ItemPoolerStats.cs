using System.Collections.Generic;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class ItemPoolerStats : ObjectStats
    {
        [Header("ItemPoolerStats")]
        public List<ItemData> _itemsData;
        [SerializeField] ItemPooler _itemPooler;

        protected override void Start()
        {
            base.Start();
            _itemPooler = GetComponent<ItemPooler>();
        }

        /// <summary> Tái sử dụng hoặc tạo ra item mới từ item có sẵn </summary>
        protected override void LoadData(GameData gameData)
        {
            base.LoadData(gameData);
            _itemsData = gameData._itemsData;

            // tái tạo 
            foreach (var item in _itemsData)
            {
                // ngăn tạo item đã có ID
                bool stop = false;
                foreach (var cusPool in _itemPooler._Items)
                {
                    if (item._id == cusPool._ID) stop = true;
                }
                if (stop) continue;

                // tạo item
                Item itemPool = _itemPooler.GetItemWithTypeID(item._typeID);
                if (!itemPool)
                {
                    Debug.LogWarning($"Item {item._typeID} Này Tạo từ pool không thành công");
                    continue;
                }

                itemPool._ID = item._id;
                itemPool._price = item._price;
                itemPool.transform.position = item._position;
                itemPool.transform.rotation = item._rotation;
            }
        }

        protected override void SaveData()
        {
            _itemsData.Clear();

            foreach (var item in _itemPooler._Items)
            {
                if (!item._itemStats && item._ID == "")
                {
                    Debug.LogWarning($"item {item.name} không save được", item.transform);
                    continue;
                }
                ItemData itemData = item._itemStats.GetItemData();
                _itemsData.Add(itemData);
            }

            GetGameData()._itemsData = _itemsData;
        }

    }
}
