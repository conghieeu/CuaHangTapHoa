using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public class HieuBehavior : MonoBehaviour
    {
        [Header("HieuBehavior")]
        [SerializeField] private bool enableDebugLog;

        protected void Log(String value)
        {
            if (enableDebugLog) Debug.Log(value, transform);
        }
    }

}