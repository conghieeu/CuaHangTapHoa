using System.Collections;
using System.Collections.Generic;
using CuaHang.AI;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class CustomerPooler : ObjectPooler
    {
        public static CustomerPooler Instance { get; private set; }

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }
        }



    }
}