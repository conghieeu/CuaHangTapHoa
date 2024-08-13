using UnityEngine;
using CuaHang.Pooler;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;

namespace CuaHang
{
    public class Item : PoolObject, IInteractable
    {
        [Header("Property")]
        public ItemSO _SO; // SO chỉ được load một lần
        public TypeID _typeID;
        public Type _type;
        public string _name;
        [SerializeField] float _price;
        public string _currency;


        [Space]
        public string _interactionPrompt;
        public bool _isCanDrag = true;  // có thằng nhân vật nào đó đang bưng bê cái này
        public bool _isCanSell;
        public bool _isOnEditMode;
        public Transform _thisParent; // là cha của item này
        public ItemSlot _itemSlot; // Có cái này sẽ là item có khả năng lưu trử các item khác
        public Item _itemParent; // item đang giữ item này

        [Space]
        public Transform _waitingPoint;
        public Transform _models;
        public CamHere _camHere;
        public TextMeshProUGUI _txtPrice;

        BoxCollider _coll;

        public float _Price { get => _price; }

        public string InteractionPrompt => _interactionPrompt;

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
            _camHere = GetComponentInChildren<CamHere>();
            SetValueSO();
        }

        private void OnEnable()
        {
            StartCoroutine(LoadSlotSO());
            _isCanDrag = true;
            _isRecyclable = false;
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

        // =================INTERFACE==================
        
        /// <summary> dùng cái này cho việc truyền itemSlot </summary>
        public virtual bool Interact(Interactor interactor)
        {
            return true;
        }

        // ==================PUBLIC====================

        public void SetPrice(float price)
        {
            if (!_SO)
            {
                Debug.LogWarning("Lỗi item này không có ScriptableObject", transform);
                return;
            }

            float newPrice = _price + price;


            if (newPrice <= _SO._priceMarketMax && newPrice >= _SO._priceMarketMin)
            {
                _price = newPrice;
            }
        }

        public void SetEditMode(bool enable)
        {
            if (enable)
            {
                _camHere.SetCamFocusHere();
                _coll.enabled = false;
                _isOnEditMode = true;
            }
            else
            {

                _coll.enabled = true;
                _isOnEditMode = false;
            }
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