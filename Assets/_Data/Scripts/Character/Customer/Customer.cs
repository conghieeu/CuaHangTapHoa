using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang.AI
{
    public class Customer : AIBehavior
    {
        [Header("Customer")]
        public float _totalPay;
        public Item _itemFinding; // item mà khách hàng đang tìm
        public Transform _slotWaiting; // Hàng chờ (WaitingLine) modun của máy tính sẽ SET thứ này
        public Transform _outShopPoint; // Là điểm sẽ tới nếu rời shop
        public CustomerStats _customerStats; // customer stats moduel
        public bool _isNotNeedBuy; // Không cần mua gì nữa
        public bool _isPickingItem; // Khi Khách hàng đang pick item
        public bool _playerConfirmPay; // Player xác nhận thanh toán
        public bool _isPay;

        public List<TypeID> _listItemBuy; // Cac item can lay, giới hạn là 15 item
        public List<Item> _itemsCard; // cac item da mua

        protected override void Awake()
        {
            base.Awake();
            _customerStats = GetComponent<CustomerStats>();
        }

        protected override void Start()
        {
            base.Start();
            _outShopPoint = GameObject.Find("GO OUT SHOP POS").transform;
        }

        private void FixedUpdate()
        {
            if (_isPickingItem) return;

            SetItemNeed();

            // set item finding
            if (!_itemFinding || !_itemFinding.gameObject.activeSelf)
            {
                foreach (var typeID in _listItemBuy)
                {
                    _itemFinding = ItemPooler.Instance.ShuffleFindItem(typeID);
                }
            }

            // điều kiện rời cửa hàng
            if (_listItemBuy.Count > 0) _isNotNeedBuy = false;
            else _isNotNeedBuy = true;

            Behavior();
            SetAnimation();
        }

        /// <summary> Hành vi </summary>
        void Behavior()
        {
            // đi lấy item thứ cần mua
            if (GoToItemNeed() && IsAgreeItem() && !_isNotNeedBuy)
            {
                In("1.1: Đang đi lấy thứ muốn mua");

                StartCoroutine(IsPickingItem());
                _totalPay += _itemFinding._price;
                _itemsCard.Add(_itemFinding);
                _itemFinding._itemParent._itemSlot.RemoveItemInList(_itemFinding);
                _itemFinding.gameObject.SetActive(false);
                _listItemBuy.Remove(_itemFinding._typeID);
                _itemFinding = null;

                return;
            }

            // không mua được nữa nhưng có item nên là thanh toán
            if (_itemsCard.Count > 0 && !_itemFinding)
            {
                In("1.2: Mua được vài thứ, đi thanh toán");
                _listItemBuy.Clear();

                ConfirmPayItem();

                if (_isPay)
                {
                    GoOutShop();
                }
                else
                {
                    IsMoveToWating();
                }
                return;
            }

            // không mua được gì Out shop
            if (_itemsCard.Count == 0 && !_itemFinding)
            {
                In("1.3: không mua được gì Out shop");
                GoOutShop();
                return;
            }
        }

        // -----------PUBLIC-----------

        /// <summary> Set Properties with Item Data </summary>
        public void SetProperties(CustomerData data)
        {
            _ID = data._id;
            _isNotNeedBuy = data._isNotNeedBuy;
            _isPay = data._isPay;
            _name = data._name;
            _playerConfirmPay = data._playerConfirmPay;
            transform.position = data._position;
            transform.rotation = data._rotation;
            _totalPay = data._totalPay;

            // chưa set lại player đã mua những gì 
        }

        /// <summary> Player xác nhận thanh toán với khách hàng này </summary>
        public void PlayerConfirmPay()
        {
            _isNotNeedBuy = true;
            _playerConfirmPay = true;
        }

        // -----------PRIVATE-----------
        private bool IsMoveToWating()
        {
            _mayTinh._waitingLine.RegisterSlot(this); // Đăng ký slot 
            if (_slotWaiting) // move
            {
                MoveToTarget(_slotWaiting);
                return true;
            }
            return false;
        }

        private void SetAnimation()
        {
            // is pick item
            if (_isPickingItem && _stageAnim != STATE_ANIM.Picking)
            {
                _stageAnim = STATE_ANIM.Picking;
                SetAnim();
                return;
            }

            // Idle
            if (_navMeshAgent.velocity.sqrMagnitude == 0 && _stageAnim != STATE_ANIM.Idle)
            {
                _stageAnim = STATE_ANIM.Idle;
                SetAnim();
                return;
            }

            // Walk
            if (_navMeshAgent.velocity.sqrMagnitude > 0.1f && _stageAnim != STATE_ANIM.Walk)
            {
                _stageAnim = STATE_ANIM.Walk;
                SetAnim();
                return;
            }
        }

        /// <summary> Chọn ngẫu nhiên item mà khách hàng này muốn lấy </summary>
        private void SetItemNeed()
        {
            if (_listItemBuy.Count == 0 && _itemsCard.Count == 0) // đk để được set danh sách mua
            {
                if (_listItemBuy.Count >= 0) _listItemBuy.Clear(); // Item muốn mua không còn thì reset ds

                // Tạo một số ngẫu nhiên giữa minCount và maxCount
                int countBuy = UnityEngine.Random.Range(3, 3);

                // Thêm danh sach item muon mua
                for (int i = 0; i < countBuy; i++)
                {
                    _listItemBuy.Add(GetRandomItemBuy());
                }
            }
        }

        private TypeID GetRandomItemBuy()
        {
            Debug.Log($"Chỗ này chưa xong");
            return TypeID.apple_1;
        }

        /// <summary> Chạy tới vị trí item cần lấy </summary>
        private bool GoToItemNeed()
        {
            Item target = _itemPooler.GetItemContentItem(_itemFinding); // lấy cái bàn chứa quả táo
            if (target == null)
            {
                _itemFinding = null;
                return false;
            }

            Transform movePoint = null;
            if (target._waitingPoint)
            {
                movePoint = target._waitingPoint;
            }
            else
            {
                movePoint = target.transform;
            }

            if (MoveToTarget(movePoint)) return true;
            return false;
        }

        /// <summary> Ra về khách tìm điểm đến là ngoài ở shop </summary>
        private void GoOutShop()
        {
            if (MoveToTarget(_outShopPoint))
            {
                // xoá tắt cả item dang giữ
                foreach (var item in _itemsCard)
                {
                    ItemPooler.Instance.RemoveObject(item);
                }
                CustomerPooler.Instance.RemoveObject(this);
            }
        }

        /// <summary> Giá quá cao thì không đồng ý mua </summary>
        private bool IsAgreeItem()
        {
            if (_itemFinding)
            {
                if (_itemFinding._price > _itemFinding._SO._priceMarketMax)
                {
                    ExpressedComplaintsItem();
                    _isNotNeedBuy = true;
                    return false;
                }
            }

            return true;
        }

        /// <summary> Thanh toán tiền, trả true nếu thanh toán thành công </summary>
        private bool ConfirmPayItem()
        {
            if (_isPay) return true;
            if (_playerConfirmPay)
            {
                _isPay = true;
                _mayTinh._waitingLine.CancelRegisterSlot(this);
                return true;
            }
            return false;
        }

        /// <summary> Expressed complaints because this product is too expensive </summary>
        private void ExpressedComplaintsItem()
        {
            Debug.Log("Bán gì mắt vậy cha");
        }

        /// <summary> Delay time pickup item </summary>
        private IEnumerator IsPickingItem()
        {
            _isPickingItem = true;
            yield return new WaitForSeconds(2f);
            _isPickingItem = false;
        }



    }
}
