using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang
{

    public class CustomerPoolerStats : ObjectStats
    {
        [Header("CUSTOMER POOLER STATS")]
        public List<CustomerData> _customersData;
        [SerializeField] CustomerPooler _customerPooler;

        protected override void Start()
        {
            base.Start();
            _customerPooler = GetComponent<CustomerPooler>();
        }

        /// <summary> Tái sử dụng hoặc tạo ra item mới từ item có sẵn </summary>
        protected override void LoadData(GameData gameData)
        {
            base.LoadData(gameData);

            _customersData = gameData._customers;

            // tái tạo 
            foreach (var cus in _customersData)
            {
                // ngăn tái tạo đối tượng trùng ID
                bool stop = false;
                foreach (var cusPool in _customerPooler._listCustomer)
                {
                    if (cus._id == cusPool._ID) stop = true;
                }
                if(stop) continue;

                // Tái tạo đối tượng
                Customer cusCreated = _customerPooler.GetCustomer(cus._name);
                if (!cusCreated)
                {
                    Debug.LogWarning($"Item {cus._name} Này Tạo từ pool không thành công");
                    continue;
                }

                cusCreated._ID = cus._id;
                cusCreated._name = cus._name;
                cusCreated._isNotNeedBuy = cus._isNotNeedBuy;
                cusCreated._isPay = cus._isPay;
                cusCreated._playerConfirmPay = cus._playerConfirmPay;
                cusCreated._totalPay = cus._totalPay;
                cusCreated.transform.position = cus._position;
                cusCreated.transform.rotation = cus._rotation;
            }
        }

        protected override void SaveData()
        {
            _customersData.Clear();

            foreach (var cus in _customerPooler.GetPoolItem)
            {
                if (cus.gameObject.activeInHierarchy)
                {
                    CustomerData customerData = cus._customerStats.GetCustomerData();
                    _customersData.Add(customerData);
                }
            }

            GetGameData()._customers = _customersData;
        }
    }
}