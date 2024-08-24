using System.Collections.Generic;
using UnityEngine;

namespace CuaHang.Pooler
{
    public enum TypePool
    {
        Item,
        Customer,
        Staff,
    }

    public class ObjectPooler : MonoBehaviour
    {
        [Header("BoolingObjects")]
        public TypePool _poolType;
        [SerializeField] protected List<Transform> _prefabs;
        [SerializeField] private List<ObjectPool> _objectPools; 

        public List<ObjectPool> _ObjectPools { get => _objectPools; private set => _objectPools = value; }

        protected virtual void Start()
        {
            // load childen item
            foreach (Transform child in transform)
            {
                _objectPools.Add(child.GetComponent<ObjectPool>());
            }
        }

        /// <summary> Xoá object trong hồ </summary>
        public virtual void RemoveObject(ObjectPool objectPool)
        {
            objectPool.gameObject.SetActive(false);
            objectPool._isRecyclable = true;
        }

        public virtual bool IsContentID(string id)
        {
            foreach (var obj in _ObjectPools)
            {
                if (obj._ID == id) return true;
            }
            return false;
        }

        public ObjectPool GetObjectPool(TypeID typeID)
        {
            ObjectPool objectPool = GetHider(typeID);

            if (objectPool) // use old
            {
                objectPool.gameObject.SetActive(true);
            }
            else // Create New 
            {
                foreach (var prefab in _prefabs)
                {
                    ObjectPool pO = prefab.GetComponent<ObjectPool>();

                    if (pO && pO._typeID == typeID)
                    {
                        objectPool = Instantiate(pO, transform);
                        _objectPools.Add(objectPool);
                        break;
                    }
                }
            }

            if (objectPool) objectPool.CreateID();
            else  Debug.LogWarning($"Item {typeID} Này Tạo từ pool không thành công");

            return objectPool;
        }

        /// <summary> Tìm object nghỉ </summary>
        private ObjectPool GetHider(TypeID typeID)
        {
            foreach (var objectPool in _objectPools)
            {
                if (objectPool._typeID == typeID && objectPool._isRecyclable)
                    return objectPool;
            }
            return null;
        }
    }

}