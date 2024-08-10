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
        public Transform _cam;

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

        [SerializeField] Vector3 _moveDir;

        Rigidbody _rb;
        PlayerCtrl _ctrl;

        void Awake()
        {
            _ctrl = GetComponent<PlayerCtrl>();
            _rb = GetComponent<Rigidbody>();
            _cam = Camera.main.transform;
            _rb.angularDrag = 0.0f; // lực cản khi xoay vật
        }

        void Update()
        {
            SetAnimator();
        }

        void FixedUpdate()
        {
            Movement();
        }

        private void Movement()
        {
            // Input
            float horInput = Input.GetAxisRaw("Horizontal");
            float verInput = Input.GetAxisRaw("Vertical");  

            // camera dir
            Vector3 camForward = _cam.forward;
            Vector3 camRight = _cam.right;

            camForward.y = 0;
            camRight.y = 0;

            // creating relate cam direction
            Vector3 forwardRelative = verInput * camForward;
            Vector3 rightRelative = horInput * camRight;

            _moveDir = (forwardRelative + rightRelative).normalized;

            // movement 
            Vector3 velocity = new Vector3(_moveDir.x, _rb.velocity.y, _moveDir.z) * _moveSpeed;

            _rb.velocity = velocity;

            // Trường hợp đang kéo thả Item nào đó
            if (_rb.velocity.magnitude > 0 && !_ctrl._objectDrag._isDragging)
            {
                velocity.y = 0;
                transform.forward = velocity;
            }
        }

        void SetAnimator()
        {
            Animator anim = _ctrl._anim;

            // animation idle
            if (_moveDir == Vector3.zero && _stageAnim != STATE_ANIM.Idle)
            {
                _stageAnim = STATE_ANIM.Idle;
                SetAnim();
                return;
            }

            // animation walk
            if (_moveDir != Vector3.zero && _stageAnim != STATE_ANIM.Walk)
            {
                _stageAnim = STATE_ANIM.Walk;
                SetAnim();
                return;
            }
        }

        void SetAnim() => _ctrl._anim.SetInteger("State", (int)_stageAnim);
    }
}
