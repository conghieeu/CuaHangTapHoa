using System;
using System.Collections;
using System.Collections.Generic;
using CuaHang;
using CuaHang.AI;
using UnityEngine;

namespace CuaHang.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Space]
        public float _moveSpeed;
        public Transform _cam;
        public STATE_ANIM _stageAnim;
        public bool _triggerDragging; // trigger player đang drag item

        [SerializeField] Vector3 _moveDir;

        Rigidbody _rb;
        PlayerCtrl _ctrl;

        private void Start()
        {
            _ctrl = GetComponent<PlayerCtrl>();
            _rb = GetComponent<Rigidbody>();
            _cam = Camera.main.transform;
            _rb.angularDrag = 0.0f; // lực cản khi xoay vật
        }

        private void FixedUpdate()
        {
            SetAnimator();
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

        private void SetAnimator()
        {
            bool _isDragItem = _ctrl._objectDrag.gameObject.activeInHierarchy;

            // Idle
            if (_moveDir == Vector3.zero && (_stageAnim != STATE_ANIM.Idle || _triggerDragging != _isDragItem))
            {
                if (_isDragItem) _stageAnim = STATE_ANIM.Idle_Carrying;
                else _stageAnim = STATE_ANIM.Idle;
                _triggerDragging = _isDragItem;
                SetAnim();
                return;
            }

            // Walk
            if (_moveDir != Vector3.zero && _stageAnim != STATE_ANIM.Walk || _triggerDragging != _isDragItem)
            {
                if (_isDragItem) _stageAnim = STATE_ANIM.Walk_Carrying;
                else _stageAnim = STATE_ANIM.Walk;
                _triggerDragging = _isDragItem;
                SetAnim();
                return;
            }
        }

        private void SetAnim() => _ctrl._anim.SetInteger("State", (int)_stageAnim);
    }
}
