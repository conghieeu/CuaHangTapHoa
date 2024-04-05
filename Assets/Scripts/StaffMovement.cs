using System.Collections;
using System.Collections.Generic;
using CuaHang;
using UnityEngine;
using UnityEngine.AI;

public class StaffMovement : MonoBehaviour
{
    public ObjectPlant _objectPlant;

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
