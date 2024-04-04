using System.Collections;
using System.Collections.Generic;
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
        public Rigidbody _rb;
        public Vector3 _inputVector;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.angularDrag = 0.0f;
        }

        void Update()
        {
            SetInputVector();
        }

        void FixedUpdate()
        {
            Vector3 moveVector = _inputVector * _moveSpeed * Time.deltaTime;
            _rb.velocity = moveVector;
        }

        void SetInputVector()
        {
            _inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            _inputVector =  _cameraTransform.TransformDirection(_inputVector).normalized;
        }
    }
}
