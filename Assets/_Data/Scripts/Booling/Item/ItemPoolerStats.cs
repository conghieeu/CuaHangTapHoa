using System.Collections.Generic;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class ItemPoolerStats : ObjectStats
    {
        [Header("ItemPoolerStats")]
        public List<ItemData> _items;
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

            _items = gameData._items;

            if (_items.Count > 0)
            {
                
            }
        }

        protected override void SaveData()
        {  
            _items.Clear();

            foreach (var item in _itemPooler.GetPoolItem)
            {   
                ItemData itemData = item._itemStats.GetItemData();
                _items.Add(itemData);
            }
 
            GetGameData()._items = _items;
        }

    }
}
