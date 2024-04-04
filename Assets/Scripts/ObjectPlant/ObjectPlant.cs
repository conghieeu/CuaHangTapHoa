using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CuaHang
{
    public class ObjectPlant : MonoBehaviour
    {
        public ObjectPlantSO _objectInfoSO;
        public Transform _models;
        public Transform _tempPrefab;
        public PlayerCtrl _player;

        [Space]
        public List<Transform> _slots;

        private void Start()
        {
            _player = PlayerCtrl.Instance;

            LoadItemsSlot();
        }

        // tải hình ảnh item từ trong SO lên, đây là việc load dữ liệu lênh nên ko được chỉnh sửa dử liệu
        public void LoadItemsSlot()
        {
            for (int i = 0; i < _slots.Count && i < _objectInfoSO._listItem.Count; i++)
            {
                // tạo đưa vào
                Instantiate(_objectInfoSO._listItem[i]._itemPrefabs, _slots[i]);
            }

            // TODO: Lấy item ra
            for (int i = _slots.Count - 1; i >= 0; i--)
            {
                if (_objectInfoSO._listItem.Count < i + 1 && _slots[i].childCount > 0)
                {
                    foreach (Transform child in _slots[i].transform)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }

        // đặt object cần đặt vào vị trí
        public void InstantTemp()
        {
            Destroy(this.gameObject);

            // Set Temp 
            ObjectTemp temp = _player._temp;
            temp.gameObject.SetActive(true);
            temp._ObjectContactDisplay = _objectInfoSO;
        }
    }

}