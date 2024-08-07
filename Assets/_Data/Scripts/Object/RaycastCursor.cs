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
        public float _rotationSpeed = 10.0f; // Tốc độ xoay
        public float _snapDistance = 6f; // Khoảng cách cho phép đặt 
        public float _tileSize = 1;
        public Vector3 _tileOffset = Vector3.zero;
        public LayerMask _layerMask;
        public RaycastHit _hit;
        public RaycastHit[] _hits;
        Camera cam;

        private void Awake()
        {
            cam = Camera.main;
        }

        void Update()
        {
            SetRayHit();
            SetItemFocus();
            SetItemDrag();

            MoveItemDrag();
            RotationItemDrag();
        }

        void FixedUpdate()
        {
            CanNotPlant();
        }

        /// <summary> Chiếu tia raycast lấy dữ liệu cho _Hit </summary>
        private void SetRayHit()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out _hit, 100, _layerMask);
            _hits = Physics.RaycastAll(ray, 100f, _layerMask);
            In($"You Hit {_hit.transform}");
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

        /// <summary> Set thông số trường hợp cho không thể đặt </summary>
        void CanNotPlant()
        {
            if (!_objDrag) return;

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
        void SetItemDrag()
        {
            if (!_itemFocus || !Input.GetKeyDown(KeyCode.E)) return;

            Item item = _itemFocus.transform.GetComponent<Item>();

            if (item) if (item._isCanDrag)
                {
                    item.DragItem();
                    _objDrag.PickUpItem(item);
                }
        }

        /// <summary> Di chuyen item </summary>
        void MoveItemDrag()
        {
            //  Làm tròn vị trí temp để nó giống snap
            if (_enableSnapping)
            {
                Vector3 hitPoint = _hit.point;

                float sX = Mathf.Round(hitPoint.x / _tileSize) * _tileSize + _tileOffset.x;
                float sZ = Mathf.Round(hitPoint.z / _tileSize) * _tileSize + _tileOffset.z;
                float sY = Mathf.Round(hitPoint.y / _tileSize) * _tileSize + _tileOffset.y;

                Vector3 snappedPosition = new Vector3(sX, sY, sZ);
                _objDrag.transform.position = snappedPosition;
            }
            else
            {
                _objDrag.transform.position = _hit.point;
            }
        }

        /// <summary> Xoay item </summary>
        void RotationItemDrag()
        {
            _objDrag.transform.rotation = Quaternion.FromToRotation(Vector3.up, _hit.normal);

            // rotate model
            if (_objDrag)
            {
                if (_objDrag._modelsHolding)
                {
                    float scrollValue = Input.mouseScrollDelta.y;
                    float rotationAngle = scrollValue * _rotationSpeed;
                    _objDrag._modelsHolding.Rotate(Vector3.up, rotationAngle);
                }
            }
        }
    }

}
