using System;
using UnityEngine;

namespace CuaHang
{
    public class AIBehavior : MonoBehaviour
    {
        [Header("AIBehavior")]
        public BoxSensor _boxSensor;
        public AIMovement _movement;
        [Space]
        public Transform _targetTransform;  // Thứ mà cái này nhân viên hướng tới, không phải là thứ đang dữ trong người
        public Transform _parcelHolding; // objectPlant người chơi đã nhặt đc và đang giữ trong người
        [Space]
        public Transform _objectPlantHolder; // Kho chứa các ObjectPlant ở trong world
        public Transform _targetModelHolding; // là vị trí mà nhân viên này đang giữ ObjectPlant trong người

        // Nó sẽ đưa cái item từ slot này sang slot kia của cái bàn, false là còn kiện hàng đơn hàng chưa giao hết
        protected virtual void SenderItemsObjectPlant(ObjectPlant sender, ObjectPlant receiver)
        {
            // Nếu vật thể đã chạm được tới thực thể cần tới 
            if (GetObjPlantHit() && _parcelHolding != null)
            {
                // thực hiện việc truyền đơn hàng
                Debug.Log("Thực hiện việc truyền dữ liệu đơn hàng");

                // chuyển item
                for (int i = sender._listItem.Count - 1; i >= 0; i--)
                {
                    if (sender._listItem[i] == null) continue;

                    for (int j = 0; j < receiver._listItem.Count; j++)
                    {
                        if (receiver._listItem[j] == null)
                        {
                            receiver._listItem[j] = sender._listItem[i];
                            sender._listItem[i] = null;
                        }
                    }
                }

                // Load lại các item hiển thị
                sender.LoadItemsSlot();
                receiver.LoadItemsSlot();
            }
        }

        // AI biết nó chạm tới tứ nó cần
        protected virtual ObjectPlant GetObjPlantHit()
        {
            Transform obj = _boxSensor._hits.Find(hit => hit.transform == _targetTransform && hit.GetComponent<ObjectPlant>());
            return obj.GetComponent<ObjectPlant>();
        }

        protected virtual void Movement()
        {
            if (_targetTransform != null)
            {
                Debug.Log("AI di chuyển tới target");
                _movement.MoveTo(_targetTransform);
            }
        }

        protected virtual void StateItemToTable()
        {
            if (!_targetTransform)
            {
                _targetTransform = FindObjectPlantWithName("Table");
            }
            else if (GetObjPlantHit())
            {
                if (GetObjPlantHit().GetComponent<ObjectPlant>()._name == "Table")
                {
                    ObjectPlant parcel = _parcelHolding.GetComponent<ObjectPlant>();
                    ObjectPlant table = _targetTransform.GetComponent<ObjectPlant>();
                    SenderItemsObjectPlant(parcel, table);

                    _targetTransform = null;
                }
            }
        }

        /// <summary> Cần tìm đối tượng để AI có thể đạt parcel xuống </summary>
        protected virtual void StateDropParcel()
        {
            // Tìm kho hoặc thùng rác
            if (_parcelHolding && !_targetTransform)
            {
                Debug.Log("Co con item o trong parcel hay khong " + IsItemInParcel());
                if (IsItemInParcel())
                {
                    _targetTransform = FindStorageRoom();
                }
                else
                {
                    _targetTransform = FindTrash();
                }
            }
            // Tới điểm cần tới
            else if (IsArrivesStorage())
            {
                Debug.Log("Đặt tới kho, hoặc thùng rác");
                if (_targetTransform.GetComponent<StorageRoom>().GetSlotEmpty())
                {
                    // Nếu là va vào thùng rác thì Thêm parcel vào list của thùng rác để nó xoá
                    if (!IsItemInParcel())
                    {
                        _targetTransform.GetComponent<Trash>().AddDeleteItem(_parcelHolding);
                    }

                    DropParcel(_targetTransform.GetComponent<StorageRoom>().GetSlotEmpty());

                    _targetTransform = null;
                }
            }
        }

        protected virtual bool IsArrivesStorage()
        {
            return _boxSensor._hits.Find(hit => hit.transform == _targetTransform && hit.GetComponent<StorageRoom>()); ;
        }

        // AI nó sẽ nhặt lênh
        protected virtual void PickUpParcel()
        {
            // AI ẩn cái ObjectPlant mà nó nhặt đi
            _targetTransform.SetParent(_targetModelHolding);
            _targetTransform.localPosition = Vector3.zero;
            _targetTransform.localRotation = Quaternion.identity;
        }

        protected virtual void DropParcel(Transform location)
        {

            // AI ẩn cái ObjectPlant mà nó nhặt đi
            _parcelHolding.SetParent(location);
            _parcelHolding.position = location.position;
            _parcelHolding.rotation = location.rotation;

            _parcelHolding = null;

        }

        // Tìm parcel còn item trước và parcel không còn item sau
        protected virtual Transform FindParcel()
        {
            foreach (Transform objPlant in _objectPlantHolder)
            {
                ObjectPlant parcel = objPlant.GetComponent<ObjectPlant>();
                if (!parcel) continue;
                if (parcel._name != "Parcel") continue;
                if (!parcel._listItem.Contains(null) && FindObjectPlantWithName("Table")) return objPlant;
                else return objPlant;
            }
            return null;
        }

        protected virtual Transform FindTrash()
        {
            Debug.Log("Nhân viên tìm thùng rác");
            foreach (Transform child in _objectPlantHolder)
            {
                Trash trash = child.GetComponent<Trash>();
                if (!trash) continue;
                if (trash._name != "Trash") continue;
                if (trash.GetSlotEmpty() == null) continue;
                return child;
            }
            return null;
        }

        protected virtual Transform FindStorageRoom()
        {
            foreach (Transform child in _objectPlantHolder)
            {
                StorageRoom storage = child.GetComponent<StorageRoom>();
                if (storage == null) continue;
                if (storage._name != "Storage") continue;
                if (storage.GetSlotEmpty() == null) continue;
                return child;
            }
            return null;
        }

        /// <summary> Tim object plant theo ten trong SO </summary>
        protected virtual Transform FindObjectPlantWithName(String name)
        {
            foreach (Transform child in _objectPlantHolder)
            {
                ObjectPlant table = child.GetComponent<ObjectPlant>();
                if (!table) continue;
                if (table._name != name) continue;
                if (!table._listItem.Contains(null)) continue;
                return child;
            }
            return null;
        }

        /// <summary> Tim object plant theo ten trong SO </summary>
        protected virtual Transform FindObjectPlantWithType(String type)
        {
            foreach (Transform child in _objectPlantHolder)
            {
                ObjectPlant obj = child.GetComponent<ObjectPlant>();
                if (!obj) continue;
                if (obj._type != type) continue;
                if (!obj._listItem.Contains(null)) continue;
                return child;
            }
            return null;
        }

        /// <summary> Kiểm tra item có còn trong parcel không </summary>
        protected virtual bool IsItemInParcel()
        {
            if (_parcelHolding)
                if (_parcelHolding.GetComponent<ObjectPlant>())
                    foreach (var item in _parcelHolding.GetComponent<ObjectPlant>()._listItem)
                    {
                        if (item != null) return true;
                    }

            return false;
        }


    }
}