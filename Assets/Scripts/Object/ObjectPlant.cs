using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

namespace CuaHang
{
    public class ObjectPlant : MonoBehaviour
    {

        [Header("Settings")]
        public ObjectPlantSO _SO; // SO chỉ được load một lần
        public string _ID;
        public string _typeID;
        public string _name;
        public string _type;
        public float _price;
        public string _currency;

        [Header("ObjectPlant")]
        [SerializeField] protected Transform _parent;
        public Transform _models;
        public BoxCollider _coll;

        [Header("Item Slots")]
        public List<ObjectSell> _items; // object đang gáng trong boolingObject đó
        public List<Transform> _slots;

        public Transform _Parent => _parent;

        private void Awake()
        {
            _coll = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            SetValueSO();
            LoadListItemSO();
        }

        public virtual void AddDeleteItem(Transform objectPlant)
        {

        }

        /// <summary> Set value với SO có đang gáng </summary>
        public void SetValueSO()
        {
            if (_SO == null) return;
            _typeID = _SO._typeID;
            _name = _SO._name;
            _type = _SO._type;
            _price = _SO._price;
            _currency = _SO._currency;
        }

        /// <summary> Tải các item đang có trong SO, Khi nhập hàng thì bưu kiện cần phải có item sẵn nên cần hàm này </summary>
        public void LoadListItemSO()
        {
            if (_SO == null) return;

            for (int i = 0; i < _SO._items.Count; i++)
            {
                AddItem(_SO._items[i]._thisPrefabs.GetComponent<ObjectSell>());
            }
        }

        /// <summary> Đối tượng này sẽ theo đối tượng cha đang set </summary>
        public void FollowParent()
        {
            if (_parent == null) return;

            transform.position = _parent.position;
            transform.rotation = _parent.rotation;
        }

        /// <summary> Làm con của thằng cha, đặt lại vị trí </summary>
        /// <returns> set cha của đối tượng này</returns>
        public void SetThisParent(Transform parent)
        {
            _parent = parent;

            if (parent)
            {
                transform.SetParent(parent);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
            else
            {
                transform.SetParent(BoolingObjPlants.Instance.transform);
            }
        }

        /// <summary> Lấy 1 slot đang rỗng </summary>
        public virtual Transform GetSlotEmpty()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_items[i] == null)
                {
                    return _slots[i];
                }
            }
            return null;
        }

        /// <summary> Có slot nào đang trống không </summary>
        public bool IsAnyEmptyItem()
        {
            foreach (var i in _items) if (i == null) return true;
            return false;
        }

        /// <summary> Có item nào có trong slot không </summary>
        public bool IsAnyItem()
        {
            foreach (var i in _items) if (i != null) return true;
            return false;
        }

        /// <summary> Thêm item vào slot và tự load lại object hiển thị </summary>
        public bool AddItem(ObjectSell itemAdd)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                // tạo object đưa vào slot
                if (_items[i] == null)
                {
                    // thêm đối tượng object Sell vào object assign trong slot
                    if (itemAdd)
                    {
                        ObjectSell item = BoolingItems.Instance.GetObject(itemAdd._typeID, _slots[i]);
                        if (item)
                        {
                            _items[i] = item;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary> Xoá item slot và tự load lại object hiển thị </summary>
        public bool RemoveItem(ObjectSell itemRemove)
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (_items[i] == itemRemove && _items[i] != null)
                {
                    // Tìm đối tượng muốn remove trong ao
                    BoolingItems.Instance.RemoveObject(_items[i]);
                    _items[i] = null;
                    return true;
                }
            }
            return false;
        }

        /// <summary> Con trỏ gọi vào hàm này để kích hoạt object Temp  </summary>
        public void ActiveTempState()
        {
            // Set Temp 
            ObjectTemp temp = PlayerCtrl.Instance._temp;
            temp.gameObject.SetActive(true);
            temp._objectPlantDragging = this;
        }
    }
}