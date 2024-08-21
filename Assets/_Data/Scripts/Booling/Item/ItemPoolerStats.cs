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

            if (_itemsData.Count > 0)
            { 
                // tạo lại item từ save file
                foreach (var item in _itemsData)
                {
                    Item itemCre = _itemPooler.GetItemWithTypeID(item._typeID);
                    if (!itemCre)
                    {
                        Debug.LogWarning($"Item {item._typeID} Này Tạo từ pool không thành công");
                        continue;
                    }
                    itemCre._ID = item._id;
                    itemCre._price = item._price;
                    itemCre.transform.position = item._position;
                    itemCre.transform.rotation = item._rotation;
                }
            }
        }

        protected override void SaveData()
        {
            _itemsData.Clear();

            foreach (var item in _itemPooler.GetPoolItem)
            {
                if (item._itemStats == null)
                {
                    Debug.LogWarning($"item {item.name} này không có stats", item.transform);
                    continue;
                }
                ItemData itemData = item._itemStats.GetItemData();
                _itemsData.Add(itemData);
            }

            GetGameData()._itemsData = _itemsData;
        }

    }
}
