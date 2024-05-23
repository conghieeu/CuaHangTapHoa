using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CuaHang
{
    public class PlayerCtrl : HieuBehavior
    {
        public ObjectDrag _temp;
        public Transform _posHoldParcel; // vị trí đặt cái parcel này trên tay
        public SensorCast _sensor;

        public static PlayerCtrl Instance { get; private set; }

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }
        }

    }
}