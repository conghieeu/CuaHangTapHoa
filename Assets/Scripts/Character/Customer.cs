using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CuaHang.Pooler;
using Unity.VisualScripting;
using UnityEngine;
using System;

namespace CuaHang.AI
{

    public class Customer : AIBehavior
    {
        [Header("Customer")]
        public float _totalPay;
        public Item _itemFinding;
        public Transform _slotWaiting; // Hàng chờ (WaitingLine) modun của máy tính sẽ SET thứ này
        public Transform _outShopPoint; // Là điểm sẽ tới nếu rời shop
        public bool _isNotNeedBuy; // Không cần mua gì nữa
        [SerializeField] private bool _playerConfirmPay; // Player xác nhận thanh toán
        public List<TypeID> _listItemBuy; // Cac item can lay, giới hạn là 15 item
        public List<Item> _itemsCard;
        bool _isPay;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            _outShopPoint = GameObject.Find("Point out shop").transform;
        }

        private void FixedUpdate()
        {
            SetItemNeed();

            // set item finding
            if (!_itemFinding || !_itemFinding.gameObject.activeSelf)
            {
                foreach (var typeID in _listItemBuy)
                {
                    _itemFinding = FindItem(typeID);
                }
            }

            // điều kiện rời cửa hàng
            if (_listItemBuy.Count > 0) _isNotNeedBuy = false;
            else _isNotNeedBuy = true;

            Behavior();
        }


        /// <summary> Player xác nhận thanh toán với khách hàng này </summary>
        public void PlayerConfirmPay()
        {
            _isNotNeedBuy = true;
            _playerConfirmPay = true;
        }

        /// <summary> Hành vi </summary>
        void Behavior()
        {
            // đi lấy item thứ cần mua
            if (GoToItemNeed() && IsAgreeItem() && !_isNotNeedBuy && _itemFinding)
            {
                In("1.1: Đang đi lấy thứ muốn mua");
                AddItemToCart();
                return;
            }

            // không mua được nữa nhưng có item nên là thanh toán
            if (_itemsCard.Count > 0 && !_itemFinding)
            {
                In("1.2: Mua được vài thứ, đi thanh toán");
                _listItemBuy.Clear();
                if (GoPayItem()) GoOutShop();
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

        /// <summary> Chọn ngẫu nhiên item mà khách hàng này muốn lấy </summary>
        void SetItemNeed()
        {
            if (_listItemBuy.Count == 0 && _itemsCard.Count == 0) // đk để được set danh sách mua
            {
                if (_listItemBuy.Count >= 0) _listItemBuy.Clear(); // Item muốn mua không còn thì reset ds

                // Tạo một số ngẫu nhiên giữa minCount và maxCount
                int countBuy = UnityEngine.Random.Range(3, 3);

                // Thêm đối tượng vào danh sách ngẫu nhiên số lần
                for (int i = 0; i < countBuy; i++)
                {
                    if (FindItem(GetRandomItemBuy()))
                        _listItemBuy.Add(GetRandomItemBuy());
                }
            }

        }

        TypeID GetRandomItemBuy()
        {
            // Lấy itemTypeID ngẫu nhiên (tạm cố định mỗi cái apple_1)
            // Array values = Enum.GetValues(typeof(TypeID));
            // System.Random random = new System.Random();
            // TypeID randomTypeID = (TypeID)values.GetValue(random.Next(values.Length)); 
            return TypeID.apple_1;
        }

        /// <summary> Đi thanh toán item </summary>
        bool GoPayItem()
        {
            if (GoWaitingSlots()) if (PayItem()) return true;
            return false;
        }

        /// <summary> Chạy tới vị trí item cần lấy </summary>
        bool GoToItemNeed()
        {
            _itemTarget = _itemPooler.FindShelfContentItem(_itemFinding); // lấy cái bàn chứa quả táo 
            if (IsHitItemTarget()) return true;
            MoveToTarget();
            return false;
        }

        /// <summary> Ra về khách tìm điểm đến là ngoài ở shop </summary>
        void GoOutShop()
        {
            if (MoveToTarget(_outShopPoint))
            {
                CustomerPooler.Instance.DeleteObject(transform);

                // xoá tắt cả item đăng giữ
                foreach (var item in _itemsCard)
                {
                    item.SetParent(null, null, false);
                }
            }
        }

        /// <summary> Tìm slot đợi thanh toán </summary>
        bool GoWaitingSlots()
        {
            if (_isPay) return true; // thanh toán rồi thì không cần đén hàng chờ 

            _mayTinh._waitingLine.RegisterSlot(this); // Đăng ký slot 
            if (_slotWaiting == null) return false;

            return MoveToTarget(_slotWaiting);
        }

        /// <summary> Giá quá cao thì không đồng ý mua </summary>
        bool IsAgreeItem()
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

        /// <summary> Expressed complaints because this product is too expensive </summary>
        void ExpressedComplaintsItem()
        {
            Debug.Log("Bán gì mắt vậy cha");
        }

        /// <summary> Thanh toán tiền, trả true nếu thanh toán thành công </summary>
        bool PayItem()
        {
            if (_isPay) return true;
            if (_playerConfirmPay)
            {
                _isPay = true;
                _gameManager.AddCoin(_totalPay);
                _mayTinh._waitingLine.CancelRegisterSlot(this);
                return true;
            }
            return false;
        }

        void AddItemToCart()
        {
            _totalPay += _itemFinding._price;
            _itemsCard.Add(_itemFinding);
            _itemFinding._itemParent._itemSlot.CustomerAddItem(_itemFinding);
            _listItemBuy.Remove(_itemFinding._typeID);
            _itemFinding = null;
        }

        /// <summary> Tìm item lần lượt theo mục đang muốn mua </summary>
        Item FindItem(TypeID typeID)
        {
            List<Item> poolItem = ItemPooler.Instance.GetPoolItem.ToList();
            poolItem.Shuffle<Item>();


            foreach (var i in poolItem)
            {
                if (!i) continue;
                if (!i._itemParent) continue;
                if (i._typeID == typeID && i._itemParent._type == Type.Shelf && i.gameObject.activeSelf) return i;
            }

            return null;
        }

    }
}