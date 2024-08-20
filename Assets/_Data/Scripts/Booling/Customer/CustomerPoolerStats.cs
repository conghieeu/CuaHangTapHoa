using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using UnityEngine;

public class CustomerPoolerStats : ObjectStats
{
      [Header("CUSTOMER POOLER STATS")]
        public List<CustomerData> _customers;
        [SerializeField] CustomerPooler _customerPooler;

        protected override void Start()
        {
            base.Start();
            _customerPooler = GetComponent<CustomerPooler>();
        }   

        /// <summary> Tái sử dụng hoặc tạo ra item mới từ item có sẵn </summary>
        protected override void LoadData(GameData gameData)
        {
            base.LoadData(gameData);

            _customers = gameData._customers;

            if (_customers.Count > 0)
            {
                
            }
        }

        protected override void SaveData()
        {  
            // _customers.Clear();

            // foreach (var cus in _customerPooler.GetPoolItem)
            // {   
            //     CustomerData itemData = cus._customerStats.GetItemData();
            //     _customers.Add(itemData);
            // }
 
            // GetGameData()._items = _items;
        }
}