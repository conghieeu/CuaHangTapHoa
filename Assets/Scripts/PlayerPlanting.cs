using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CuaHang
{
    public class PlayerPlanting : MonoBehaviour
    {
        PlayerCtrl _ctrl;
        // khi dragging object temp thì thằng này sẽ hiện model của object đang drag nớ ra

        private void Awake()
        {
            _ctrl = GetComponent<PlayerCtrl>();
        }

        // TODO: Làm sao để model temp đang dragging nó hiện giống model đang di chuyển

    }
}
