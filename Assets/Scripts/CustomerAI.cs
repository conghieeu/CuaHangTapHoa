using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CuaHang
{
    public class CustomerAI : AIBehavior
    {

        [Header("CustomerAI")]
        public Transform _slotWaiting;
        public MayTinh _mayTinh;
        [Space]
        public bool _isGetItem;
        public bool _isPayItem;

        public List<ObjectSellSO> _listItem;

        /// <summary> 
        /// 0: Idle, 1: Find Item, 2: Payment, 3: go out market 
        /// </summary>
        public int _state = 0;

        private void Awake()
        {
            _movement = GetComponent<AIMovement>();
        }

        private void FixedUpdate()
        {
            SetState();

            // Run State
            switch (_state)
            {
                case 0:
                    StateIdle();
                    break;
                case 1:
                    StateFindItem();
                    break;
                case 2:
                    StatePayment();
                    break;
                case 3:
                    StateGoOutMarket();
                    break;
            }
        }

        void SetState()
        {
            // Tìm hàng
            if (!_isGetItem)
            {
                _state = 1;
            }
            // khi nhặt được hàng cần tìm Tới quầy đợi thanh toán,  
            else if (!_isPayItem && _isGetItem)
            {
                _state = 2;
            }
            // Điều kiện để khách hàng rời cửa hàng (Đã trả xong tiền, đã có vật phẩm cần mua)
            else if (_isPayItem && _isPayItem)
            {
                _state = 3;
            }
            else
            {
                _state = 0;
            }
        }

        // khách hàng xuất hiện như nào ? Cần xử lý sự xuất hiện khách hàng
        void StateIdle()
        {

        }

        void StateFindItem()
        {
            // mức độ hài lòng của khách hàng, khách hàng chê sản phẩm đắt rẻ không ?
            if (TryComplainItem()) return;

            // Nếu hài lòng về sản phẩm thì nhặt item đó lênh
            PickUpItemInShelves();
        }

        /// <summary> Nhân vật bình luận về vật phẩm </summary>
        /// <returns> True: nhân vật có phàn nàn về sản phẩmf </returns>
        bool TryComplainItem()
        {
            // TODO: khách hàng chọn nhiều loại mặt hàng ngẫu nhiên nhưng tuỳ theo giá cả
            // item lởm hoặc giá quá cao thì phàn nàng và bảo về không thì thôi
            // Vậy là phải có thang đo quy chuẩn về giá cả, phải có giá thị thường

            return true;
        }
        // khách hàng chạy tới mua hàng thì truyền item từ kệ hàng sang khách hàng như nào ?
        void PickUpItemInShelves()
        {
            // Lay target
            Transform target = FindObjectPlantWithType("KeHang");

            if (target == null) return;

            // di chuyen den target
            _targetTransform = target;
            Movement();

            // Kiem tra cham vao target
            if (GetObjPlantHit() == target && _listItem.Count == 0)
            {
                // dua item vao _listItem  
                ObjectPlant table = target.GetComponent<ObjectPlant>();
                GetItems(table, this);
            }
        }
        /// <summary>  </summary>
        void GetItems(ObjectPlant sender, CustomerAI receiver)
        {
            // van de la cai nay no lay het ma khach hang thi no lay thu can lay thoi

            // thực hiện việc truyền đơn hàng

            // chuyển item
            for (int i = sender._listItem.Count - 1; i >= 0; i--)
            {
                if (sender._listItem[i] == null) continue;

                for (int j = 0; j < receiver._listItem.Count; j++)
                {
                    if (receiver._listItem[j] == null)
                    {
                        receiver._listItem[j] = sender._listItem[i];
                        sender._listItem[i] = null;
                    }
                }

                // Load lại các item hiển thị
                sender.LoadItemsSlot();
            }
        }


        // khách hàng thanh toán như nào
        void StatePayment()
        {
            // khách hàng chạy tới quầy thanh toán để thanh toán, thanh toán thì cần người thanh toán
            // Đứng vào khu vực thanh toán là tự động thanh toán
            // Khi đặt cầm được vật phẩm trong tay thì khách hàng này tới quầy thanh toán
            FindSlotPayment();

            // Đưa tiền cho PlayerProperty
            ShellOutItem();

            // Khi đi thì khách hàng sẽ đánh giá về sản phẩm này
            StoreReviews();
        }
        // Khách hàng phải biết xếp hàng 
        // + Có các điểm set sẵn khi khách hàng thanh toán thì đứng vào các điểm slot đó
        // + TODO: phải code thêm nâng cấp địa điểm máy tính thanh toán
        void FindSlotPayment()
        {

        }
        // Khách hàng tiến hành thanh toán
        void ShellOutItem()
        {
            // đưa tiền cho PlayerProperty
            // khi khách hàng hoàn thành thanh toán xong thì cộng thêm tiền với các item mà khách hàng mua
        }
        void StoreReviews()
        {

        }

        // Khách hàng ra về
        void StateGoOutMarket()
        {
            // sẽ có các điểm ở ngoài cửa hàng và set điểm đó ở AI để AI rời khỏi cửa hàng
        }

    }
}