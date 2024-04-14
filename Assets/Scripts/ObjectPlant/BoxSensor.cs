using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CuaHang
{
    public class BoxSensor : MonoBehaviour
    {
        public List<Transform> _hits;
        // [SerializeField] List<String> _tags;
        [SerializeField] Vector3 _size;
        public UnityEvent _eventTrigger;

        private void Update()
        {
            DetectTarget();
        }

        void DetectTarget()
        {
            if (!GetHits().SequenceEqual(_hits))
            {
                _hits = GetHits();
                Debug.Log("BoxSensor: _eventTrigger.Invoke đã được gọi");
                _eventTrigger.Invoke();
            }
        }

        List<Transform> GetHits()
        {
            RaycastHit[] hits = Physics.BoxCastAll(transform.position, _size / 2f, transform.forward, transform.rotation, 0f);

            return hits.Select(x => x.transform).ToList();
        }

        protected void OnDrawGizmosSelected()
        {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, _size);
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(Vector3.zero, _size);
        }
    }
}