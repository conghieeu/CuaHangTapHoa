using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang.UI
{
    public class UISpaceDetect : MonoBehaviour
    {
        public Camera _cam;
        public Transform _target;

        private void Awake()
        {
            _cam = Camera.main;
        }

        void FixedUpdate()
        {
            // Lấy vị trí của camera
            Vector3 cameraPosition = _cam.transform.position;
            Vector3 directionToCamera = transform.position - cameraPosition;
            Quaternion rotation = Quaternion.LookRotation(directionToCamera);
            transform.rotation = rotation;

            // di chuyen den diem target
            transform.position = _target.transform.position;
        }
    }
}
