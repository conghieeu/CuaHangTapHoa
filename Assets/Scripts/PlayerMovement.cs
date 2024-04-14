using System.Collections;
using System.Collections.Generic;
using CuaHang;
using UnityEngine;

namespace Hieu.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float _moveSpeed;

        [Space]
        public Transform _cameraTransform;
        public Camera _camera;

        [Space]
        public PlayerCtrl _ctrl;
        Rigidbody _rb;
        [SerializeField] Vector3 _inputVector;

        void Awake()
        {
            _ctrl = GetComponent<PlayerCtrl>();
            _rb = GetComponent<Rigidbody>();
            _rb.angularDrag = 0.0f;
        }

        void Update()
        {
            SetInputVector();
        }

        void FixedUpdate()
        {

            Movement();

        }

        private void Movement()
        {
            Vector3 direction = _inputVector * _moveSpeed * Time.deltaTime;
            _rb.velocity = direction;

            // hướng nhân vật về phía hướng di chuyển
            if (_rb.velocity.magnitude > 0)
            {
                direction.y = 0;
                if (!_ctrl._temp._isDragging) transform.forward = direction;
            }
        }

        void SetInputVector()
        {
            _inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            _inputVector = _cameraTransform.TransformDirection(_inputVector).normalized;
        }
    }
}
