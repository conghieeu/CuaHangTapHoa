using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CuaHang
{
    public class PlayerCtrl : MonoBehaviour
    {
        public ObjectTemp _temp;
        public Transform _modelTempHolding;

        public static PlayerCtrl Instance { get; private set; }

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }
        }

    }

}