using System.Collections;
using System.Collections.Generic;
using CuaHang.AI;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class StaffPooler : ObjectPooler
    {
        public static StaffPooler Instance;

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }
        }

    }
}
