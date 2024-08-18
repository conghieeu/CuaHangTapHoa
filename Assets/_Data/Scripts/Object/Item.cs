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
        [Header("Item")]
        [Header("State Data")]
        public ItemSO _SO; // SO chỉ được load một lần
        public TypeID _typeID;
        public Type _type;
        public string _name;
        [SerializeField] float _price;
        [SerializeField] bool _isBlockPrice;

        [Space]
        public string _interactionPrompt;
        public bool _isCanDrag = true;  // có thằng nhân vật nào đó đang bưng bê cái này
        public bool _isCanSell;
        public bool _isOnEditMode;
        public bool _isSamePrice; // muốn đặt giá tiền các item trong kệ sẽ ngan item
        public Transform _thisParent; // là cha của item này
        public ItemSlot _itemSlot; // Có cái này sẽ là item có khả năng lưu trử các item khác
        public Item _itemParent; // item đang giữ item này

        [Space]
        public Transform _waitingPoint;
        public Transform _models;
        public CamHere _camHere;
        [SerializeField] TextMeshProUGUI _txtPrice;
        [SerializeField] BoxCollider _coll;

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

        void Start()
        {
            // Hiện giá tiền ra UI
            if (_txtPrice) _txtPrice.text = $"{_price.ToString()}c";
        }

        void OnEnable()
        {
            _isCanDrag = true;
            _isRecyclable = false;
        }

        void OnDisable()
        {
            _thisParent = null;
        }

        /// <summary> Set value với SO có đang gáng </summary>
        private void SetValueSO()
        {
            if (_SO == null) return;

            _name = _SO._name;
            _typeID = _SO._typeID;
            _type = _SO._type;
            _price = _SO._priceDefault;
            _isCanSell = _SO._isCanSell;
            _isBlockPrice = _SO._isBlockPrice;
        }

        // =================INTERFACE==================

        /// <summary> dùng cái này cho việc truyền itemSlot </summary>
        public virtual bool Interact(Interactor interactor)
        {
            return true;
        }

        // ==================PUBLIC====================

        /// <summary> Tạo item có thể mua với list item SO </summary>
        public void CreateItemInSlot(List<ItemSO> items)
        {
            for (int i = 0; i < _itemSlot._itemsSlots.Count && i < items.Count; i++)
            {
                if (items[i])
                {
                    Item item = ItemPooler.Instance.GetItemWithTypeID(items[i]._typeID);

                    if (_itemSlot.TryAddItemToItemSlot(item, false) && _isSamePrice)
                    {
                        item._price = _price;
                    }
                }
            }
        }

        public void SetRandomPos()
        {
            float size = 2f;
            float rx = Random.Range(-size, size);
            float rz = Random.Range(-size, size);

            Vector3 p = SingleModuleManager.Instance._itemSpawnPos.position;

            transform.position = new Vector3(p.x + rx, p.y, p.z + rz);
        }

        public void SetPrice(float price)
        {
            if(_isBlockPrice) return;
            
            if (!_SO)
            {
                Debug.LogWarning("Lỗi item này không có ScriptableObject", transform);
                return;
            }

            float newPrice = _price + price;

            if (newPrice > _SO._priceMarketMax) Debug.Log("Cảnh báo bạn đang bị ảo giá");
            if (newPrice < _SO._priceMarketMin) newPrice = _SO._priceMarketMin;
            _price = newPrice;
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