using System.Collections;
using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang
{
    public class AutoSpawnCustomer : HieuBehavior
    {
        public List<Customer> _customerPrefabs;
        public List<Transform> _spawnPoint;

        private void Start()
        {
            StartCoroutine(AutoSpawn());
        }

        /// <summary> Luôn Spawn khách hàng ngẫu nhiên </summary>
        IEnumerator AutoSpawn()
        {
            float rTime = UnityEngine.Random.Range(3, 3);
            yield return new WaitForSeconds(rTime);

            if (_customerPrefabs.Count > 0 && _spawnPoint.Count > 0)
            {
                In($"tạo khách hàng");
                int rCustomer = UnityEngine.Random.Range(0, _customerPrefabs.Count - 1);
                int rPoint = UnityEngine.Random.Range(0, _spawnPoint.Count - 1);

                // spawn customer
                Customer cus = CustomerPooler.Instance.GetCustomer(_customerPrefabs[rCustomer]);
                cus.transform.position = _spawnPoint[rPoint].position;
            }

            StartCoroutine(AutoSpawn());
        }
    }

}