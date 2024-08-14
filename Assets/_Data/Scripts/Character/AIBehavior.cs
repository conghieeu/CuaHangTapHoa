using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CuaHang.Pooler;

namespace CuaHang.AI
{
    [Serializable]
    public enum STATE_ANIM
    {
        Idle = 0,
        Walk = 1,
        Picking = 2,
        Idle_Carrying = 3,
        Walk_Carrying = 4,
    }

    public class AIBehavior : PoolObject
    {
        [Header("AIBehavior")]

        public GameManager _gameManager;
        public MayTinh _mayTinh;

        protected ItemPooler _itemPooler;
        protected SensorCast _boxSensor;
        protected NavMeshAgent _navMeshAgent;

        public STATE_ANIM _stageAnim;

        public bool _isToDestination; // đén đích rồi
        public float _stopDistance = 0.5f;

        public Animator _anim;

        protected virtual void Awake()
        {
            _boxSensor = GetComponentInChildren<SensorCast>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _anim = GetComponentInChildren<Animator>();
        }

        protected virtual void Start()
        {
            _itemPooler = ItemPooler.Instance;
            _gameManager = GameManager.Instance;
            _mayTinh = GameObject.FindObjectOfType<MayTinh>();
        }

        protected virtual void SetAnim() => _anim.SetInteger("State", (int)_stageAnim);

        /// <summary> Di chuyển đến target và trả đúng nếu đến được đích </summary>
        protected virtual bool MoveToTarget(Transform target)
        {
            _navMeshAgent.SetDestination(target.transform.position);

            // Kiểm tra tới được điểm target
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= _stopDistance)
            {
                _isToDestination = true;
                return true;
            }

            _isToDestination = false;
            return false;
        }

    }
}