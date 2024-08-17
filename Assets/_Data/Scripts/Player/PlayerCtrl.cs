using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CuaHang
{
    public class PlayerCtrl : Singleton<PlayerCtrl>
    {
        public ObjectDrag _objectDrag;
        public Transform _posHoldParcel; // vị trí đặt cái parcel này trên tay
        public SensorCast _sensorForward; // cảm biến đằng trước
        public Animator _anim;
        public Interactor _interactor;

        protected override void Awake()
        {
            base.Awake();
            _anim = GetComponentInChildren<Animator>();
            _interactor = GetComponentInChildren<Interactor>();
        }

        private void Update()
        {
            Interactive();
        }

        // Interactive item
        private void Interactive()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                foreach (var hit in _sensorForward._hits)
                {
                    var interactable = hit.GetComponent<IInteractable>();

                    // kiểm tra chạm
                    if (interactable != null)
                    {
                        interactable.Interact(_interactor);
                    }
                }
            }
        }
    }
}