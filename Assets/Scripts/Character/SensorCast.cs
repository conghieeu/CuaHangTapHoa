using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CuaHang
{
    /// <summary> Sử dụng Physics.BoxCastAll để phát hiện va chạm </summary>
    public class SensorCast : MonoBehaviour
    {
        public LayerMask _layer;
        public List<Transform> _hits;
        public UnityEvent _eventTrigger;
        [SerializeField] protected Vector3 _size;

        private void Update()
        {
            DetectTarget();
        }

        /// <summary> Lấy đối tượng chạm đầu trong danh sách </summary>
        public Item GetItemTypeHit(Type type)
        {
            foreach (Transform hit in _hits)
            {
                Item iHit = hit.GetComponent<Item>(); 
                
                if (iHit)
                {
                    if (iHit._type == type)
                    {
                        return iHit;
                    }
                }
            }
            return null;
        }

        public Item GetObjectPlantHit()
        {
            foreach (var hit in _hits)
            {
                // kiểm tra chạm
                if (hit.GetComponent<Item>())
                {
                    return hit.GetComponent<Item>();
                }
            }
            return null;
        }

        /// <summary> Trigger va chạm thi chạm đối tượng, có sự thay đổi mới trong đối tượng va chạm mới thì event gọi các đối tượng đăng ký </summary>
        void DetectTarget()
        {
            if (!GetHits().SequenceEqual(_hits))
            {
                _hits = GetHits();
                _eventTrigger.Invoke();
            }
        }

        /// <summary> Gọi liên tục để lấy va chạm </summary>
        List<Transform> GetHits()
        {
            RaycastHit[] hits = Physics.BoxCastAll(transform.position, _size / 2f, transform.forward, transform.rotation, 0f, _layer);

            return hits.Select(x => x.transform).ToList();
        }

        // Vẽ box hit ra khi click vào thì thấy được box hit
        void OnDrawGizmosSelected()
        {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, _size);
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(Vector3.zero, _size);
        }
    }
}