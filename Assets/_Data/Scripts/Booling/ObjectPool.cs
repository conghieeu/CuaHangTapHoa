 
using UnityEngine;

namespace CuaHang
{
    public class ObjectPool : HieuBehavior
    {
        [Header("POOL OBJECT")]
        public string _ID;
        public TypeID _typeID;
        public string _name;
        public bool _isRecyclable;

        /// <summary> tạo mã định danh </summary>
        [ContextMenu("CreateID")]
        public void CreateID()
        {
            _ID = System.Guid.NewGuid().ToString();
        }

    }
}
