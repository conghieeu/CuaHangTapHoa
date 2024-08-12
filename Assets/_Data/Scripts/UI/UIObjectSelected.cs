using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CuaHang.UI
{
    public class UIObjectSelected : MonoBehaviour
    {
        public Item _item;
        public RectTransform _uiContent;
        public TextMeshProUGUI _tmp;

        [SerializeField] string _defaultTmp;

        private void Start() {
            _defaultTmp = _tmp.text;
        } 

        private void Update()
        {
            RaycastCursor rc = SingleModuleManager.Instance._raycastCursor;

            // Get Item selected
            if (rc._itemFocus)
            {
                _item = rc._itemFocus.GetComponentInChildren<Item>();

                if (_item)
                {
                    _tmp.text = $"Name: {_item._name} \n Price: {_item._Price.ToString("F1")} ";
                }
                else 
                {
                    _tmp.text = _defaultTmp;
                }
            }

            // bật tắt tuỳ theo có item hay không
            if (_item) _uiContent.gameObject.SetActive(_item);

            // thoát _item

        }

        public void Btn_IncreasePrice()
        {
            if (_item) _item.SetPrice(0.1f);
        }

        public void Btn_DiscountPrice()
        {
            if (_item) _item.SetPrice(-0.1f);
        }

    }
}
