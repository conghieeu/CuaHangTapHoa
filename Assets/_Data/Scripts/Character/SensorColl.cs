using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CuaHang
{
    /// <summary> Sử dụng collider để phát hiện va chạm </summary>
    public class SensorColl : MonoBehaviour
    {
        [SerializeField] protected List<Transform> _hits;
        [SerializeField] protected Vector3 _size;
        public UnityEvent _eventTrigger;

        public List<Transform> _Hits { get => _hits; }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("IT HIT SOMETHING!");
            _eventTrigger.Invoke();
        }
    }

}