using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    /// <summary> Aim con trỏ, drag temp và có thể snap khi drag temp </summary>
    public class Snapping : MonoBehaviour
    {
        public ObjectDrag _temp;
        public bool _enableSnapping; // bật chế độ snapping
        public float _snapDistance = 6f; // Khoảng cách cho phép đặt 
        public float tilesize = 1;
        public Vector3 tileOffset = Vector3.zero;
        public LayerMask _layerMask;
        public RaycastHit _hit;
        Camera cam;

        private void Awake()
        {
            cam = Camera.main;
        }

        void Update()
        {
            GetHitForward();
        }

        void FixedUpdate()
        {
            DroppingObjectTemp();
        }

        private void DroppingObjectTemp()
        {
            if (!_temp) return;

            SetTempRotation();
            SetSnapping();

            // khoảng cách bị quá dài
            if (Vector3.Distance(cam.transform.position, _hit.point) < _snapDistance)
            {
                _temp.GetComponent<ObjectDrag>()._isDistance = true;
            }
            else
            {
                _temp.GetComponent<ObjectDrag>()._isDistance = false;
            }
        }

        private void GetHitForward()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out _hit, 100, _layerMask))
            {
                Item objPHit = _hit.transform.GetComponent<Item>();
                if (Input.GetKeyDown(KeyCode.E) && objPHit)
                {
                    // Nếu chạm phải là vật thể object có thể đem đi dặt thì biến nó thành dạng temp, để có thể đem đi đặt
                    objPHit.DragItem();
                    _temp.PickUpObjectPlant();
                }
            }
        }

        // Làm tròn vị trí temp để nó giống snap
        private void SetSnapping()
        {
            if (!_enableSnapping) return;

            Vector3 currentPosition = _temp.transform.position;

            float snappedX = Mathf.Round(currentPosition.x / tilesize) * tilesize + tileOffset.x;
            float snappedZ = Mathf.Round(currentPosition.z / tilesize) * tilesize + tileOffset.z;
            float snappedY = Mathf.Round(currentPosition.y / tilesize) * tilesize + tileOffset.y;

            Vector3 snappedPosition = new Vector3(snappedX, snappedY, snappedZ);
            _temp.transform.position = snappedPosition;
        }

        // Giúp xoay temp
        private void SetTempRotation()
        {
            _temp.transform.position = _hit.point;
            _temp.transform.rotation = Quaternion.FromToRotation(Vector3.up, _hit.normal);
        }
    }

}
