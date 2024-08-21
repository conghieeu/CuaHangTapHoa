using System.Collections;
using System.Collections.Generic;
using CuaHang.AI;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class StaffPooler : ObjectPooler
    {
        [Header("STAFF POOLER")]
        public List<Staff> _listStaff;

        public List<Staff> GetPoolItem { get => _listStaff; }
        public static StaffPooler Instance { get; private set; }

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }
        }

        public Staff GetCustomer(Staff cusPrefab)
        {
            Staff cus = GetHider(cusPrefab.name);

            if (cus)
            {
                cus.gameObject.SetActive(true);
            }
            else // Create New 
            {
                foreach (var objP in _listPrefab)
                {
                    Staff newCus = objP.GetComponent<Staff>();
                    if (newCus)
                    {
                        if (newCus.name == cusPrefab.name)
                        {
                            cus = Instantiate(newCus, transform); // create
                            _listStaff.Add(cus); // Add list
                            break;
                        }
                    }
                }
            }

            return cus;
        }

        /// <summary> Lấy customer đang ẩn </summary>
        private Staff GetHider(string name)
        {
            foreach (var cus in _listStaff)
            {
                if (cus.name == name && !cus.gameObject.activeSelf && cus._isRecyclable) return cus;
            }
            return null;
        }
    }
}
