using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CuaHang
{
    public class PlayerPlanting : MonoBehaviour
    {
        public bool _isDragging;

        PlayerCtrl _ctrl;
        // khi dragging object temp thì thằng này sẽ hiện model của object đang drag nớ ra

        private void Awake()
        {
            _ctrl = GetComponent<PlayerCtrl>();
        }

        private void FixedUpdate()
        {
            _isDragging = _ctrl._temp.isActiveAndEnabled;

            OnDragging();
        }

        // TODO: Làm sao để model temp đang dragging nó hiện giống model đang di chuyển

        private void OnDragging()
        {
            if (_isDragging)
            {
                // _ctrl._modelTempHolding.gameObject.SetActive(true);
                Transform objTemp = _ctrl._temp._objPlantOnDrag;

                objTemp.SetParent(_ctrl._modelTempHolding);
                objTemp.localPosition = Vector3.zero;
                objTemp.localRotation = Quaternion.identity;
            }
            else
            {
                
            }
        }
    }
}
