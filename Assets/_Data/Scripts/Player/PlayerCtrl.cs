using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CuaHang
{
    public class PlayerCtrl : Singleton<PlayerCtrl>
    {
        public PlayerStats _playerStats;
        public ItemDrag _objectDrag;
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
    }
}