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
        [Serializable]
        public class ItemBuy
        {
            public TypeID _typeID;
            public bool _isGet; // Đã lấy được item này
            public bool _isNull; // Không mua được
        }

        [Header("Customer")]
        public float _totalPay;
        public Item _itemFinding;
        public Transform _slotWaiting; // Hàng chờ (WaitingLine) modun của máy tính sẽ SET thứ này
        public Transform _outShopPoint; // Là điểm sẽ tới nếu rời shop
        public bool _isNotNeedBuy; // Không cần mua gì nữa
        [SerializeField] private bool _playerConfirmPay; // Player xác nhận thanh toán
        public List<ItemBuy> _listItemBuy; // Cac item can lay, giới hạn là 15 item
        public List<Item> _itemsCard;
        bool _isPay;
        bool _resetItemsBuyCount; // Reset số lần reset item có thể mua

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
            if (!_itemFinding || !_itemFinding.gameObject.activeSelf) _itemFinding = FindItem();

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
            if (GoToItemNeed())
            {
                if (IsAgreeItem())
                {
                    In("1.1: Đang đi lấy thứ muốn mua");
                    AddItemToCart();
                    return;
                }
            }

            // không mua được nữa nhưng có item nên là thanh toán
            if (_itemsCard.Count > 0)
            {
                In("1.2: Mua được vài thứ, đi thanh toán");
                if (GoPayItem()) GoOutShop();
                return;
            }

            // không mua được gì Out shop
            if (_itemsCard.Count == 0)
            {
                In("1.3: không mua được gì Out shop");
                GoOutShop();
                return;
            }

        }

        /// <summary> Chọn ngẫu nhiên item mà khách hàng này muốn lấy </summary>
        void SetItemNeed()
        {
            if (_listItemBuy.Count == 0 || IsEmptyItemWantBuy()) // đk để được set danh sách mua
            {
                if (_listItemBuy.Count >= 0) _listItemBuy.Clear(); // Item muốn mua không còn thì reset ds

                // Tạo một số ngẫu nhiên giữa minCount và maxCount
                int randomCount = UnityEngine.Random.Range(3, 3);

                // Thêm đối tượng vào danh sách ngẫu nhiên số lần
                for (int i = 0; i < randomCount; i++)
                {
                    ItemBuy itemBuy = GetRandomItem();
                    _listItemBuy.Add(itemBuy);
                }
            }
            
        }

        ItemBuy GetRandomItem()
        {
            // Lấy itemTypeID ngẫu nhiên (tạm cố định mỗi cái apple_1)
            // Array values = Enum.GetValues(typeof(TypeID));
            // System.Random random = new System.Random();
            // TypeID randomTypeID = (TypeID)values.GetValue(random.Next(values.Length));
            TypeID randomTypeID = TypeID.apple_1;
            ItemBuy itemBuy = new ItemBuy();
            itemBuy._typeID = randomTypeID;
            return itemBuy;
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
            if (_isNotNeedBuy) return false;

            Item shelf = _itemPooler.FindShelfContentItem(_itemFinding); // lấy cái bàn chứa quả táo

            if (IsHitItemTarget())  // Kiem tra sensor có chạm vào itemTarget đang hướng tới không
            {
                _itemTarget = null; // Dừng di chuyển
                return true;
            }
            else
            {
                _itemTarget = shelf;
                MoveToTarget();
            }
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
            if (ItemNext() == null || _itemFinding == null) return false;

            if (_itemFinding._price < _itemFinding._SO._priceMarketMax) return true;
            else
            {
                ExpressedComplaintsItem();
                _isNotNeedBuy = true;
            }

            return false;
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

        /// <summary> chọn tiem cần lấy theo thứ tự </summary>
        ItemBuy ItemNext()
        {
            foreach (var item in _listItemBuy)
            {
                if (!item._isGet || !item._isNull) return item;
            }
            return null;
        }

        /// <summary> Không còn tìm được item nào muốn mua trong danh sách item mua</summary>
        bool IsEmptyItemWantBuy()
        {
            int countNull = 0;

            foreach (var item in _listItemBuy)
            {
                if (item._isNull) countNull++;
            }

            if (_listItemBuy.Count == countNull)
            {
                return true;
            }

            else return false;
        }

        void AddItemToCart()
        {
            _totalPay += _itemFinding._price;
            _itemsCard.Add(_itemFinding);
            _itemFinding._itemParent._itemSlot.CustomerAddItem(_itemFinding);

            foreach (var item in _listItemBuy)
            {
                if (!item._isGet && !item._isNull)
                {
                    item._isGet = true;
                    break;
                }
            }

            _itemFinding = null;
        }

        /// <summary> Tìm item lần lượt theo mục đang muốn mua </summary>
        Item FindItem()
        {
            List<Item> poolItem = ItemPooler.Instance.GetPoolItem.ToList();
            poolItem.Shuffle<Item>();

            foreach (var item in _listItemBuy)
            {
                if (!item._isGet && !item._isNull)
                {
                    foreach (var i in poolItem)
                    {
                        if (!i) continue;
                        if (!i._itemParent) continue;
                        if (i._typeID == item._typeID && i._itemParent._type == Type.Shelf && i.gameObject.activeSelf) return i;
                    }
                }
            }

            return null;
        }

    }
}