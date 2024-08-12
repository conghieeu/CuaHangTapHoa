using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CuaHang
{
    public class UIObjectSelected : MonoBehaviour
    {
        public Item _item;
        public RectTransform _uiContent;
        public TextMeshProUGUI _tmp;

        private void Update()
        {
            RaycastCursor rc = SingleModuleManager.Instance._raycastCursor;

            // Get Item selected
            if (rc._itemFocus)
            {
                _item = rc._itemFocus.GetComponentInChildren<Item>(); 
                
                if (_item)
                {
                    _tmp.text = _item._price.ToString();
                }
            }

            // bật tắt tuỳ theo có item hay không
            if(_item) _uiContent.gameObject.SetActive(_item);

            // thoát _item
            
        }

        

    }
}
