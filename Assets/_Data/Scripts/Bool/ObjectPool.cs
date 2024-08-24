using System;
using UnityEngine;

namespace CuaHang
{
    public abstract class ObjectPool : HieuBehavior
    {
        [Header("POOL OBJECT")]
        public string _ID;
        public TypeID _typeID;
        public string _name;
        public bool _isRecyclable;

        public string GenerateIdentifier => System.Guid.NewGuid().ToString();

        /// <summary> tạo mã định danh </summary>
        [ContextMenu("CreateID")]
        public void CreateID()
        {
            _ID = GenerateIdentifier;
        }

    }
}
