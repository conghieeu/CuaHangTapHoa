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
        [SerializeField] protected Item _itemTarget;  // Parcel mà nhân vật đang hướng tới
        [SerializeField] protected Item _itemHolding; // Parcel đã nhặt và đang giữ trong người
        public GameManager _gameManager;
        public MayTinh _mayTinh;

        protected ItemPooler _itemPooler;
        protected SensorCast _boxSensor;
        protected NavMeshAgent _navMeshAgent;

        protected virtual void Awake()
        {
            _boxSensor = GetComponentInChildren<SensorCast>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Start()
        {
            _itemPooler = ItemPooler.Instance;
            _gameManager = GameManager.Instance;
        }

        /// <summary> AI biết nó chạm tới tứ nó cần </summary>
        protected virtual bool IsHitItemTarget()
        {
            if (_itemTarget == null) return false;
            foreach (var item in _boxSensor._hits)
            {
                if (item == _itemTarget.transform)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary> Di chuyển tới property _ItemTarget </summary>
        protected virtual void MoveToTarget()
        { 
            if (_itemTarget != null)
            {
                _navMeshAgent.SetDestination(_itemTarget.transform.position);
            }
        }

        /// <summary> Di chuyển đến target và trả đúng nếu đến được đích </summary>
        protected virtual bool MoveToTarget(Transform target)
        {
            _navMeshAgent.SetDestination(target.transform.position); 

            // Kiểm tra tới được điểm target
            if (!_navMeshAgent.pathPending)
            {
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected virtual Item FindItemWithTypeID(string typeID) => _itemPooler.FindItemWithTypeID(typeID, true);
    }
}