using System.Collections.Generic;
using System.Linq;
using CuaHang.Pooler;
using UnityEngine; 


namespace CuaHang
{
    public class RootManager : MonoBehaviour
    {
        public List<ObjectPooler> _pooler;
        

        private void Reset()
        {
            _pooler = FindObjectsByType<ObjectPooler>(FindObjectsSortMode.None).ToList();
        }

        /// <summary> SAVE OBJECT ROOT </summary>
        public void SaveRoot()
        {

        }
    }
}