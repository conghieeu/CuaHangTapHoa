using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public class SingleModuleManager : MonoBehaviour
    {
        public ItemDrag _objectDrag;
        public RaycastCursor _raycastCursor;
        public PlayerCtrl _playerCtrl;
        public Transform _itemSpawnPos;
        public Transform _pointOutShop;

        public static SingleModuleManager Instance;

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }
        }
    }
}
