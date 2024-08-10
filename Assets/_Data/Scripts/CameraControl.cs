using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public class CameraControl : MonoBehaviour
    {
        public float _rotationSpeed;
        public Transform _objectForcus;
        public Transform _objectFollow; // là đối tượng theo giỏi object forcus
        public Transform _cameraHolder;
        public Camera _cam;

        private void Start()
        {
            _objectForcus = PlayerCtrl.Instance.transform;
            _cam = Camera.main;
        }

        private void Update()
        {
            _objectFollow.transform.position = _objectForcus.transform.position;
            _cam.transform.position = _cameraHolder.transform.position;
            _cam.transform.rotation = _cameraHolder.transform.rotation;

            // Kiểm tra nếu giữ chuột phải
            if (Input.GetMouseButton(1))
            {
                // Lấy giá trị delta của chuột (sự thay đổi vị trí chuột)
                float mouseX = Input.GetAxis("Mouse X");

                // Xoay đối tượng quanh trục Y dựa trên giá trị delta của chuột
                _objectFollow.Rotate(Vector3.up, mouseX * _rotationSpeed, Space.Self);
            }



        }
    }
}
