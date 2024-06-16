using UnityEngine;

namespace CuaHang
{
    /// <summary> Aim con trỏ, có thể snap khi drag các item </summary>
    public class RaycastCursor : HieuBehavior
    {
        [Header("RaycastCursor")]
        public ObjectDrag _objDrag;
        public Transform _itemFocus;
        public bool _enableSnapping; // bật chế độ snapping
        public float _snapDistance = 6f; // Khoảng cách cho phép đặt 
        public float tilesize = 1;
        public Vector3 tileOffset = Vector3.zero;
        public LayerMask _layerMask;
        public RaycastHit _hit;
        Camera cam;

        private void Awake()
        {
            cam = Camera.main;
        }

        void Update()
        {
            SetRayHit();
            SetItemFocus();
            MoveItemDrag();

            if (_hit.transform) Log($"Object đang hit là {_hit.transform.name}");
        }

        /// <summary> Chiếu tia raycast lấy dữ liệu cho _Hit </summary>
        private void SetRayHit()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out _hit, 100, _layerMask);
        }

        void FixedUpdate()
        {
            DroppingObjectTemp();
        }

        /// <summary> Tạo viền khi click vào đối tượng để nó focus </summary>
        void SetItemFocus()
        {
            if (_hit.transform && Input.GetMouseButtonDown(0))
            {
                // chuyển đói tượng focus
                if (_itemFocus != _hit.transform && _itemFocus != null)
                    SetOutlines(_itemFocus, false);

                _itemFocus = _hit.transform;
                SetOutlines(_itemFocus, true);
            }

            if (_itemFocus && Input.GetKeyDown(KeyCode.Escape))
            {
                SetOutlines(_itemFocus, false);
                _itemFocus = null;
            }
        }

        /// <summary> Tìm outline trong đối tượng và bật tắt viền của nó </summary>
        void SetOutlines(Transform target, bool value)
        {
            foreach (Outline outline in target.GetComponentsInChildren<Outline>())
            {
                outline.enabled = value;
            }
        }

        /// <summary> Cho phép đặt item xuống </summary>
        void DroppingObjectTemp()
        {
            if (!_objDrag) return;

            SetTempRotation();
            SetSnapping();

            // khoảng cách bị quá dài
            if (Vector3.Distance(cam.transform.position, _hit.point) < _snapDistance)
            {
                _objDrag.GetComponent<ObjectDrag>()._isDistance = true;
            }
            else
            {
                _objDrag.GetComponent<ObjectDrag>()._isDistance = false;
            }
        }

        /// <summary> Bật item drag với item được _Hit chiếu</summary>
        void MoveItemDrag()
        {
            if (!_enableSnapping || !_itemFocus || !Input.GetKeyDown(KeyCode.E)) return;

            Item item = _itemFocus.transform.GetComponent<Item>();

            if (item)
            {
                if (!item._ThisParent && !_objDrag._modelsHolding && item._isCanDrag)
                {
                    item.DragItem();
                    _objDrag.PickUpObjectPlant();
                }
            }

        }

        /// <summary> Làm tròn vị trí temp để nó giống snap </summary>
        void SetSnapping()
        {
            Vector3 currentPosition = _objDrag.transform.position;

            float snappedX = Mathf.Round(currentPosition.x / tilesize) * tilesize + tileOffset.x;
            float snappedZ = Mathf.Round(currentPosition.z / tilesize) * tilesize + tileOffset.z;
            float snappedY = Mathf.Round(currentPosition.y / tilesize) * tilesize + tileOffset.y;

            Vector3 snappedPosition = new Vector3(snappedX, snappedY, snappedZ);
            _objDrag.transform.position = snappedPosition;
        }

        // Giúp xoay temp
        void SetTempRotation()
        {
            _objDrag.transform.position = _hit.point;
            _objDrag.transform.rotation = Quaternion.FromToRotation(Vector3.up, _hit.normal);
        }
    }

}
