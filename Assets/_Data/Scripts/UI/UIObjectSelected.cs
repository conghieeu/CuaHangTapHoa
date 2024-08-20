using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CuaHang.UI
{
    public class UIObjectSelected : MonoBehaviour
    {
        public Item _item;
        public RectTransform _uiContent;
        public TextMeshProUGUI _tmp;

        [SerializeField] RaycastCursor rc;
        [SerializeField] string _defaultTmp;
        [SerializeField] BtnPressHandler _btnIncreasePrice;
        [SerializeField] BtnPressHandler _btnDiscountPrice;

        void Start()
        {
            _defaultTmp = _tmp.text;
            rc = SingleModuleManager.Instance._raycastCursor;

            _btnIncreasePrice.OnButtonDown += BtnDownIncreasePrice;
            _btnIncreasePrice.OnButtonHolding += BtnHoldIncreasePrice;

            _btnDiscountPrice.OnButtonDown += BtnDownDiscountPrice;
            _btnDiscountPrice.OnButtonHolding += BtnHoldDiscountPrice;
        }

        void FixedUpdate()
        {
            // Get Item selected
            if (rc._itemFocus)
            {
                _item = rc._itemFocus.GetComponentInChildren<Item>();

                if (_item && _item._SO)
                {
                    string x = $"Name: {_item._name} \nPrice: {_item._price.ToString("F1")} \n";
                    _tmp.text = _item._SO._isCanSell ? x + "Item có thể bán" : x + "Item không thể bán";
                }
                else
                {
                    _tmp.text = _defaultTmp;
                }
            }

            // bật tắt tuỳ theo có item hay không
            if (_item) _uiContent.gameObject.SetActive(_item);

        }

        // --------------BUTTON--------------

        public void BtnDownIncreasePrice()
        {
            if (_item) _item.SetPrice(0.1f);
        }
        public void BtnHoldIncreasePrice()
        {
            if (_item) _item.SetPrice(0.1f);
        }

        public void BtnDownDiscountPrice()
        {
            if (_item) _item.SetPrice(-0.1f);
        }
        public void BtnHoldDiscountPrice()
        {
            if (_item) _item.SetPrice(-0.1f);
        }

    }
}
