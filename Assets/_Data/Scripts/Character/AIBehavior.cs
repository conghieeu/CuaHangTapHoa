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
        public string _name;

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
            _mayTinh = GameObject.FindObjectOfType<MayTinh>();
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
            if (_itemTarget == null) return;

            if (_itemTarget._waitingPoint)
            {
                _navMeshAgent.SetDestination(_itemTarget._waitingPoint.transform.position);
            }
            else
            {
                _navMeshAgent.SetDestination(_itemTarget.transform.position);
            }
        }

        /// <summary> Di chuyển đến target và trả đúng nếu đến được đích </summary>
        protected virtual bool MoveToTarget(Transform target)
        {
            _navMeshAgent.SetDestination(target.transform.position);

            // Kiểm tra tới được điểm target
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= 0.5f)
            {
                return true;
            }

            return false;
        }

    }
}