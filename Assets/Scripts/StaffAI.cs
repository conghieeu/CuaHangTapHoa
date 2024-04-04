using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CuaHang
{
    public class StaffAI : MonoBehaviour
    {
        public BoxSensor _sensorForward;
        public StaffMovement _movement;
        public Transform _targetTransform;  // Thứ mà cái này nhân viên hướng tới, không phải là thứ đang dữ trong người
        public Transform _objectPlantHolding; // objectPlant người chơi đã nhặt đc và đang giữ trong người
        public Transform _objectPlantHolder; // Kho chứa các ObjectPlant ở trong world
        public Transform _targetModelHolding; // là vị trí mà nhân viên này đang giữ ObjectPlant trong người 
        public bool _isMoveToTarget;
        public bool _isPickedUpObjectPlant;

        private void Start()
        {
            _movement.MoveTo(_targetTransform);
            _sensorForward._eventTrigger.AddListener(OnArrivesColl);
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

        public void OnArrivesColl()
        {
            if (IsTouchTargetPlace())
            {
                PickUpParcel();
                DropItemToTable();
            }
        }

        // AI biết nó chạm tới tứ nó cần
        public bool IsTouchTargetPlace()
        {
            foreach (var hit in _sensorForward._Hits)
            {
                if (hit.transform == _targetTransform && hit.GetComponent<ObjectPlant>())
                {
                    Debug.Log("AI đã chạm vào ObjectPlant");
                    return true;
                }
            }
            return false;
        }

        // AI nó sẽ nhặt lênh
        public void PickUpParcel()
        {
            if (!_isPickedUpObjectPlant)
            {
                // AI ẩn cái ObjectPlant mà nó nhặt đi
                _targetTransform.SetParent(_targetModelHolding);
                _targetTransform.localPosition = Vector3.zero;
                _targetTransform.localRotation = Quaternion.identity;

                _isPickedUpObjectPlant = true;
            }
        }

        // AI nó sẽ di chuyển tới điểm tiếp, AI tìm chỗ để đặt các item
        public Transform GetTableCanDrop()
        {
            // thực hiện chọn địa điểm: Chọn ngẫu nhiên cái kệ còn trống
            foreach (Transform objPlant in _objectPlantHolder)
            {
                ObjectPlant table = objPlant.GetComponent<ObjectPlant>();
                if (!table) continue;
                if (table._objectInfoSO._name != "Table") continue;
                if (table._objectInfoSO._listItem.Count >= table._objectInfoSO._numberSlots) continue;
                _targetTransform = objPlant;
            }

            // Trường hợp không còn có cái kệ nào còn trống


            return null;
        }

        // AI tìm chỗ để thả objectPlant
        public void DropItemToTable()
        {
            // TODO: Thả như nào ?
            TryShipItems();

            // Khi nào thì nên thả ObjectPlant
            _objectPlantHolding = _targetTransform;
            _targetTransform = null;

            // DONE: Địa điểm thả ở đâu
            if (_targetTransform == null)
            {
                Debug.Log("Nhân viên tìm kệ còn trống");
                GetTableCanDrop();
            }
        }

        // Nó sẽ đưa cái item từ slot này sang slot kia của cái bàn
        public bool TryShipItems()
        {
            // Nếu vật thể đã chạm được tới thực thể cần tới 
            if (IsTouchTargetPlace() && _objectPlantHolding != null)
            {
                // thực hiện việc truyền đơn hàng
                Debug.Log("Thực hiện việc truyền dữ liệu đơn hàng");

                ObjectPlantSO parcelSO = _objectPlantHolding.GetComponent<ObjectPlant>()._objectInfoSO;
                ObjectPlantSO tableSO = _targetTransform.GetComponent<ObjectPlant>()._objectInfoSO;

                for (int i = 0; i < parcelSO._listItem.Count; i++)
                {
                    ObjectSellSO item = parcelSO._listItem[i];

                    // Truyền Item
                    tableSO._listItem.Add(item);
                    parcelSO._listItem.RemoveAt(i);
                    i--;

                    // Load lại các item hiển thị
                    _targetTransform.GetComponent<ObjectPlant>().LoadItemsSlot();
                    _objectPlantHolding.GetComponent<ObjectPlant>().LoadItemsSlot();

                    // Khi đơn hàng được thả hết, hoặc kệ hàng này bị đầy thì thả thành công
                    // Lúc này cần chuyển sang table tiếp theo để thả hết hàng
                    if (parcelSO._listItem.Count <= parcelSO._numberSlots && tableSO._listItem.Count >= tableSO._numberSlots)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}