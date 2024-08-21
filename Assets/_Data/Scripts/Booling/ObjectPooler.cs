using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class ObjectPooler : MonoBehaviour
    {
        [Header("BoolingObjects")]
        [SerializeField] protected List<Transform> _listPrefab;

        /// <summary> Xoá object trong hồ </summary>
        public virtual void RemovePoolObj(PoolObject obj)
        {
            obj.gameObject.SetActive(false);
            obj._isRecyclable = true;
        }

        
    }

}