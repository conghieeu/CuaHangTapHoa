using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    public class ObjectSell : MonoBehaviour
    {
        public ObjectSellSO _SO;
        public ObjectPlant _thisParent; // ObjectPlant sẽ là cha
        public string _typeID;
        public string _name;
        public string _type;

        [SerializeField] Transform _parent;

        public Transform _Parent => _parent;

        /// <summary> Làm con của thằng cha, đặt lại vị trí </summary>
        public void SetThisParent(Transform parent)
        {
            _parent = parent;

            if (parent)
            {
                transform.SetParent(parent);
            }
            else
            {
                transform.SetParent(BoolingObjPlants.Instance.transform);
            }
        }
    }

}