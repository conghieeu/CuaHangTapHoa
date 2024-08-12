using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public class SingleModuleManager : MonoBehaviour
    {
        public ObjectDrag _objectDrag;
        public RaycastCursor _raycastCursor;
        public PlayerCtrl _playerCtrl;

        public static SingleModuleManager Instance;

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }
        }
    }
}
