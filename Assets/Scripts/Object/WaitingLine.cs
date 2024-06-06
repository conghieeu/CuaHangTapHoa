using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public class WaitingLine : MonoBehaviour
    {
        [Serializable]
        public class WaitingSlot
        {
            public Transform _customer; // object đang gáng trong boolingObject đó
            public Transform _slot;

            // Constructor với tham số
            public WaitingSlot(Transform customer, Transform slot)
            {
                _customer = customer;
                _slot = slot;
            }
        }

        public List<WaitingSlot> _waitingSlots;

        private void Awake()
        {
            LoadSlots();
        }
 
        private void FixedUpdate()
        {
            PushCustomerNextSlot();
        }

        /// <summary> Slot load </summary>
        void LoadSlots()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                WaitingSlot newSlot = new WaitingSlot(null, transform.GetChild(i));
                if (_waitingSlots.Count < transform.childCount) _waitingSlots.Add(newSlot);
            }
        }

        void PushCustomerNextSlot()
        {
            for (int i = 0; i < _waitingSlots.Count - 1; i++)
            {
                // Đẩy khách hàng sau vào chỗ trống dư
                if (_waitingSlots[i]._customer != null)
                {
                    _waitingSlots[i]._customer = _waitingSlots[i + 1]._customer;
                    _waitingSlots[i + 1]._customer = null;
                }
            }
        }

        public bool IsSlotContentCustomer(Transform customer)
        {
            foreach (var slot in _waitingSlots)
            {
                if (slot._customer == customer) return true;
            }
            return false;
        }

        /// <summary> đăng ký slot hàng đợi </summary> 
        public Transform RegisterSlot(Transform customer)
        {
            if (IsSlotContentCustomer(customer)) return null;

            for (int i = 0; i < _waitingSlots.Count; i++)
            {
                if (_waitingSlots[i]._customer == null)
                {
                    _waitingSlots[i]._customer = customer;
                    return _waitingSlots[i]._slot;
                }
            }
            return null;
        }

        /// <summary> Rời slot hàng đợi </summary>
        public void CancelRegisterSlot(Transform customer)
        {
            foreach (var slot in _waitingSlots)
            {
                if (slot._customer == customer)
                {
                    slot._customer = null;
                }
            }
        }

        /// <summary> Lấy hàng đợi </summary>
        public Transform GetCustomerSlot(Transform customer)
        {
            for (int i = 0; i < _waitingSlots.Count; i++)
            {
                if (customer == _waitingSlots[i]._customer)
                {
                    return _waitingSlots[i]._slot;
                }
            }
            return null;
        }



    }

}