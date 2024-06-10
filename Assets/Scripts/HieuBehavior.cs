using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public class HieuBehavior : MonoBehaviour
    {
        [Header("HieuBehavior")]
        [SerializeField] private bool enablePrint;

        protected void Print(String value)
        {
            if (enablePrint) Debug.Log(value);
        }
    }

}