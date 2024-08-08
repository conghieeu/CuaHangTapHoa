using System;
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

        [Serializable]
        public enum STATE_ANIM
        {
            Idle = 0,
            Walk = 1,
            Picking = 2,
            Idle_Carrying = 3,
            Walk_Carrying = 4,
        }
        public STATE_ANIM _stageAnim;

        [SerializeField] Vector3 _inputVector;

        Rigidbody _rb;
        PlayerCtrl _ctrl;

        void Awake()
        {
            _ctrl = GetComponent<PlayerCtrl>();
            _rb = GetComponent<Rigidbody>();
            _rb.angularDrag = 0.0f; // lực cản khi xoay vật
        }

        void Update()
        {
            SetInputVector();
            SetAnimator();
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
                if (!_ctrl._objectDrag._isDragging) transform.forward = direction;
            }
        }

        void SetAnimator()
        {
            Animator anim = _ctrl._anim;

            // animation idle
            if (_inputVector == Vector3.zero && _stageAnim != STATE_ANIM.Idle)
            {
                _stageAnim = STATE_ANIM.Idle;
                SetAnim();
                return;
            }

            // animation walk
            if (_inputVector != Vector3.zero && _stageAnim != STATE_ANIM.Walk)
            {
                _stageAnim = STATE_ANIM.Walk;
                SetAnim();
                return;
            }
        }

        void SetAnim() => _ctrl._anim.SetInteger("State", (int)_stageAnim);

        void SetInputVector()
        {
            _inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            _inputVector = _cameraTransform.TransformDirection(_inputVector).normalized;
        }
    }
}
