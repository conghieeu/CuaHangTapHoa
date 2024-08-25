
using System.Collections.Generic;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang.AI
{

    public class CustomerStats : ObjectStats
    {
        [Header("ItemStats")]
        [SerializeField] Customer _customer;
        [SerializeField] CustomerData _customerData;

        protected override void Start()
        { 
            _customer = GetComponent<Customer>();
        }

        // Lay du lieu cua chinh cai nay de save
        public CustomerData GetData()
        {
            SaveData();
            return _customerData;
        }

        protected override void SaveData()
        {
            List<ItemData> itemsCard = new();

            foreach (var item in _customer._itemsCard)
            {
                if (item)
                {
                    itemsCard.Add(item._itemStats.GetData());
                }
            }

            _customerData = new CustomerData(
                _customer._ID,
                _customer._typeID,
                _customer._name,
                _customer._totalPay,
                _customer._isNotNeedBuy,
                _customer._playerConfirmPay,
                _customer._isPay,
                _customer.transform.position,
                _customer.transform.rotation,
                itemsCard);
        }

        public override void LoadData<T>(T data)
        {
            _customerData = data as CustomerData;
            if (CustomerPooler.Instance.IsContentID(_customerData._id)) return;

            // set du lieu
            _customer = GetComponent<Customer>();
            _customer.SetProperties(_customerData);
        }

    }

}