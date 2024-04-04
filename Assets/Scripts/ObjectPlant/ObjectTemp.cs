using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public class ObjectTemp : MonoBehaviour
    {
        [SerializeField] ObjectPlantSO _objectPlantSO;
        
        [Space]
        [SerializeField] Transform _objectPlant;
        [SerializeField] Transform _objectContactHolder;
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
        public ObjectPlantSO _ObjectContactDisplay { get => _objectPlantSO; set => _objectPlantSO = value; }

        private void Awake()
        {
            
        }

        private void Update()
        {
            
            PlantPrefab();
        }

        private void FixedUpdate()
        {

            CheckPlant();
            SetMaterial();
        }

        private void PlantPrefab()
        {
            if (Input.GetMouseButtonDown(0) && _canPlant)
            {
                gameObject.SetActive(false);

                ObjectPlant o = Instantiate(_objectPlant, this.transform.position, this.transform.rotation, _objectContactHolder).GetComponent<ObjectPlant>();

                o._models.transform.rotation = _models.rotation;
                o._objectInfoSO = _objectPlantSO;

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
            if (_sensorAround._Hits.Count == 0 && IsTouchGround() && _isDistance)
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
            foreach (var obj in _sensorGround._Hits)
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