using System.Collections;
using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using Unity.VisualScripting;
using UnityEngine;

namespace CuaHang
{
    public class AutoSpawnCustomer : HieuBehavior
    {
        public List<Customer> _customerPrefabs;
        public List<Transform> _spawnPoint;

        private void Start()
        {
            StartSpawnCustomer();
        }

        [ContextMenu("StartSpawnCustomer")]
        public void StartSpawnCustomer() => StartCoroutine(AutoSpawn());

        /// <summary> Luôn Spawn khách hàng ngẫu nhiên </summary>
        IEnumerator AutoSpawn()
        {
            float delay = Random.Range(10, 10);
            yield return new WaitForSeconds(delay);

            if (_customerPrefabs.Count > 0 && _spawnPoint.Count > 0)
            {
                In($"tạo khách hàng");
                int rCustomer = Random.Range(0, _customerPrefabs.Count);
                int rPoint = Random.Range(0, _spawnPoint.Count);

                // spawn customer
                Customer cus = CustomerPooler.Instance.GetObjectPool(_customerPrefabs[rCustomer]._typeID).GetComponent<Customer>();
                cus.transform.position = _spawnPoint[rPoint].position;
            }

            StartCoroutine(AutoSpawn());
        }
    }

}