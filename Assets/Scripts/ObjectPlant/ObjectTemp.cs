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
        [SerializeField] bool _canPlant;
        [SerializeField] bool _isDistance;
        [SerializeField] bool _isCheckGround;
        [Space]
        [SerializeField] String _groundTag = "Ground";
        [SerializeField] Transform _models;
        [SerializeField] Material _green, _red;
        [Space]
        [SerializeField] BoxSensor _sensorAround;
        [SerializeField] BoxSensor _sensorGround;

        public bool _IsDistance { get => _isDistance; set => _isDistance = value; }

        private void FixedUpdate()
        {
            CheckPlant();
            SetMaterial();
        }

        private void Update()
        {
            PlantPrefab();
        }

        // TODO: Làm sao để model temp đang dragging nó hiện giống model đang di chuyển


        private void PlantPrefab()
        {
            if (Input.GetMouseButtonDown(0) && _canPlant)
            {
                _objPlantOnDrag.transform.position = this.transform.position;
                _objPlantOnDrag.transform.rotation = this.transform.rotation;
                _objPlantOnDrag.GetComponent<ObjectPlant>()._models.transform.rotation = _models.rotation;
                _objPlantOnDrag.SetParent(_objPlantHolder);
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
            if (_sensorAround.GetHits().Count == 0 && IsTouchGround() && _isDistance)
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
            foreach (var obj in _sensorGround.GetHits())
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