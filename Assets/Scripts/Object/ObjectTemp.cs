using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    /// <summary> ObjectTemp là đối tượng đại diện cho object Plant khi di dời đối tượng </summary>
    public class ObjectTemp : MonoBehaviour
    {
        [Space]
        public ObjectPlant _objectPlantDragging;
        [Space]
        public bool _isDragging;
        public bool _isDistance;
        public bool _canPlant;
        public bool _isCheckGround;
        [Space]
        [SerializeField] String _groundTag = "Ground";
        [SerializeField] Transform _models;
        [SerializeField] Material _green, _red;

        [Space]
        [SerializeField] SensorCast _sensorAround;
        [SerializeField] SensorCast _sensorGround;

        private void FixedUpdate()
        {
            CheckPlant();
            SetMaterial();
        }

        private void Update()
        {
            DropObjectPlant();
        }

        /// <summary> để model temp đang dragging nó hiện giống model đang di chuyển ở thằng Player </summary>
        public void PickUpObjectPlant()
        {
            _objectPlantDragging.SetThisParent(PlayerCtrl.Instance._posHoldParcel);
            _objectPlantDragging._coll.enabled = false;
            _isDragging = true;
        }

        private void DropObjectPlant()
        {
            if (Input.GetMouseButtonDown(0) && _canPlant)
            {
                _objectPlantDragging.transform.position = transform.position;
                _objectPlantDragging.transform.rotation = transform.rotation;
                _objectPlantDragging._models.transform.rotation = _models.rotation;
                _objectPlantDragging.SetThisParent(null);
                _objectPlantDragging._coll.enabled = true;
                _isDragging = false;
                gameObject.SetActive(false);
            }
        }

        private void SetMaterial()
        {
            if (_canPlant)
            {
                _models.GetComponent<Renderer>().material = _green;
            }
            else
            {
                _models.GetComponent<Renderer>().material = _red;
            }
        }

        private void CheckPlant()
        {
            if (_sensorAround._hits.Count == 0 && IsTouchGround() && _isDistance)
            {
                _canPlant = true;
            }
            else
            {
                _canPlant = false;

            }
        }

        bool IsTouchGround()
        {
            if (_isCheckGround == false) return true;

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