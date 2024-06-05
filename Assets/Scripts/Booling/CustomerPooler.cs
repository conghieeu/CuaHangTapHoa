using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class CustomerPooler : ObjectPooler
    {
        public static CustomerPooler Instance;

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }
        }
        

    }

}