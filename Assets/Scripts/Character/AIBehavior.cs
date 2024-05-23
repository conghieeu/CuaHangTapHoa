using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CuaHang.Pooler;

namespace CuaHang.AI
{
    public class AIBehavior : HieuBehavior
    {
        [Header("AIBehavior")]
        [SerializeField] private Item _itemTarget;  // Thứ mà cái này nhân viên hướng tới, không phải là thứ đang dữ trong người
        public Item _itemHolding; // objectPlant người chơi đã nhặt đc và đang giữ trong người
        public Transform _itemHoldingPoint; // là vị trí mà nhân viên này đang giữ ObjectPlant trong người

        public ItemPooler _itemPooler;
        public SensorCast _boxSensor;
        public NavMeshAgent _navMeshAgent;

        public Item _ItemTarget
        {
            get => _itemTarget;
            set
            {
                _itemTarget = value;
                if (_ItemTarget)
                    if (value)
                    {
                        _ItemTarget._objFollowedThis = transform;
                    }
                    else
                    {
                        _ItemTarget._objFollowedThis = null;
                    }
            }
        }

        protected virtual void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _itemPooler = ItemPooler.Instance;
        }

        // Nó sẽ đưa cái item từ slot này sang slot kia của cái bàn, false là còn kiện hàng đơn hàng chưa giao hết
        protected virtual void SenderItem(Item sender, Item receiver)
        {
            // Nếu vật thể đã chạm được tới thực thể cần tới 
            if (GetItemHit() && _itemHolding != null)
            {
                // thực hiện việc truyền đơn hàng
                Debug.Log("Thực hiện việc truyền dữ liệu đơn hàng");

                // chuyển item 
                for (int i = sender._itemSlot._listItem.Count - 1; i >= 0; i--)
                {
                    Item item = sender._itemSlot._listItem[i]._item;
                    if (item != null && receiver._itemSlot.IsAnyEmptyItem())
                    {
                        sender._itemSlot.DeleteItem(item);
                        receiver._itemSlot.AddItemWithTypeID(item._typeID);
                    }
                }
            }
        }

        /// <summary> AI biết nó chạm tới tứ nó cần </summary>
        protected virtual Item GetItemHit()
        {
            foreach (var item in _boxSensor._hits)
            {
                if (_ItemTarget)
                    if (item == _ItemTarget.transform)
                    {
                        return item.GetComponent<Item>();
                    }
            }
            return null;
        }

        protected virtual void MoveToTarget()
        {
            if (_ItemTarget != null)
            {
                _navMeshAgent.SetDestination(_ItemTarget.transform.position);
            }
        }

        protected virtual Item FindItemWithTypeID(string typeID) => _itemPooler.FindItemWithTypeID(typeID, true);
    }
}