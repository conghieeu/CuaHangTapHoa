using System.Collections.Generic;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang
{
    public class MayTinh : Item
    {
        [Header("MayTinh")]
        public ItemSO _objectPlantSO;
        public Transform _spawnTrans;
        public List<Transform> _slotsQueue;
        public List<Transform> _slotsCustomer;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player đã chạm máy tính: Tạo 1 vật phẩm");
                CreateObjectPlant();
            }
        }

        /// <summary> đăng ký slot hàng đợi </summary>
        public void RegisterSlot(Transform customer)
        {
            _slotsCustomer.Add(customer.transform);
        }

        /// <summary> Rời slot hàng đợi </summary>
        public void CancelRegisterSlot(Transform customer)
        {
            _slotsCustomer.Remove(customer);
        }

        /// <summary> Lấy hàng đợi </summary>
        public Transform GetCustomerSlot(Transform customer)
        {
            for (int i = 0; i < _slotsCustomer.Count; i++)
            {
                if (customer == _slotsCustomer[i])
                {
                    return _slotsQueue[i].transform;
                }
            }
            return null;
        }

        // tạo vật thể với SO mới trùng vs SO mẫu nào đó
        [ContextMenu("CreateObjectPlant")]
        void CreateObjectPlant()
        {
            ItemPooler.Instance.CreateObject("parcel_1", null, _spawnTrans);
        }

    }
}