using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public class CamHere : MonoBehaviour
    {
        public bool _autoSetCam; // gọi set cam liên tục để check vị trí để như nào là hợp lý
        public float _camSize;
        public Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void FixedUpdate()
        {
            if (_autoSetCam) SetCamFocusHere();

        }

        // camera hãy tập trung vào đây
        public void SetCamFocusHere()
        {
            if (_cam)
            {
                _cam.orthographicSize = _camSize;
                _cam.transform.position = transform.position;
                _cam.transform.rotation = transform.rotation;
            }
        }
    }
}
