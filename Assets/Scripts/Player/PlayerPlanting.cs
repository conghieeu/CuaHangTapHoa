using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace CuaHang
{
    public class PlayerPlanting : MonoBehaviour
    {
        // public Transform _posHoldParcel; // vị trí đặt cái parcel này trên tay
        public ObjectPlant _parcelHolding; // bưu kiện đang giữ
        public List<ObjectSell> _listItem; // Ds item mà player đang giữ trong người
        PlayerCtrl _ctrl;
        // khi dragging object temp thì thằng này sẽ hiện model của object đang drag nớ ra

        private void Awake()
        {
            _ctrl = GetComponent<PlayerCtrl>();
        }

        private void FixedUpdate()
        {
            if (_ctrl._temp._isDragging) TempAiming();
        }

        private void Update()
        {
            // gửi item trong parcel vào kệ
            if (Input.GetKeyUp(KeyCode.T))
            {
                // SenderItems();
                SenderItemParcelToTable();

            }

            // parcel theo người chơi
            if (_parcelHolding) _parcelHolding.FollowParent();
        }

        /// <summary> chạm vào vật thể thì mới được nhặt item đó lênh </summary>
        // void PickUpItem()
        // {
        //     ObjectPlant objHit = _ctrl._sensor.GetObjectPlantHit();

        //     if (objHit)
        //     {
        //         _parcelHolding = objHit;
        //         _parcelHolding.SetThisParent(_posHoldParcel);
        //     }
        // }

        /// <summary> Khi mà drag object Temp thì player sẽ hướng về object Temp </summary>
        void TempAiming()
        {
            var direction = _ctrl._temp.transform.position - transform.position;
            direction.y = 0;
            transform.forward = direction;
        }

        /// <summary> chạm vào kệ, người chơi lấy item từ table sang listItem này </summary>
        void GetItemInTable()
        {
            if (_ctrl._sensor._hits.Count == 0) return;

            ObjectPlant objectPlantReceiver = null;

            // Get sender
            foreach (var hit in _ctrl._sensor._hits)
            {
                if (hit.GetComponent<ObjectPlant>())
                    objectPlantReceiver = hit.GetComponent<ObjectPlant>();
            }

            // Nếu vật thể đã chạm được tới thực thể cần tới 
            if (objectPlantReceiver && _parcelHolding)
            {
                for (int i = _parcelHolding._slots.Count - 1; i >= 0; i--)
                {
                    if (_parcelHolding._items[i] == null) continue;

                    // lấy item từ table nạp vào player này
                    for (int j = 0; j < _listItem.Count; j++)
                    {
                        if (_listItem[j] == null)
                        {
                            _listItem[j] = _parcelHolding._items[i];
                            _parcelHolding.RemoveItem(_parcelHolding._items[i]);
                        }
                    }
                }
            }
        }

        /// <summary> chạm vào kệ, người chơi có thể truyền item từ parcel sang table đó </summary>
        void SenderItemParcelToTable()
        {
            ObjectPlant receiver = _ctrl._sensor.GetObjectPlantHit();

            if (_parcelHolding && receiver)
            {
                // chuyển item
                for (int i = _parcelHolding._slots.Count - 1; i >= 0; i--)
                {
                    ObjectSell item = _parcelHolding._items[i];
                    if (item != null)
                    {
                        receiver.AddItem(item);
                        _parcelHolding.RemoveItem(item);
                    }
                }
            }

        }
    }
}
