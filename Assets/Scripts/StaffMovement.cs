using System.Collections;
using System.Collections.Generic;
using CuaHang;
using UnityEngine;
using UnityEngine.AI;

namespace CuaHang
{
    public class StaffMovement : MonoBehaviour
    {
        NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo(Transform target)
        {
            _navMeshAgent.SetDestination(target.position);
        }
    }
}
