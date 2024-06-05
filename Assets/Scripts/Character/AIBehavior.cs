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
        [SerializeField] private Item _itemTarget;  // Parcel mà nhân vật đang hướng tới

        public Item _itemHolding; // Parcel đã nhặt và đang giữ trong người
        public GameManager _gameManager;
        public MayTinh _mayTinh;

        protected ItemPooler _itemPooler;
        protected SensorCast _boxSensor;
        protected NavMeshAgent _navMeshAgent;

        public Item _ItemTarget
        {
            get => _itemTarget;
            set
            {
                _itemTarget = value;
                if (_ItemTarget)
                    if (value)
                    {
                        _ItemTarget._follower = transform;
                    }
                    else
                    {
                        _ItemTarget._follower = null;
                    }
            }
        }

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
        protected virtual Item GetItemHit()
        {
            foreach (var item in _boxSensor._hits)
            {
                if (_ItemTarget)
                    if (item == _ItemTarget.transform)
                    {
                        return item.GetComponent<Item>();
                    }
            }
            return null;
        }

        /// <summary> Di chuyển tới property _ItemTarget </summary>
        protected virtual void MoveToTarget()
        {

            if (_ItemTarget != null)
            {
                _navMeshAgent.SetDestination(_ItemTarget.transform.position);
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