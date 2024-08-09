using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    /// <summary> ObjectTemp là đối tượng đại diện cho object Plant khi di dời đối tượng </summary>
    public class ObjectDrag : MonoBehaviour
    {
        [Space]
        public Item _itemDragging;
        [Space]
        public bool _isDragging;
        public bool _isDistance;

        [Space]
        [SerializeField] String _groundTag = "Ground";
        [SerializeField] Transform _models;
        public Transform _modelsHolding;
        [SerializeField] Material _green, _red;

        [Space]
        [SerializeField] SensorCast _sensorAround;
        [SerializeField] SensorCast _sensorGround;

        private void FixedUpdate()
        {
            SetMaterial();
        }

        private void Update()
        {
            ClickToDropItem();
        }

        /// <summary> Tạo model giống otherModel ở vị trí _models</summary>
        public void CreateModel(Transform otherModel)
        {
            _modelsHolding = Instantiate(otherModel, _models, false);
        }

        /// <summary> để model temp đang dragging nó hiện giống model đang di chuyển ở thằng Player </summary>
        public void PickUpItem(Item item)
        {
            // Set Temp
            gameObject.SetActive(true);
            _itemDragging = item;
            CreateModel(item._models);
            _itemDragging.SetParent(PlayerCtrl.Instance._posHoldParcel, null, true);
            _isDragging = true;
        }

        void ClickToDropItem()
        {
            if (Input.GetMouseButtonDown(0) && IsCanPlant() && _itemDragging)
            {
                OnDropItem();
            }
        }

        /// <summary> Huỷ tôi không muốn đặt item nữa </summary>
        public void OnDropItem()
        {
            Destroy(_modelsHolding.gameObject); // Delete model item
            _itemDragging.DropItem(_modelsHolding);
            _itemDragging = null;
            _isDragging = false;
            gameObject.SetActive(false);
        }

        void SetMaterial()
        {
            if (IsCanPlant())
            {
                SetMaterialModel(_green);
            }
            else
            {
                SetMaterialModel(_red);
            }
        }

        void SetMaterialModel(Material color)
        {
            foreach (Renderer model in _models.GetComponentsInChildren<Renderer>())
            {
                model.material = color;
            }
        }

        bool IsCanPlant()
        {
            Debug.Log($"{_sensorAround._hits.Count == 0} && {IsTouchGround()} && { _isDistance}");
            return _sensorAround._hits.Count == 0 && IsTouchGround() && _isDistance;
        }

        bool IsTouchGround()
        {
            // Làm thế nào để cái sensor check ở dưới
            foreach (var obj in _sensorGround._hits)
            {
                if (obj.CompareTag(_groundTag))
                {
                    return true;
                }
            }
            return false;
        }
    }
}