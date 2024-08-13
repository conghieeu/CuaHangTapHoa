using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CuaHang
{
    public class UIComputerScreen : MonoBehaviour
    {
        public RectTransform _content;
        public MayTinh _mayTinh;

        private void Start()
        {
            CameraControl._EventOnEditItem += SetActiveContent;
        }

        public void SetActiveContent(Item item)
        {
            if (item)
            {
                if (item.GetComponent<MayTinh>())
                {
                    _content.gameObject.SetActive(true);
                    return;
                }
            }

            _content.gameObject.SetActive(false);
        }
    }
}
