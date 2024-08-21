using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public class PoolObject : HieuBehavior
    {
        [Header("POOL OBJECT")]
        public string _ID;
        public string _name;
        public bool _isRecyclable;
    }
}
