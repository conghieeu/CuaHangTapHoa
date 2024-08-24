using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CuaHang
{
    public class CameraControl : MonoBehaviour
    {
        public float _camSizeDefault = 5;
        public float _rotationSpeed;
        public float _moveSpeed;
        public Transform _characterFollow;
        public Transform _objectFollow; // là đối tượng theo giỏi object forcus
        public Transform _cameraHolder;
        public Item _itemEditing;
        public ItemDrag _objectDrag;
        public RaycastCursor _raycastCursor;
        public Camera _cam;
        public bool _isTargetToCamHere;

        public static event Action<Item> _EventOnEditItem;

        private void Start()
        {
            _characterFollow = PlayerCtrl.Instance.transform;
            _cam = Camera.main;
        }

        private void Update()
        {
            if (_isTargetToCamHere == false) CamCtrl();

            SetCamFocus(_raycastCursor._itemFocus);
            CamForcusShelf(_raycastCursor._itemFocus);

        }

        private void CamCtrl()
        {
            // Move forward character follow
            _objectFollow.position = Vector3.MoveTowards(_objectFollow.position, _characterFollow.position, _moveSpeed * Time.deltaTime);
            _cam.transform.position = _cameraHolder.position;
            _cam.transform.rotation = _cameraHolder.rotation;

            // Kiểm tra nếu giữ chuột phải
            if (Input.GetMouseButton(1))
            {
                // Lấy giá trị delta của chuột (sự thay đổi vị trí chuột)
                float mouseX = Input.GetAxis("Mouse X");

                // Xoay đối tượng quanh trục Y dựa trên giá trị delta của chuột
                _objectFollow.Rotate(Vector3.up, mouseX * _rotationSpeed, Space.Self);
            }
        }

        /// <summary> Cam tập trung vào đối tượng Item </summary>
        private void SetCamFocus(Transform itemF)
        {
            // Thoát trạng thái tập trung của cam
            if (Input.GetKeyDown(KeyCode.BackQuote) || _objectDrag.gameObject.activeInHierarchy)
            {
                ResetCharacterCamFocus(PlayerCtrl.Instance.transform);
            }

            // F để tập trung vào đối tượng
            if (itemF && Input.GetKeyDown(KeyCode.F))
            {
                Item item = itemF.GetComponent<Item>();

                if (item)
                {
                    _characterFollow = itemF;
                }
            }
        }

        private void ResetCharacterCamFocus(Transform chFocus)
        {
            _characterFollow = chFocus;
            _isTargetToCamHere = false;
            _cam.orthographicSize = _camSizeDefault;

            if (_itemEditing)
            {
                _itemEditing.SetEditMode(false);
                _itemEditing = null;
                _EventOnEditItem?.Invoke(null);
            }
        }

        /// <summary> cam tập trung vào kệ hàng để điều chỉnh giá sản phẩm </summary>
        private void CamForcusShelf(Transform itemF)
        {
            if (itemF && Input.GetKeyDown(KeyCode.Z))
            {
                Item item = itemF.GetComponentInChildren<Item>();

                if (item && !_isTargetToCamHere)
                {
                    if (!item._camHere)
                    {
                        Debug.LogWarning($"Đối tượng này không có _camHere");
                        return;
                    }

                    _itemEditing = item;
                    _isTargetToCamHere = true;
                    item.SetEditMode(true);
                    _EventOnEditItem?.Invoke(item);
                    return;
                }

                if (_isTargetToCamHere && _itemEditing)
                {
                    ResetCharacterCamFocus(_itemEditing.transform);
                }
            }
        }


    }
}
