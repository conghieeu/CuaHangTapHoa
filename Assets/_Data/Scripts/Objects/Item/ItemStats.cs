
using UnityEngine;

namespace CuaHang
{
    public class ItemStats : ObjectStats
    {
        [Header("ITEM STATS")]
        public ItemData _data;
        [SerializeField] Item _item;

        protected override void Start()
        {
            base.Start();
            _item = GetComponent<Item>();
        }

        // ItemPoolerStats sẽ gọi vào đây để Load Data cho Item con
        public void LoadData(ItemData itemData)
        {
            _data = itemData;
             
            _item = GetComponent<Item>();
            _item.SetProperties(itemData);
        }

        public ItemData GetData()
        {
            _data = new ItemData(
                _item._ID,
                _item._typeID,
                _item._price,
                _item.transform.position,
                _item.transform.rotation);

            return _data;
        }

        protected override void SaveData() { }


    }
}