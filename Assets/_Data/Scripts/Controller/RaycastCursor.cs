using UnityEngine;
using UnityEngine.EventSystems;

namespace CuaHang
{
    /// <summary> Dùng raycast và drag các item </summary>
    public class RaycastCursor : HieuBehavior
    {
        [Header("RaycastCursor")]
        public ItemDrag _objectDrag;
        [SerializeField] Camera cam;

        [Space]
        public bool _enableSnapping; // bật chế độ snapping
        public bool _enableRaycast; // bat raycast
        public bool _enableOutline;
        public Transform _itemFocus;
        public float _rotationSpeed = 10.0f; // Tốc độ xoay
        public float _snapDistance = 6f; // Khoảng cách cho phép đặt 
        public float _tileSize = 1; // ô snap tỷ lệ snap
        public Vector3 _tileOffset = Vector3.zero; // tỷ lệ snap + sai số này
        public LayerMask _layerMask;

        public RaycastHit _hit;
        public RaycastHit[] _hits;

        private void Start()
        {
            cam = Camera.main;
            _enableRaycast = true;
            _enableOutline = true;
            _objectDrag = SingleModuleManager.Instance._objectDrag;
        }

        void Update()
        {
            SetRayHit();
            SetItemFocus();
            SetItemDrag();

            MoveItemDrag();
            RotationItemDrag();

            if (Input.GetKeyDown(KeyCode.N))
            {
                _enableSnapping = !_enableSnapping;
            }
        }

        void FixedUpdate()
        {
            CanNotPlant();
        }

        /// <summary> Chiếu tia raycast lấy dữ liệu cho _Hit </summary>
        private void SetRayHit()
        {
            if (_enableRaycast == false) return;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out _hit, 100, _layerMask);
            _hits = Physics.RaycastAll(ray, 100f, _layerMask);
            In($"You Hit {_hit.transform}");
        }

        /// <summary> Tạo viền khi click vào đối tượng để nó focus </summary>
        private void SetItemFocus()
        {
            if (_hit.transform && !_objectDrag._itemDragging && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                // chuyển đói tượng focus
                if (_itemFocus != _hit.transform && _itemFocus != null)
                {
                    SetOutlines(_itemFocus, false);
                }

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
        private void SetOutlines(Transform target, bool value)
        {
            foreach (Outline outline in target.GetComponentsInChildren<Outline>())
            {
                if (_enableOutline && value) outline.enabled = true;
                else outline.enabled = false;
            }
        }

        /// <summary> Set thông số trường hợp cho không thể đặt </summary>
        private void CanNotPlant()
        {
            if (!_objectDrag) return;

            // khoảng cách bị quá dài
            if (Vector3.Distance(cam.transform.position, _hit.point) < _snapDistance)
            {
                _objectDrag.GetComponent<ItemDrag>()._isDistance = true;
            }
            else
            {
                _objectDrag.GetComponent<ItemDrag>()._isDistance = false;
            }
        }

        /// <summary> Bật item drag với item được _Hit chiếu</summary>
        private void SetItemDrag()
        {
            if (!_itemFocus || !Input.GetKeyDown(KeyCode.E) || _objectDrag._isDragging) return;

            Item item = _itemFocus.transform.GetComponent<Item>();

            if (item && item._isCanDrag)
            {
                item.DragItem();
                _objectDrag.PickUpItem(item);
            }
        }

        /// <summary> Di chuyen item </summary>
        private void MoveItemDrag()
        {
            //  Làm tròn vị trí temp để nó giống snap
            if (_enableSnapping)
            {
                Vector3 hitPoint = _hit.point;

                float sX = Mathf.Round(hitPoint.x / _tileSize) * _tileSize + _tileOffset.x;
                float sZ = Mathf.Round(hitPoint.z / _tileSize) * _tileSize + _tileOffset.z;
                float sY = Mathf.Round(hitPoint.y / _tileSize) * _tileSize + _tileOffset.y;

                Vector3 snappedPosition = new Vector3(sX, sY, sZ);
                _objectDrag.transform.position = snappedPosition;
            }
            else
            {
                _objectDrag.transform.position = _hit.point;
            }
        }

        /// <summary> Xoay item </summary>
        private void RotationItemDrag()
        {
            _objectDrag.transform.rotation = Quaternion.FromToRotation(Vector3.up, _hit.normal);

            // rotate model
            if (_objectDrag)
            {
                if (_objectDrag._modelsHolding)
                {
                    float scrollValue = Input.mouseScrollDelta.y;
                    float rotationAngle = scrollValue * _rotationSpeed;
                    _objectDrag._modelsHolding.Rotate(Vector3.up, rotationAngle);
                }
            }
        }
    }

}
