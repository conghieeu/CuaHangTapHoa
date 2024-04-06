using System;
using UnityEngine;

namespace CuaHang.StaffAI
{
    public class StaffAI : MonoBehaviour
    {

        public BoxSensor _sensorForward;
        public StaffMovement _movement;
        public Transform _targetTransform;  // Thứ mà cái này nhân viên hướng tới, không phải là thứ đang dữ trong người
        public Transform _parcelHolding; // objectPlant người chơi đã nhặt đc và đang giữ trong người
        public Transform _objectPlantHolder; // Kho chứa các ObjectPlant ở trong world
        public Transform _targetModelHolding; // là vị trí mà nhân viên này đang giữ ObjectPlant trong người

        public bool _isMoveToTarget;
        public bool _isPickedUpParcel;
        public bool _isFindingParcel;
        public bool _isDropParcelToTrash; // dat parcel rong vao thung rac, parcel thua vao kho

        private void Start()
        {
            _targetTransform = FindParcel();

            _movement.MoveTo(_targetTransform);
            _sensorForward._eventTrigger.AddListener(OnArrivesTarget);
        }

        private void FixedUpdate()
        {
            BehaviorAI();
        }

        void BehaviorAI()
        {
            if (_targetTransform != null)
            {
                Debug.Log("AI di chuyển tới target");
                _movement.MoveTo(_targetTransform);
            }
        }

        void OnArrivesTarget()
        {
            if (IsArrivesObjPlant())
            {
                SenderItems();
            }

            // move to parcel and pickup parcel
            if (_parcelHolding == null) StatePickParcel();

            // giao hàng vào kệ
            if (FindObjectPlant("Table") && IsItemInParcel())
            {
                //  tìm chỗ để thả item
                Debug.Log("Nhân viên tìm kệ còn trống hoặc cái kho để đặt parcel rỗng");
                StateItemToTable();
            }
            // Tìm thùng rác còn trống đặt parcel vào trong đó
            else if (!FindObjectPlant("Table") || !IsItemInParcel())
            {
                Debug.Log("Tìm thùng rác hoặc kho để đặt parcel rỗng");
                StateDropParcel();
            }
        }

        private void StatePickParcel()
        {
            if (IsArrivesObjPlant())
            {
                if (_targetTransform.GetComponent<ObjectPlant>()._objPlantSO._name == "Parcel")
                {
                    PickUpParcel();
                    _parcelHolding = _targetTransform;
                    _targetTransform = null;
                }
            }
        }

        void StateItemToTable()
        {
            _targetTransform = FindObjectPlant("Table");
        }

        /// <summary> Cần tìm đối tượng để AI có thể đạt parcel xuống </summary>
        void StateDropParcel()
        {
            // Tìm kiện hàng hoặc thùng rác
            if (_parcelHolding != null && _isFindingParcel == false)
            {
                if (IsItemInParcel())
                {
                    _targetTransform = FindStorageRoom();
                }
                else
                {
                    _targetTransform = FindTrash();
                }
            }

            // Đặt parcel vào đâu đó
            if (IsArrivesStorage())
            {
                Debug.Log("Đặt tới kho, hoặc thùng rác");

                // Thêm parcel vào thùng rác để nó xoá
                if (IsItemInParcel() == false)
                {
                    _targetTransform.GetComponent<Trash>().AddDeleteItem(_parcelHolding);
                }

                DropParcel(_targetTransform.GetComponent<StorageRoom>().GetSlotEmpty());

                if (_parcelHolding == null)
                {
                    Debug.Log("Nhân viên tìm bưu kiện khác");
                    _targetTransform = FindParcel();
                    _isFindingParcel = true;
                }
            }
        }

        bool IsArrivesStorage()
        {
            return _sensorForward.GetHits().Find(hit => hit.transform == _targetTransform && hit.GetComponent<StorageRoom>());
        }

        // AI biết nó chạm tới tứ nó cần
        bool IsArrivesObjPlant()
        {
            return _sensorForward.GetHits().Find(hit => hit.transform == _targetTransform && hit.GetComponent<ObjectPlant>());
        }

        // AI nó sẽ nhặt lênh
        void PickUpParcel()
        {
            if (!_isPickedUpParcel)
            {
                // AI ẩn cái ObjectPlant mà nó nhặt đi
                _targetTransform.SetParent(_targetModelHolding);
                _targetTransform.localPosition = Vector3.zero;
                _targetTransform.localRotation = Quaternion.identity;

                _isPickedUpParcel = true;
                _isFindingParcel = false;
            }
        }

        void DropParcel(Transform location)
        {
            if (_isPickedUpParcel)
            {
                // AI ẩn cái ObjectPlant mà nó nhặt đi
                _parcelHolding.SetParent(location);
                _parcelHolding.position = location.position;
                _parcelHolding.rotation = location.rotation;

                _isPickedUpParcel = false;
                _parcelHolding = null;
            }
        }

        // Tìm parcel còn item trước và parcel không còn item sau
        Transform FindParcel()
        {
            foreach (Transform objPlant in _objectPlantHolder)
            {
                ObjectPlant parcel = objPlant.GetComponent<ObjectPlant>();
                if (!parcel) continue;
                if (parcel._objPlantSO._name != "Parcel") continue;
                if (!parcel._objPlantSO._listItem.Contains(null) && FindObjectPlant("Table")) return objPlant;
                else return objPlant;
            }
            return null;
        }

        Transform FindTrash()
        {
            foreach (Transform child in _objectPlantHolder)
            {
                Trash trash = child.GetComponent<Trash>();
                if (!trash) continue;
                if (trash._objPlantSO._name != "Trash") continue;
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
                if (storage._objPlantSO._name != "Storage") continue;
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
                if (table._objPlantSO._name != name) continue;
                if (!table._objPlantSO._listItem.Contains(null)) continue;
                return child;
            }
            return null;
        }

        // Nó sẽ đưa cái item từ slot này sang slot kia của cái bàn, false là còn kiện hàng đơn hàng chưa giao hết
        void SenderItems()
        {
            // Nếu vật thể đã chạm được tới thực thể cần tới 
            if (IsArrivesObjPlant() && _parcelHolding != null)
            {
                // thực hiện việc truyền đơn hàng
                Debug.Log("Thực hiện việc truyền dữ liệu đơn hàng");

                ObjectPlantSO parcelSO = _parcelHolding.GetComponent<ObjectPlant>()._objPlantSO;
                ObjectPlantSO tableSO = _targetTransform.GetComponent<ObjectPlant>()._objPlantSO;

                // chuyển item
                for (int i = parcelSO._listItem.Count - 1; i >= 0; i--)
                {
                    if (parcelSO._listItem[i] == null) continue;

                    for (int j = 0; j < tableSO._listItem.Count; j++)
                    {
                        if (tableSO._listItem[j] == null)
                        {
                            tableSO._listItem[j] = parcelSO._listItem[i];
                            parcelSO._listItem[i] = null;
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
                    foreach (var item in _parcelHolding.GetComponent<ObjectPlant>()._objPlantSO._listItem)
                    {
                        if (item != null) return true;
                    }

            return false;
        }
    }
}