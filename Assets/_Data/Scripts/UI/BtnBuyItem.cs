using System.Collections;
using System.Collections.Generic;
using CuaHang.Pooler;
using UnityEngine;
using UnityEngine.UI;

namespace CuaHang.UI
{
    public class BtnBuyItem : MonoBehaviour
    {
        public List<ItemSO> _items;
        public ItemSO _parcel; // nếu khi tạo cần parcel thì bỏ parcel vào đây

        Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(BuyItem);
        }

        public void BuyItem()
        {
            if (_items.Count == 0)
            {
                Debug.LogWarning("btnButton mua này thiếu item SO yêu cầu", transform);
                return;
            }

            if (_parcel)
            {
                Item parcel = ItemPooler.Instance.GetObjectPool(_parcel._typeID).GetComponent<Item>();
                parcel.CreateItemInSlot(_items);
                parcel.SetRandomPos();
            }
            else
            {
                Item item = ItemPooler.Instance.GetObjectPool(_items[0]._typeID).GetComponent<Item>();
                item.SetRandomPos();
            }

        }


    }
}
