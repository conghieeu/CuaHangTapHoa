using System;
using Unity.VisualScripting;
using UnityEngine;

namespace CuaHang.StaffAI
{
    public class StaffAI : MonoBehaviour
    {
        public BoxSensor _sensorForward;
        public StaffMovement _movement;
        [Space]
        public Transform _targetTransform;  // Thứ mà cái này nhân viên hướng tới, không phải là thứ đang dữ trong người
        public Transform _parcelHolding; // objectPlant người chơi đã nhặt đc và đang giữ trong người
        [Space]
        public Transform _objectPlantHolder; // Kho chứa các ObjectPlant ở trong world
        public Transform _targetModelHolding; // là vị trí mà nhân viên này đang giữ ObjectPlant trong người

        private void FixedUpdate()
        {
            BehaviorCtrl();

            SetMovement();
        }

        // Khi đáp ứng sự kiện hãy gọi vào đây nên nó đưa phán đoán hành vi tiếp theo nhân viên cần làm
        public void BehaviorCtrl()
        {
            // Tìm parcel
            if (!_parcelHolding)
            {
                _targetTransform = FindParcel();
            }
            // Nhặt parcel
            if (_targetTransform && !_parcelHolding && GetObjPlantHit())
            {
                if (GetObjPlantHit().GetComponent<ObjectPlant>()._name == "Parcel")
                {
                    PickUpParcel();
                    _parcelHolding = _targetTransform;
                    _targetTransform = null;
                }
            }
            // đang có parcel và parcel còn item trong người nên cần tìm cần cái kệ để đặt item
            else if (FindObjectPlant("Table") && IsItemInParcel())
            {
                Debug.Log("Nhân viên tìm kệ còn trống hoặc cái kho để đặt parcel rỗng");
                StateItemToTable();
            }
            // Đặt parcel vào kho hoặc thùng rác
            else if (!FindObjectPlant("Table") || !IsItemInParcel())
            {
                Debug.Log("Tìm thùng rác hoặc kho để đặt parcel rỗng");
                StateDropParcel();
            }
        }

        void SetMovement()
        {
            if (_targetTransform != null)
            {
                Debug.Log("AI di chuyển tới target");
                _movement.MoveTo(_targetTransform);
            }
        }

        void StateItemToTable()
        {
            if (!_targetTransform)
            {
                _targetTransform = FindObjectPlant("Table");
            }
            else if (GetObjPlantHit())
            {
                if (GetObjPlantHit().GetComponent<ObjectPlant>()._name == "Table")
                {
                    SenderItems();

                    _targetTransform = null;
                }
            }
        }

        /// <summary> Cần tìm đối tượng để AI có thể đạt parcel xuống </summary>
        void StateDropParcel()
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

        bool IsArrivesStorage()
        {
            return _sensorForward._hits.Find(hit => hit.transform == _targetTransform && hit.GetComponent<StorageRoom>()); ;
        }

        // AI biết nó chạm tới tứ nó cần
        Transform GetObjPlantHit()
        {
            return _sensorForward._hits.Find(hit => hit.transform == _targetTransform && hit.GetComponent<ObjectPlant>()); ;
        }

        // AI nó sẽ nhặt lênh
        void PickUpParcel()
        {
            // AI ẩn cái ObjectPlant mà nó nhặt đi
            _targetTransform.SetParent(_targetModelHolding);
            _targetTransform.localPosition = Vector3.zero;
            _targetTransform.localRotation = Quaternion.identity;
        }

        void DropParcel(Transform location)
        {

            // AI ẩn cái ObjectPlant mà nó nhặt đi
            _parcelHolding.SetParent(location);
            _parcelHolding.position = location.position;
            _parcelHolding.rotation = location.rotation;

            _parcelHolding = null;

        }

        // Tìm parcel còn item trước và parcel không còn item sau
        Transform FindParcel()
        {
            foreach (Transform objPlant in _objectPlantHolder)
            {
                ObjectPlant parcel = objPlant.GetComponent<ObjectPlant>();
                if (!parcel) continue;
                if (parcel._name != "Parcel") continue;
                if (!parcel._listItem.Contains(null) && FindObjectPlant("Table")) return objPlant;
                else return objPlant;
            }
            return null;
        }

        Transform FindTrash()
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

        Transform FindStorageRoom()
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
        Transform FindObjectPlant(String name)
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

        // Nó sẽ đưa cái item từ slot này sang slot kia của cái bàn, false là còn kiện hàng đơn hàng chưa giao hết
        void SenderItems()
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

        /// <summary> Kiểm tra item có còn trong parcel không </summary>
        bool IsItemInParcel()
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