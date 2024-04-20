using System.Collections;
using System.Collections.Generic;
using CuaHang;
using UnityEngine;
using UnityEngine.AI;

namespace CuaHang
{
    public class AIMovement : MonoBehaviour
    {
        [Header("AIMovement")]
        NavMeshAgent _navMeshAgent;
        public Transform _targetTransform;
        public Transform _parcelHolding;
        BoxSensor _boxSensor;
        AIMovement _movement;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo(Transform target)
        {
            _navMeshAgent.SetDestination(target.position);
        }


        // Nó sẽ đưa cái item từ slot này sang slot kia của cái bàn, false là còn kiện hàng đơn hàng chưa giao hết
        protected virtual void SenderItems()
        {
            // Nếu vật thể đã chạm được tới thực thể cần tới 
            if (GetObjPlantHit() && _parcelHolding != null)
            {
                // thực hiện việc truyền đơn hàng
                Debug.Log("Thực hiện việc truyền dữ liệu đơn hàng");

                ObjectPlant parcel = _parcelHolding.GetComponent<ObjectPlant>();
                ObjectPlant table = _targetTransform.GetComponent<ObjectPlant>();

                // chuyển item
                for (int i = parcel._listItem.Count - 1; i >= 0; i--)
                {
                    if (parcel._listItem[i] == null) continue;

                    for (int j = 0; j < table._listItem.Count; j++)
                    {
                        if (table._listItem[j] == null)
                        {
                            table._listItem[j] = parcel._listItem[i];
                            parcel._listItem[i] = null;
                        }
                    }
                }

                // Load lại các item hiển thị
                _targetTransform.GetComponent<ObjectPlant>().LoadItemsSlot();
                _parcelHolding.GetComponent<ObjectPlant>().LoadItemsSlot();
            }
        }

        // AI biết nó chạm tới tứ nó cần
        protected Transform GetObjPlantHit()
        {
            return _boxSensor._hits.Find(hit => hit.transform == _targetTransform && hit.GetComponent<ObjectPlant>()); ;
        }
    }
}
