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

    public class AIBehavior : ObjectPool
    {
        [Header("AIBehavior")]
        public STATE_ANIM _stageAnim;
        [SerializeField] private float _stopDistance = 0.5f;
        [SerializeField] protected MayTinh _mayTinh;
        [SerializeField] protected ItemPooler _itemPooler;
        [SerializeField] protected SensorCast _boxSensor;
        [SerializeField] protected NavMeshAgent _navMeshAgent;
        [SerializeField] protected GameManager _gameManager;
        [SerializeField] protected Animator _anim;

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
                return true;
            }
 
            return false;
        }
    }
}