using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using CuaHang.AI;
using TMPro;

namespace CuaHang.UI
{
    public class UIComputerScreen : MonoBehaviour
    {
        [Serializable]
        public class SlotBar
        {
            public Customer _customer;
            public BtnBarCustomer _bar;
        }

        public MayTinh _mayTinh;

        [Header("Buy Panel")]
        public Button _btnGoPayPanel;
        public RectTransform _panelBuyItem;

        [Header("Payment Panel")]
        public bool _isCanPay;
        public Button _btnGoBuyPanel;
        public Button _btnPay;
        public RectTransform _panelPayment;
        public RectTransform _panelSlotHolder;
        public RectTransform _prefabBtnSlot;
        public TextMeshProUGUI _txtCustomerValue;
        public TextMeshProUGUI _txtReport;
        public Customer _customerSelectMark;
        public TMP_InputField _infRefund;
        [SerializeField] float _tienThoi;
        [SerializeField] List<WaitingLine.WaitingSlot> _comSlot;
        public List<SlotBar> _barSlots;

        void Start()
        {
            CameraControl._EventOnEditItem += SetActiveContent;
            SetActiveContent(null);

            _btnGoPayPanel.onClick.AddListener(OnClickGoPayPanel);
            _btnGoBuyPanel.onClick.AddListener(OnClickGoBuyPanel);
            _btnPay.onClick.AddListener(OnClickPayBtn);
            _infRefund.onValueChanged.AddListener(ValidateInput);

            for (int i = 0; i < 10; i++) _barSlots.Add(new SlotBar());
        }

        void FixedUpdate()
        {
            CreateBtnSlot(); 
        }

        // -----------BUY PANEL----------

        // -----------PAYMENT PANEL----------

        public void SetActiveContent(Item item)
        { 
            if (item && item.GetComponent<MayTinh>())
            {
                if (_panelPayment) _panelPayment.gameObject.SetActive(true);
                _mayTinh = item.GetComponent<MayTinh>();
            }
            else
            {
                if (_panelPayment) _panelPayment.gameObject.SetActive(false);
                if (_panelBuyItem) _panelBuyItem.gameObject.SetActive(false);
                _mayTinh = null;
            }
        }

        // Hàm này sẽ được gọi mỗi khi giá trị trong inputField thay đổi
        private void ValidateInput(string input)
        {
            if (float.TryParse(input, out float tienThoi))
            {
                if (_customerSelectMark && 300 - tienThoi <= _customerSelectMark._totalPay)
                {
                    if (GameManager._Coin < tienThoi)
                    {
                        _txtReport.text = "Cảnh báo: Không đủ tiền để thối";
                    }
                    else
                    {
                        _txtReport.text = "Bạn có thể thanh toán, tính đúng nếu không mún bị mất tiền";
                        _tienThoi = tienThoi;
                        _isCanPay = true;
                        return;
                    }
                }
                else
                {
                    _txtReport.text = "Bạn đang lấy tiền của khách hoặc bạn chưa lựa chọn khách hàng để giao dịch";
                }
            }
            else
            {
                _txtReport.text = "Chuỗi ko hợp lệ: Chỉ chứa số.";
            }

            _isCanPay = false;
        }

        // tạo slot trong panel slot holder cho cho hop voi khach hang o hang cho
        private void CreateBtnSlot()
        {
            if (_mayTinh == null) return;

            _comSlot = _mayTinh._waitingLine._waitingSlots;

            // doi chieu su khach biet
            for (int c = 0; c < _comSlot.Count; c++)
            {
                if (_barSlots[c]._customer == _comSlot[c]._customer) continue;

                // Recreate list bar
                for (int i = 0; i < _comSlot.Count; i++)
                {
                    if (_barSlots[i]._bar) Destroy(_barSlots[i]._bar.gameObject);
                    // tao cai moi
                    _barSlots[i]._customer = _comSlot[i]._customer;

                    if (_barSlots[i]._customer)
                    {
                        BtnBarCustomer btnBar = Instantiate(_prefabBtnSlot, _panelSlotHolder).GetComponentInChildren<BtnBarCustomer>();
                        _barSlots[i]._bar = btnBar;
                        if (_comSlot[i]._customer) btnBar.SetCustomer(_comSlot[i]._customer);
                    }
                }
            }
        }

        private void OnClickPayBtn()
        {
            if (!_customerSelectMark) return;

            if (GameManager._Coin < _tienThoi)
            {
                _txtReport.text = "Cảnh báo: Không đủ tiền để thối";
                return;
            }

            if (_isCanPay)
            {
                _customerSelectMark.PlayerConfirmPay();

                float coinAdd = 300 - _tienThoi;

                if (GameManager._Coin >= _tienThoi)
                {
                    GameManager.AddCoin(coinAdd);
                }

                _customerSelectMark = null;
            }
        }

        private void OnClickGoPayPanel()
        {
            SetAtivePanel(_panelPayment);
        }

        private void OnClickGoBuyPanel()
        {
            SetAtivePanel(_panelBuyItem);
        }

        private void SetAtivePanel(Transform panel)
        {
            if (panel == _panelPayment)
            {
                _panelPayment.gameObject.SetActive(true);
                _panelBuyItem.gameObject.SetActive(false);
                return;
            }

            if (panel == _panelBuyItem)
            {
                _panelBuyItem.gameObject.SetActive(true);
                _panelPayment.gameObject.SetActive(false);
                return;
            }
        }
    }
}
