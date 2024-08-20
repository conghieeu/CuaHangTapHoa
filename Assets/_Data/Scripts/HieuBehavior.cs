using UnityEngine;

namespace CuaHang
{
    public class HieuBehavior : MonoBehaviour
    {
        [Header("HIEU BEHAVIOR")]
        [SerializeField] private bool enableDebugLog;

        protected void In(object value)
        {
            if (enableDebugLog) Debug.Log(value, transform);
        }

        /// <summary> Đặt địa điểm Item này </summary> 
        public void SetLocation(Transform location)
        {
            transform.position = location.position;
            transform.rotation = location.rotation;
        }
    }

}