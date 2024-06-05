using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace CuaHang.AI
{
    public class Customer : AIBehavior
    {
        [Header("CustomerAI")]
        public Transform _slotWaiting; // Hàng chờ (WaitingLine) modun của máy tính sẽ SET thứ này
        public Transform _outShopPoint; // Là điểm sẽ tới nếu rời shop
        public bool _isNotNeedBuy; // Không cần mua gì nữa

        [SerializeField] private bool _playerConfirmPay; // Player xác nhận thanh toán

        public ItemSlot _itemSlot; // Dùng để lưu item va check item da lay duoc
        public List<Item> _itemsNeed; // Cac item can lay, giới hạn là 15 item

        bool _isConfirmPay;

        protected override void Awake()
        {
            base.Awake();
            _itemSlot = GetComponentInChildren<ItemSlot>();
        }

        protected override void Start()
        {
            base.Start();
            SetItemNeed();
        }

        private void FixedUpdate()
        {
            Behavior();
        }

        void Behavior()
        {
            if (!_itemSlot.ItemsSequenceEqual(_itemsNeed)) // Kiểm tra item cần mua
            {
                if (GoToItemNeed())
                {
                    if (IsAgreeItem())
                    {
                        Debug.Log("1.1: Lấy danh sách items");
                        GetItem();
                    }
                    else if (!IsAgreeItem() && _itemSlot.IsAnyItem()) // mua được vài thứ nên đi về 
                    {
                        Debug.Log("1.2: Thanh toan vi hang dom, thanh toan nhung mon do dang co");
                        if (GoPayItem()) GoOutShop();
                    }
                }
                else if (_isNotNeedBuy && _itemSlot.IsAnyItem() == false) // không mua được gì nên đi về
                {
                    Debug.Log("1.3: Đồ quá mắt không mua được gì đi về");
                    GoOutShop();
                }
            }
            else
            {
                Debug.Log("2: Thanh toan hang mua");
                if (GoPayItem()) GoOutShop();
                Debug.Log(GoPayItem());
            }
        }

        /// <summary> Player xác nhận thanh toán với khách hàng này </summary>
        public void SetPlayerConfirmPay()
        {
            _playerConfirmPay = true;
            _isNotNeedBuy = true;
            _mayTinh._waitingLine.CancelRegisterSlot(transform);
            Debug.Log("Player thanh toán cho khách hàng ở slot 1");
        }

        /// <summary> Lấy item từ Shelf đưa vào this._itemSlot </summary> 
        void GetItem()
        {
            if (ItemNeedGet() == null) return;

            Item itemNeedGet = ItemNeedGet();
            Item shelf = _itemPooler.FindItemContentProduct(ItemNeedGet());

            for (int i = 0; i < shelf._itemSlot._listItem.Count; i++)
            {
                Item shelfItem = shelf._itemSlot._listItem[i]._item;
                if (shelfItem == itemNeedGet)
                {
                    shelf._itemSlot.RemoveItemInList(shelfItem);
                    _itemSlot.AddItemToSlot(shelfItem);
                    _itemSlot.HideAllItems();
                }
            }
        }

        /// <summary> Đi thanh toán item </summary>
        bool GoPayItem()
        {
            if (GoSlotPayment())   // tìm hàng chờ
                if (PayItem()) // thanh toán item
                    return true;
            return false;
        }

        /// <summary> Chạy tới vị trí item cần lấy </summary>
        bool GoToItemNeed()
        {
            // Lay target
            Item itemGet = ItemNeedGet(); // lấy quả táo trong thế giới
            if (itemGet == null || _isNotNeedBuy) return false;

            Item shelf = _itemPooler.FindItemContentProduct(ItemNeedGet()); // lấy cái bàn chứa quả táo

            if (shelf == null) return false;

            // Kiem tra cham vao shelf
            if (GetItemHit() == shelf)
            {
                Debug.Log("Dung di chuyen");
                _ItemTarget = null; // Dừng di chuyển
                return true;
            }
            else
            {
                _ItemTarget = shelf; // di chuyển đến target
                MoveToTarget();
            }

            return false;
        }

        /// <summary> Ra về khách tìm điểm đến là ngoài ở shop </summary>
        void GoOutShop()
        {
            MoveToTarget(_outShopPoint);
        }

        /// <summary> Chọn ngẫu nhiên item mà khách hàng này muốn lấy </summary>
        void SetItemNeed()
        {
            // Lấy danh sách item mà cửa hàng đang có
            List<Item> shopItems = _itemPooler.GetAllItemsCanSell();
            Debug.Log("So item dang co the mua: " + shopItems.Count);
            if (shopItems.Count == 0) return;

            // Giới hạn là < 14
            int itemCount = Random.Range(1, 14);
            for (int i = 0; i < itemCount; i++)
            {
                int indexItemPick = Random.Range(0, shopItems.Count - 1);
                _itemsNeed.Add(shopItems[indexItemPick]);
            }
            Debug.Log("So item can mua: " + _itemsNeed.Count);
        }

        /// <summary> Giá quá cao thì không đồng ý mua </summary>
        bool IsAgreeItem()
        {
            if (ItemNeedGet()._price < ItemNeedGet()._SO._priceMarketMax) return true;
            else
            {
                ExpressedComplaintsItem();
                _isNotNeedBuy = true;
            }
            return false;
        }

        /// <summary> TODO: expressed complaints because this product is too expensive </summary>
        void ExpressedComplaintsItem()
        {
            Debug.Log("Bán gì mắt vậy cha");
        }

        /// <summary> Tìm slot đợi thanh toán </summary>
        bool GoSlotPayment()
        {
            if(_isConfirmPay) return true; // thanh toán rồi thì không cần đén hàng chờ 

            _mayTinh._waitingLine.RegisterSlot(transform); // Đăng ký slot
            Transform slotWait = _mayTinh._waitingLine.GetCustomerSlot(transform); // tìm vị trí slot
            return MoveToTarget(slotWait);
        }

        /// <summary> Thanh toán tiền, trả true nếu thanh toán thành công </summary>
        bool PayItem()
        {
            if (_playerConfirmPay && !_isConfirmPay)
            {
                _gameManager.AddCoin(TotalCoinPay());
                _isConfirmPay = true;
                return true;
            }
            else if (_isConfirmPay) return true;
            return false;
        }

        /// <summary> Tổng số tiền cần trả </summary>
        float TotalCoinPay()
        {
            float total = 0;
            foreach (var item in _itemSlot._listItem)
            {
                if (item._item != null)
                {
                    total += item._item._price;
                }
            }
            return total;
        }

        /// <summary> chọn tiem cần lấy theo thứ tự </summary>
        Item ItemNeedGet()
        {
            // Tìm item đang không có
            for (int i = 0; i < _itemsNeed.Count; i++)
                if (!_itemSlot.IsContentItem(_itemsNeed[i])) return _itemsNeed[i];
            return null;
        }
    }
}