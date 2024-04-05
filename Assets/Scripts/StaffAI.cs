using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Hieu;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace CuaHang
{
    public class StaffAI : MonoBehaviour
    {
        public BoxSensor _sensorForward;
        public StaffMovement _movement;
        public Transform _targetTransform;  // Thứ mà cái này nhân viên hướng tới, không phải là thứ đang dữ trong người
        public Transform _parcelHolding; // objectPlant người chơi đã nhặt đc và đang giữ trong người
        public Transform _objectPlantHolder; // Kho chứa các ObjectPlant ở trong world
        public Transform _targetModelHolding; // là vị trí mà nhân viên này đang giữ ObjectPlant trong người 
        public Transform _storageHolder; // cái kho
        public bool _isMoveToTarget;
        public bool _isPickedUpParcel;

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
            // move to parcel and pickup parcel
            if (_parcelHolding == null) OnArrivesParcel();

            // giao hàng vào kệ
            if (IsItemInParcel())
            {
                //  tìm chỗ để thả item
                Debug.Log("Nhân viên tìm kệ còn trống hoặc cái kho để đặt parcel rỗng");
                PutItemToTable();
            }

            // khi đã giao hết hàng
            if (!IsItemInParcel())
            {
                // Tìm kho và tìm chỗ trống còn trong kho đó
                Debug.Log("Tìm kho hàng để đặt parcel rỗng");
                PutParcelToStorage();
            }

            // TODO: Trường hợp parcel đã đặt ra khỏi tay rồi thì cần tìm kiện hàng còn item bênh trong để đặt tiếp vào kệ
        }

        private void OnArrivesParcel()
        {
            if (IsArrivesObjPlant())
            {
                Debug.Log("Đã chạm phải ObjPlant");
                if (_targetTransform.GetComponent<ObjectPlant>()._objPlantSO._name == "Parcel")
                {
                    PickUpParcel();
                    _parcelHolding = _targetTransform;
                    _targetTransform = null;
                }
            }
        }

        void PutItemToTable()
        {
            if (IsArrivesObjPlant())
            {
                SenderItems();
            }

            _targetTransform = FindTableCanDrop();
        }

        void PutParcelToStorage()
        {
            if (_parcelHolding != null)
            {
                _targetTransform = FindStorageRoom();
            }

            if (IsArrivesStorage())
            {
                Debug.Log("Đặt tới kho");
                DropParcel(_targetTransform.GetComponent<StorageRoom>().GetSlotEmpty());
                _targetTransform = FindParcel();
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
            }
        }

        void DropParcel(Transform location)
        {
            if (_isPickedUpParcel)
            {
                // AI ẩn cái ObjectPlant mà nó nhặt đi
                _parcelHolding.SetParent(_objectPlantHolder);
                _parcelHolding.position = location.position;
                _parcelHolding.rotation = location.rotation;

                _isPickedUpParcel = false;
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
                if (!parcel._objPlantSO._listItem.Contains(null) && FindTableCanDrop()) return objPlant;
                else return objPlant;
            }
            return null;
        }


        Transform FindStorageRoom()
        {
            foreach (Transform child in _storageHolder)
            {
                StorageRoom storage = child.GetComponent<StorageRoom>();
                if (storage == null) continue;
                if (storage.GetSlotEmpty() == null) continue;
                return child;
            }
            return null;
        }

        // AI nó sẽ di chuyển tới điểm tiếp, AI tìm chỗ để đặt các item
        Transform FindTableCanDrop()
        {
            // thực hiện chọn địa điểm: Chọn ngẫu nhiên cái kệ còn trống
            foreach (Transform objPlant in _objectPlantHolder)
            {
                ObjectPlant table = objPlant.GetComponent<ObjectPlant>();
                if (!table) continue;
                if (table._objPlantSO._name != "Table") continue;
                if (!table._objPlantSO._listItem.Contains(null)) continue;
                return objPlant;
            }

            // Trường hợp không còn có cái kệ nào còn trống
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