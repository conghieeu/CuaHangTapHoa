using System;
using System.Collections;
using System.Collections.Generic;
using CuaHang.AI;
using UnityEngine;

namespace CuaHang
{
    public class WaitingLine : MonoBehaviour
    {
        [Serializable]
        public class WaitingSlot
        {
            public Customer _customer; // object đang gáng trong boolingObject đó
            public Transform _slot;

            // Constructor với tham số
            public WaitingSlot(Customer customer, Transform slot)
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

        /// <summary> Slot load </summary>
        void LoadSlots()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                WaitingSlot newSlot = new WaitingSlot(null, transform.GetChild(i));
                if (_waitingSlots.Count < transform.childCount) _waitingSlots.Add(newSlot);
            }
        }

        /// <summary> cho khách hàng vào slot trống và đưa thông tin slot cho khách hàng </summary>
        void UpdateSlots()
        {
            // Đẩy khách  vào chỗ trống dư
            for (int i = 0; i < _waitingSlots.Count - 1; i++)
            {
                if (_waitingSlots[i + 1]._customer != null && _waitingSlots[i]._customer == null)
                {
                    _waitingSlots[i]._customer = _waitingSlots[i + 1]._customer;
                    _waitingSlots[i + 1]._customer = null;
                    i = 0;
                }
            }

            // Thông báo slot đợi lại cho khách hàng
            foreach (var s in _waitingSlots)
            {
                if (s._customer) s._customer._slotWaiting = s._slot;
            }
        }

        public bool IsSlotContentCustomer(Customer customer)
        {
            foreach (var slot in _waitingSlots)
            {
                if (slot._customer == customer) return true;
            }
            return false;
        }

        /// <summary> đăng ký slot hàng đợi </summary> 
        public void RegisterSlot(Customer customer)
        {
            if (IsSlotContentCustomer(customer)) return;

            for (int i = 0; i < _waitingSlots.Count; i++)
            {
                if (_waitingSlots[i]._customer == null)
                {
                    _waitingSlots[i]._customer = customer;
                    break;
                }
            }

            UpdateSlots();
        }

        /// <summary> Rời slot hàng đợi </summary>
        public void CancelRegisterSlot(Customer customer)
        {
            foreach (var slot in _waitingSlots)
            {
                if (slot._customer == customer)
                {
                    customer.GetComponent<Customer>()._slotWaiting = null;
                    slot._customer = null;
                    break;
                }
            }

            UpdateSlots();
        }

        /// <summary> Lấy hàng đợi của bản thân khách hàng </summary>
        public Transform GetCustomerSlot(Customer customer)
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