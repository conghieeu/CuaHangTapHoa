
using System.Collections.Generic;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang
{
    public class ItemStats : ObjectStats
    {
        [Header("ITEM STATS")]
        [SerializeField] Item _item;
        [SerializeField] ItemSlot _itemSlot;
        [SerializeField] ItemData _itemData; 

        protected override void Start()
        {
            _item = GetComponent<Item>();
            _itemSlot = GetComponentInChildren<ItemSlot>();
        }

        // Lay du lieu cua chinh cai nay de save
        public ItemData GetData()
        {
            SaveData();
            return _itemData;
        }

        protected override void SaveData()
        {
            List<ItemData> itemsDataChild = new();

            if (_itemSlot)
            { 
                foreach (var item in _itemSlot._itemsSlots)
                {
                    if (item._item)
                    {
                        itemsDataChild.Add(item._item._itemStats.GetData());
                    }
                }
            }

            _itemData = new ItemData(
                _item._ID,
                _item._typeID,
                _item._price,
                _item.transform.position,
                _item.transform.rotation,
                itemsDataChild);
        }

        public override void LoadData<T>(T data)
        {
            _itemData = data as ItemData;
            if (ItemPooler.Instance.IsContentID(_itemData._id)) return;

            // set du lieu
            _item = GetComponent<Item>();
            _item.SetProperties(_itemData);
        }

        
    }
}
