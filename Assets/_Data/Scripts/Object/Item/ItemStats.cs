 
using UnityEngine;

namespace CuaHang
{
    public class ItemStats : ObjectStats
    {
        [Header("ITEM STATS")]
        public ItemData _itemData;
        [SerializeField] Item _item;

        protected override void Start()
        {
            base.Start();
            _item = GetComponent<Item>();
        }

        // ItemPoolerStats sẽ gọi vào đây để Load Data cho Item con
        public void LoadData(ItemData itemData)
        {
            _itemData = itemData;
            _item.SetProperties(itemData);
        }

        public ItemData GetItemData()
        { 
            _itemData = new ItemData(_item._ID, _item._price, _item._typeID, _item.transform.position, _item.transform.rotation);
            return _itemData;
        }

        protected override void SaveData() { }


    }
}