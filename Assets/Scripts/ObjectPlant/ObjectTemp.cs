using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public class ObjectTemp : MonoBehaviour
    {
        [Space]
        public Transform _objPlantHolder;
        public Transform _objPlantOnDrag;
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
        [SerializeField] BoxSensor _sensorAround;
        [SerializeField] BoxSensor _sensorGround;

        private void FixedUpdate()
        {
            CheckPlant();
            SetMaterial();
        }

        private void Update()
        {
            PlantObjPlant();
        }

        /// <summary> để model temp đang dragging nó hiện giống model đang di chuyển ở thằng Player </summary>
        public void OnDragging(bool isDragging)
        {
            if (isDragging)
            {
                _objPlantOnDrag.SetParent(PlayerCtrl.Instance._modelTempHolding);
                _objPlantOnDrag.localPosition = Vector3.zero;
                _objPlantOnDrag.localRotation = Quaternion.identity;
            }
            else
            {
                _objPlantOnDrag.transform.position = this.transform.position;
                _objPlantOnDrag.transform.rotation = this.transform.rotation;
                _objPlantOnDrag.GetComponent<ObjectPlant>()._models.transform.rotation = _models.rotation;
                _objPlantOnDrag.SetParent(_objPlantHolder);
                gameObject.SetActive(false);
            }

            _isDragging = isDragging;

            // gọi các nhân viên để nó cập nhập tình huống
            ListStaff.Instance.CallListStaffAIUpdateArrivesTarget();
        }

        private void PlantObjPlant()
        {
            if (Input.GetMouseButtonDown(0) && _canPlant)
            {
                OnDragging(false);
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