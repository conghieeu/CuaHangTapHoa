
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
        [SerializeField] List<ItemData> _itemDatasChild = new();

        protected virtual void Start()
        {
            _item = GetComponent<Item>();
            _itemSlot = GetComponentInChildren<ItemSlot>();
        }

        // Lay du lieu cua chinh cai nay de save
        public ItemData GetData()
        {
            // yeu cac nhanh phai luu cac nhan con lai
            SaveData();

            return _itemData;
        }

        protected override void SaveData()
        {
            _itemData = new ItemData(
                _item._ID,
                _item._typeID,
                _item._price,
                _itemDatasChild,
                _item.transform.position,
                _item.transform.rotation);

            if (!_itemSlot) return;

            foreach (var item in _itemSlot._itemsSlots)
            {
                if (item._item)
                {
                    _itemDatasChild.Add(item._item._itemStats.GetData());
                }
            }
        }

        public override void LoadData<T>(T data)
        {
            ItemData itemData = data as ItemData;
            if (ItemPooler.Instance.IsContentID(itemData._id)) return;

            // set du lieu
            _item = GetComponent<Item>();
            _item.SetProperties(itemData);

            // Tai tao cac nhanh
            RecreateItemChild(itemData._itemSlot);
        }

        public void RecreateItemChild(List<ItemData> itemsData)
        {
            ItemSlot itemSlot = GetComponentInChildren<ItemSlot>();

            // tái tạo items data
            foreach (var itemData in itemsData)
            {
                // tạo
                ObjectPool item = ItemPooler.Instance.GetObjectPool(itemData._typeID);

                item.GetComponent<ItemStats>().LoadData(itemData);
                if (itemSlot)
                {
                    itemSlot.TryAddItemToItemSlot(item.GetComponent<Item>(), true);
                }
            }
        }
    }
}
