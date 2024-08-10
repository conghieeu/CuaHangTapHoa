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
        public TypeID _typeID;
        public Type _type;
        public string _name;
        public float _price;
        public string _currency;

        [Header("Item")]
        public bool _isCanDrag = true;  // có thằng nhân vật nào đó đang bưng bê cái này
        public bool _isCanSell;
        public TextMeshProUGUI _txtPrice;
        public Transform _models;
        public ItemSlot _itemSlot; // Có cái này sẽ là item có khả năng lưu trử các item khác
        public Item _itemParent; // item đang giữ item này
        public Transform _thisParent; // là cha của item này

        [Header("Other")]
        public Transform _waitingPoint;

        BoxCollider _coll;

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
            _isCanDrag = true;
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

            // Hiện giá tiền ra UI
            if (_txtPrice) _txtPrice.text = $"{_price.ToString()}c";
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
                        item._price = _price;
                    }
                }
            }

            DropItem(null); // let z pos = 0
        }

        public virtual void DragItem()
        {
            if (_itemParent)
            {
                if (_itemParent._itemSlot)
                {
                    _itemParent._itemSlot.RemoveItemInList(this);
                    _isCanDrag = false;
                }
            }

            _coll.enabled = false;

            if (_itemSlot)
            {
                _itemSlot.SetItemsDrag(false);
            }
        }

        public virtual void DropItem(Transform location)
        {
            SetParent(null, null, true);

            if (location)
            {
                transform.position = location.position;
                transform.rotation = location.rotation;
            }

            _coll.enabled = true;

            if (_itemSlot)
            {
                _itemSlot.SetItemsDrag(true);
            }
        }

    }
}