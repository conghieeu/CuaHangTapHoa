using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CuaHang
{
    public class BoxSensorColl : MonoBehaviour
    {
        [SerializeField] List<Transform> _hits;
        // [SerializeField] List<String> _tags;
        [SerializeField] Vector3 _size;
        public UnityEvent _eventTrigger;

        public List<Transform> _Hits { get => _hits; }

        private void FixedUpdate()
        {
            DetectTarget();
        }

        void DetectTarget()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log(other);
        }
    }

}