using UnityEngine;
using CuaHang.Pooler;

namespace CuaHang
{
    public class Item : HieuBehavior
    {
        [Header("Settings")]
        public ItemSO _SO; // SO chỉ được load một lần
        public string _ID; // Mỗi đối tượng đc cấp 1 ID khác nhau
        public string _typeID;
        public string _name;
        public string _type;
        public float _price;
        public string _currency;

        [Header("ObjectPlant")]
        public Transform _objFollowedThis; // Đối tượng này có được AI nào đặt là mục tiêu không
        public Transform _thisParent;
        public Transform _models;
        public BoxCollider _coll;
        public ItemSlot _itemSlot;

        private void Awake()
        {
            _coll = GetComponent<BoxCollider>();
            _itemSlot = GetComponentInChildren<ItemSlot>();
        }

        private void Start()
        {
            SetValueSO();
        }

        /// <summary> Set value với SO có đang gáng </summary>
        public void SetValueSO()
        {
            if (_SO == null) return;

            _name = _SO._name;
            _typeID = _SO._typeID;
            _type = _SO._type;
            _price = _SO._price;
            _currency = _SO._currency;

            // Load Slot SO
            if (_itemSlot)
                for (int i = 0; i < _itemSlot._listItem.Count && i < _SO._items.Count; i++)
                {
                    if (_SO._items[i]) _itemSlot.AddItemWithTypeID(_SO._items[i]._typeID);
                }
        }

        /// <returns> set cha của đối tượng này</returns>
        public void SetPosition(Transform parent)
        {
            if (parent)
            {
                transform.SetParent(parent);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                _thisParent = parent;
            }
            else
            {
                transform.SetParent(ItemPooler.Instance.transform);
                _thisParent = null;
            }
        }

        /// <summary> Con trỏ gọi vào hàm này để kích hoạt object Temp  </summary>
        public void DragItem()
        {
            // Set Temp 
            ObjectDrag temp = PlayerCtrl.Instance._temp;
            temp.gameObject.SetActive(true);
            temp._itemDragging = this;
        }

    }
}