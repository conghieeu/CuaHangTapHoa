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
        [Header("ObjectPlant")]
        public ObjectPlantSO _objPlantSO;
        public Transform _models;
        public Transform _tempPrefab;

        [Header("Value")]
        public string _name;
        public string _type;
        public float _price;
        public string _currency;
        public List<ObjectSellSO> _listItem;
        public List<Transform> _slots;

        private void Start()
        {
            LoadSO();

            LoadItemsSlot();
        }

        public void LoadSO()
        {
            if(_objPlantSO == null) return;

            _name = _objPlantSO._name;
            _type = _objPlantSO._type;
            _price = _objPlantSO._price;
            _currency = _objPlantSO._currency;
            _listItem = _objPlantSO._listItem;
        }

        // tải hình ảnh item từ trong SO lên, đây là việc load dữ liệu lênh nên ko được chỉnh sửa dử liệu
        public void LoadItemsSlot()
        {
            if (_listItem.Count == 0) return;
            for (int i = 0; i < _slots.Count && i < _listItem.Count; i++)
            {
                // tạo đưa vào slot
                if (_listItem[i] && _slots[i].childCount == 0)
                {
                    Instantiate(_listItem[i]._itemPrefabs, _slots[i]);
                }
            }

            // Lấy item ra
            for (int i = _slots.Count - 1; i >= 0; i--)
            {
                if (_listItem[i] == null && _slots[i].childCount > 0)
                {
                    Destroy(_slots[i].GetChild(0).gameObject);
                }
            }
        }

        // đặt object cần đặt vào vị trí
        public void InstantTemp()
        {
            // Set Temp 
            ObjectTemp temp = PlayerCtrl.Instance._temp;
            temp.gameObject.SetActive(true);
            temp._objPlantOnDrag = this.transform;
        }
    }
}