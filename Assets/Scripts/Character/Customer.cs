using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CuaHang.Pooler;
using Unity.VisualScripting;
using UnityEditorInternal.VersionControl;
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
        }

        private void FixedUpdate()
        {
            // Nhân vật phải có í định mua 1 cái gì dấy
            if (_itemsNeed.Count == 0) SetItemNeed();

            Behavior();
        }


        /// <summary> Player xác nhận thanh toán với khách hàng này </summary>
        public void SetPlayerConfirmPay()
        {
            _isNotNeedBuy = true;
            _playerConfirmPay = true;
        }

        void Behavior()
        {
            if (!_itemSlot.ItemsSequenceEqual(_itemsNeed)) // Kiểm tra item cần mua
            {
                if (GoToItemNeed())
                {
                    if (IsAgreeItem())
                    {
                        Log("1.1: Lấy danh sách items");
                        GetItem();
                    }
                    else if (!IsAgreeItem() && _itemSlot.IsAnyItem()) // mua được vài thứ nên đi về 
                    {
                        Log("1.2: Thanh toan vi hang dom, thanh toan nhung mon do dang co");
                        if (GoPayItem()) GoOutShop();
                    }
                }
                else if (_isNotNeedBuy && _itemSlot.IsAnyItem() == false) // không mua được gì nên đi về
                {
                    Log("1.3: Đồ quá mắt không mua được gì đi về");
                    GoOutShop();
                }
            }
            else
            {
                Log("2: Thanh toan hang mua");
                if (GoPayItem()) GoOutShop();
            }
        }

        /// <summary> Chọn ngẫu nhiên item mà khách hàng này muốn lấy </summary>
        void SetItemNeed()
        {
            // Lấy danh sách item mà cửa hàng đang có
            List<Item> allItems = _itemPooler.GetAllItemsCanSell();
            if (allItems.Count == 0) return;

            List<Item> itemGets = new List<Item>();

            // Giới hạn là < 10
            int itemCount = Random.Range(1, 10);
            for (int i = 0; i < itemCount; i++)
            {
                int r = Random.Range(0, allItems.Count - 1); // random trong allItems
                if (!itemGets.Contains(allItems[r]))
                    itemGets.Add(allItems[r]);
            }

            Log("Số lượng khác hàng " + this.name + " muốn lấy là: " + itemGets.Count);

            for (int i = 0; i < itemGets.Count; i++) // 
            {
                // Thêm item đầu tiên vào trước
                if (!_itemsNeed.Contains(itemGets[i]) && _itemsNeed.Count == 0)
                {
                    _itemsNeed.Add(itemGets[i]);
                    i = 0;
                    continue;
                }
                // if2: Từ item đã thêm ở vị trí cuối , lấy các item khác cùng liên quan 
                if (!_itemsNeed.Contains(itemGets[i]) && _itemPooler.IsSameShelf(_itemsNeed[_itemsNeed.Count - 1], itemGets[i]))
                {
                    _itemsNeed.Add(itemGets[i]);
                    i = 0;
                    continue;
                }
                // Thêm item mới vào để thực hiện lại if2
                if (!_itemsNeed.Contains(itemGets[i]))
                {
                    _itemsNeed.Add(itemGets[i]);
                    i = 0;
                    continue;
                }
            }

        }

        /// <summary> Lấy item từ Shelf đưa vào this._itemSlot </summary> 
        void GetItem()
        {
            if (ItemNeedGet() == null) return;

            Item itemNeedGet = ItemNeedGet();
            Item shelf = _itemPooler.FindShelfContentItem(ItemNeedGet());

            for (int i = 0; i < shelf._itemSlot._itemsSlots.Count; i++)
            {
                Item shelfItem = shelf._itemSlot._itemsSlots[i]._item;
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
            if (GoWaitingSlots()) if (PayItem()) return true;
            return false;
        }

        /// <summary> Chạy tới vị trí item cần lấy </summary>
        bool GoToItemNeed()
        {
            // Lay target
            Item itemGet = ItemNeedGet(); // lấy quả táo trong thế giới
            if (itemGet == null || _isNotNeedBuy) return false;

            Item shelf = _itemPooler.FindShelfContentItem(itemGet); // lấy cái bàn chứa quả táo

            // Huỷ target nếu target có chứa parent
            if (shelf) if (shelf._ThisParent) shelf = null;
            // Bỏ item này vì item đã có chủ
            if (!shelf) _itemsNeed.Remove(itemGet);

            // Kiem tra sensor có chạm vào itemTarget đang hướng tới không
            if (IsHitItemTarget())
            {
                _itemTarget = null; // Dừng di chuyển
                return true;
            }
            else
            {
                _itemTarget = shelf; // di chuyển đến target
                MoveToTarget();
            }
            return false;
        }

        /// <summary> Ra về khách tìm điểm đến là ngoài ở shop </summary>
        void GoOutShop()
        {
            MoveToTarget(_outShopPoint);
        }

        /// <summary> Tìm slot đợi thanh toán </summary>
        bool GoWaitingSlots()
        {
            if (_isConfirmPay) return true; // thanh toán rồi thì không cần đén hàng chờ 

            _mayTinh._waitingLine.RegisterSlot(this); // Đăng ký slot 
            if (_slotWaiting == null) return false;

            return MoveToTarget(_slotWaiting);
        }

        /// <summary> Giá quá cao thì không đồng ý mua </summary>
        bool IsAgreeItem()
        {
            if (ItemNeedGet() == null) return false;

            if (ItemNeedGet()._price < ItemNeedGet()._SO._priceMarketMax) return true;
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
            if (_playerConfirmPay && !_isConfirmPay)
            {
                _isConfirmPay = true;
                _gameManager.AddCoin(TotalCoinPay());
                _mayTinh._waitingLine.CancelRegisterSlot(this);
                return true;
            }
            else if (_isConfirmPay) return true;
            return false;
        }

        /// <summary> Tổng số tiền cần trả </summary>
        float TotalCoinPay()
        {
            float total = 0;
            foreach (var item in _itemSlot._itemsSlots)
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