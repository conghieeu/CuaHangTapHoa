using System.Collections;
using System.Collections.Generic;
using CuaHang.AI;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class CustomerPooler : ObjectPooler
    {
        public List<Customer> _listCustomer;

        public static CustomerPooler Instance { get; private set; }

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }
        }

        public Customer GetCustomer(Customer cusPrefab)
        {
            Customer cus = GetHider(cusPrefab.name);

            if (cus)
            {
                cus.gameObject.SetActive(true);
            }
            else // Create New 
            {
                foreach (var objP in _listPrefab)
                {
                    Customer newCus = objP.GetComponent<Customer>();
                    if (newCus)
                    {
                        if (newCus.name == cusPrefab.name)
                        {
                            cus = Instantiate(newCus, transform); // create
                            _listCustomer.Add(cus); // Add list
                            break;
                        }
                    }
                }
            }

            return cus;
        }

        /// <summary> Lấy customer đang ẩn </summary>
        private Customer GetHider(string name)
        {
            foreach (var cus in _listCustomer)
            {
                if (cus.name == name && !cus.gameObject.activeSelf) return cus;
            }
            return null;
        }


    }
}