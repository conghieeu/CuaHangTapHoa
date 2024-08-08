using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CuaHang
{
    public class PlayerCtrl : HieuBehavior
    {
        public ObjectDrag _objectDrag;
        public Transform _posHoldParcel; // vị trí đặt cái parcel này trên tay
        public SensorCast _sensorForward; // cảm biến đằng trước
        public Animator _anim;

        public static PlayerCtrl Instance { get; private set; }

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }


            _anim = GetComponentInChildren<Animator>();
        }
 
    }
}