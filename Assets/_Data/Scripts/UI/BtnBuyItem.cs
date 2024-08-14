using System.Collections;
using System.Collections.Generic;
using CuaHang.Pooler;
using UnityEngine;
using UnityEngine.UI;

namespace CuaHang.UI
{
    public class BtnBuyItem : MonoBehaviour
    {
        public float _count;
        public ItemSO _itemSO;
        public Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(BuyItem);
        }

        public void BuyItem()
        {
            if (!_itemSO)
            {
                Debug.LogWarning("chỗ này thiếu item SO");
                return;
            }

            Debug.Log("BUY ITEM " + _itemSO._name);

            Item parcel = ItemPooler.Instance.GetItemWithTypeID(_itemSO._typeID);

            Debug.Log(parcel);

            if (parcel)
            {
                float size = 2f;
                float rx = Random.Range(-size, size);
                float rz = Random.Range(-size, size);

                Vector3 p = SingleModuleManager.Instance._itemSpawnPos.position;

                parcel.transform.position = new Vector3(p.x + rx, p.y, p.z + rz);
            }
        }
    }
}
