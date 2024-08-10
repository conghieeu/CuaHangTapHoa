using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public class CameraControl : MonoBehaviour
    {
        public float _rotationSpeed;
        public float _moveSpeed;
        public Transform _characterFollow;
        public Transform _objectFollow; // là đối tượng theo giỏi object forcus
        public Transform _cameraHolder;
        public ObjectDrag _objectDrag;
        public Camera _cam;

        private void Start()
        {
            _characterFollow = PlayerCtrl.Instance.transform;
            _cam = Camera.main;
        }

        private void Update()
        {
            // Move forward character follow
            _objectFollow.position = Vector3.MoveTowards(_objectFollow.position, _characterFollow.position, _moveSpeed * Time.deltaTime);
            _cam.transform.position = _cameraHolder.position;
            _cam.transform.rotation = _cameraHolder.rotation;

            // Kiểm tra nếu giữ chuột phải
            if (Input.GetMouseButton(1))
            {
                // Lấy giá trị delta của chuột (sự thay đổi vị trí chuột)
                float mouseX = Input.GetAxis("Mouse X");

                // Xoay đối tượng quanh trục Y dựa trên giá trị delta của chuột
                _objectFollow.Rotate(Vector3.up, mouseX * _rotationSpeed, Space.Self);
            }

            // return _characterFollow = player 
            if (_objectDrag._itemDragging)
                if (_objectDrag._itemDragging.transform == _characterFollow)
                {
                    _characterFollow = PlayerCtrl.Instance.transform;
                }
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                _characterFollow = PlayerCtrl.Instance.transform;
            }
        }
    }
}
