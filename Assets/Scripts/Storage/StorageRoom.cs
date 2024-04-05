using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hieu
{
    public class StorageRoom : MonoBehaviour
    {
        public List<Transform> _slots;

        public Transform GetSlotEmpty()
        {
            return _slots.Find(child => child.childCount == 0);
        }
    }
}