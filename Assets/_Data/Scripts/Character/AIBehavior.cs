using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CuaHang.Pooler;

namespace CuaHang.AI
{
    public class AIBehavior : HieuBehavior
    {
        [Header("AIBehavior")] 

        public GameManager _gameManager;
        public MayTinh _mayTinh;

        protected ItemPooler _itemPooler;
        protected SensorCast _boxSensor;
        protected NavMeshAgent _navMeshAgent;

        public bool _isToDestination; // đén đích rồi
        public float _distanceMove = 0.5f;

        protected virtual void Awake()
        {
            _boxSensor = GetComponentInChildren<SensorCast>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Start()
        {
            _itemPooler = ItemPooler.Instance;
            _gameManager = GameManager.Instance;
            _mayTinh = GameObject.FindObjectOfType<MayTinh>();
        }

        /// <summary> Di chuyển đến target và trả đúng nếu đến được đích </summary>
        protected virtual bool MoveToTarget(Transform target)
        {
            _navMeshAgent.SetDestination(target.transform.position);

            // Kiểm tra tới được điểm target
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= _distanceMove)
            {
                _isToDestination = true;
                return true;
            }

            _isToDestination = false;
            return false;
        }

    }
}