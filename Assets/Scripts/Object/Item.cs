using UnityEngine;
using CuaHang.Pooler;
using System.Collections;

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

        [Header("Item")]
        public bool _isCanDrag = true;
        public bool _isCanSell;
        public Transform _models;
        public BoxCollider _coll;
        public ItemSlot _itemSlot; // Có cái này sẽ là item có khả năng lưu trử các item khác
        [SerializeField] private Transform _thisParent; // là cha của item này 
        
        /// <summary> set vị trí và cha (_thisParent) cho item này </summary>
        public Transform _ThisParent
        {
            get => _thisParent;
            set
            {
                if (value)
                {
                    transform.SetParent(value);
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.identity;
                    _thisParent = value;
                }
                else
                {
                    transform.SetParent(ItemPooler.Instance.transform);
                    _thisParent = null;
                }
            }
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

        /// <summary> Tạo item có thể mua với list item SO </summary>
        IEnumerator LoadSlotSO()
        {
            while (ItemPooler.Instance == null || _itemSlot == null || _itemSlot._itemsSlots.Count == 0)
            {
                yield return null;
            }

            if (_itemSlot)
            {
                for (int i = 0; i < _itemSlot._itemsSlots.Count && i < _SO._items.Count; i++)
                {
                        Log("Khởi tạo item SO từ SO có sẵn " + i);
                    if (_SO._items[i]) _itemSlot.AddItemWithTypeID(_SO._items[i]._typeID);
                }
            }
        }

        /// <summary> Con trỏ gọi vào hàm này để kích hoạt object Temp  </summary>
        public void DragItem()
        {
            // Set Temp 
            ObjectDrag itemDrag = PlayerCtrl.Instance._temp;
            itemDrag.gameObject.SetActive(true);
            itemDrag._itemDragging = this;
            itemDrag.CreateModel(_models);
        }

        /// <summary> Item này có đang tồn tại và là vô chủ hay không </summary>
        public bool IsThisItemFreedom()
        {
            return gameObject.activeSelf == true && _ThisParent == null;
        }

    }
}