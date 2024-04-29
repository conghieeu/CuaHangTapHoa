using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CuaHang
{
    public class AIBehavior : MonoBehaviour
    {
        [Header("AIBehavior")]
        public ObjectPlant _objPlantTarget;  // Thứ mà cái này nhân viên hướng tới, không phải là thứ đang dữ trong người
        public ObjectPlant _parcelHolding; // objectPlant người chơi đã nhặt đc và đang giữ trong người
        public Transform _targetModelHolding; // là vị trí mà nhân viên này đang giữ ObjectPlant trong người
        public List<Transform> _slots;

        [SerializeField] protected SensorCast _boxSensor;
        [SerializeField] private NavMeshAgent _navMeshAgent;

        protected virtual void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Nó sẽ đưa cái item từ slot này sang slot kia của cái bàn, false là còn kiện hàng đơn hàng chưa giao hết
        protected virtual void SenderItemsToOtherObjectPlant(ObjectPlant sender, ObjectPlant receiver)
        {
            // Nếu vật thể đã chạm được tới thực thể cần tới 
            if (GetObjPlantHit() && _parcelHolding != null)
            {
                // thực hiện việc truyền đơn hàng
                Debug.Log("Thực hiện việc truyền dữ liệu đơn hàng");

                // chuyển item
                for (int i = sender._slots.Count - 1; i >= 0; i--)
                {
                    ObjectSell item = sender._items[i];
                    if (item != null && receiver.IsAnyEmptyItem())
                    {
                        receiver.AddItem(item);
                        sender.RemoveItem(item);
                    }
                }
            }
        }

        // AI biết nó chạm tới tứ nó cần
        protected virtual ObjectPlant GetObjPlantHit()
        {
            Transform obj = _boxSensor._hits.Find(hit => hit.transform == _objPlantTarget.transform);
            if (obj) return obj.GetComponent<ObjectPlant>();
            return null;
        }

        protected virtual void Movement()
        {
            if (_objPlantTarget != null)
            {
                _navMeshAgent.SetDestination(_objPlantTarget.transform.position);
            }
        }

        /// <summary> Nhặt item </summary>
        /// <returns> slot đối tượng sẽ là cha của ObjectPlant </returns>
        protected virtual bool PickUpParcel()
        {
            _parcelHolding = _objPlantTarget.GetComponent<ObjectPlant>();
            _parcelHolding.SetThisParent(_targetModelHolding);
            _objPlantTarget = null;
            return false;
        }

        /// <summary> Đặt item xuống thì set vị trí và parent lại </summary>
        /// <returns> location: vị trí đặt parcel này xuống </returns>
        protected virtual void DropParcel(Transform location)
        {
            _parcelHolding.SetThisParent(BoolingObjPlants.Instance.transform);
            _parcelHolding.transform.position = location.position;
            _parcelHolding.transform.rotation = location.rotation;
            _parcelHolding = null;
            _objPlantTarget = null;
        }

        /// <summary> Tìm objectPlant với typeID và phải có chỗ để còn trống </summary>
        protected virtual ObjectPlant GetObjPlantWithTypeID(String typeID)
        {
            return BoolingObjPlants.Instance.FindObject(typeID, true); 
        }

        /// <summary> Tim object plant với type còn chỗ trống vì khách hàng sẽ chạy tới các loại kệ hàng </summary>
        protected virtual ObjectPlant GetObjPlaWithType(String type)
        {
            return BoolingObjPlants.Instance.FindObjectType(type, true);
        }

        protected virtual bool IsAnyItemInParcel()
        {
            if (_parcelHolding)
                if (_parcelHolding.GetComponent<ObjectPlant>())
                {
                    if (_parcelHolding.GetComponent<ObjectPlant>().IsAnyItem()) return true;
                }
            return false;
        }
    }
}