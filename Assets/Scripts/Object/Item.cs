using UnityEngine;
using CuaHang.Pooler;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace CuaHang
{
    public class Item : HieuBehavior
    {
        [Header("Settings")]
        public ItemSO _SO; // SO chỉ được load một lần
        public string _ID; // Mỗi đối tượng đc cấp 1 ID khác nhau
        public TypeID _typeID;
        public string _name;
        public Type _type;
        public float _price;
        public string _currency;

        [Header("Item")]
        public bool _isCanDrag = true;  // có thằng nhân vật nào đó đang bưng bê cái này
        public bool _isCanSell;
        public TextMeshProUGUI _txtPrice;
        public Transform _models;
        public BoxCollider _coll;
        public ItemSlot _itemSlot; // Có cái này sẽ là item có khả năng lưu trử các item khác
        public Item _itemParent; // item đang giữ item này
        public Transform _thisParent; // là cha của item này

        public void SetParent(Transform thisParent, Item itemParent, bool isCanDrag)
        {
            if (thisParent)
            {
                transform.SetParent(thisParent);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
            else
            {
                transform.SetParent(ItemPooler.Instance.transform);
            }

            _thisParent = thisParent;
            _itemParent = itemParent;
            _isCanDrag = isCanDrag;
        }

        protected virtual void Awake()
        {
            _coll = GetComponent<BoxCollider>();
            _itemSlot = GetComponentInChildren<ItemSlot>();
            SetValueSO();
        }

        private void OnEnable()
        {
            StartCoroutine(LoadSlotSO());
        }

        private void OnDisable()
        {
            _thisParent = null;
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
            _isCanSell = _SO._isCanSell;
        }

        public void SetPrice(float value)
        {
            _price = value;
            if (_txtPrice) _txtPrice.text = $"{value.ToString()}c";
        }

        /// <summary> Tạo item có thể mua với list item SO </summary>
        IEnumerator LoadSlotSO()
        {
            while (ItemPooler.Instance == null || _itemSlot == null || _itemSlot._itemsSlots.Count == 0)
            {
                yield return null;
            }

            for (int i = 0; i < _itemSlot._itemsSlots.Count && i < _SO._items.Count; i++)
            {
                In("Khởi tạo item SO từ SO có sẵn " + i);
                if (_SO._items[i])
                {
                    Item item = ItemPooler.Instance.GetItemWithTypeID(_SO._items[i]._typeID);

                    if (_itemSlot.TryAddItemToItemSlot(item, false))
                    {
                        item.SetPrice(_price);
                    }
                }
            }
        }

        public void DragItem()
        {
            if (_itemParent)
            {
                if (_itemParent._itemSlot)
                {
                    _itemParent._itemSlot.RemoveItemInList(this);
                    _isCanDrag = false;
                }
            }

            if (_itemSlot)
            {
                _itemSlot.SetItemsDrag(false);
            }
        }

        public void DropItem(Transform location)
        {
            SetParent(null, null, true);
            transform.position = location.position;
            transform.rotation = location.rotation;

            if (_itemSlot)
            {
                _itemSlot.SetItemsDrag(true);
            }
        }



    }
}