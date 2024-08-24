using System.Collections.Generic;
using System.Linq;

namespace CuaHang.Pooler
{
    public class ItemPooler : ObjectPooler
    {
        public TypePool _typePool;
        public static ItemPooler Instance;

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }
        }

        /// <summary> Tìm item có item Slot và còn chỗ trống </summary>
        public Item GetItemEmptySlot(TypeID typeID)
        {
            foreach (var o in _ObjectPools)
            {
                Item item = o.GetComponent<Item>();
                if (item && item._itemSlot && item._typeID == typeID && item._itemSlot.IsHasSlotEmpty()) return item;
            }
            return null;
        }

        /// <summary> Tìm item có itemSlot có chứa itemProduct cần lấy </summary>
        public virtual Item GetItemContentItem(Item item)
        {
            foreach (var objectBool in _ObjectPools)
            {
                Item i = objectBool.GetComponent<Item>();

                if (i && i._itemSlot && i._itemSlot.IsContentItem(item))
                {
                    return i;
                }
            }
            return null;
        }

        /// <summary> Tìm item lần lượt theo mục đang muốn mua </summary>
        public Item ShuffleFindItem(TypeID typeID)
        {
            List<ObjectPool> poolsO = _ObjectPools.ToList();
            poolsO.Shuffle<ObjectPool>();

            foreach (var o in poolsO)
            {
                Item item = o.GetComponent<Item>();

                if (item && item._itemParent && o._typeID == typeID && item._itemParent._type == Type.Shelf && item.gameObject.activeSelf) return item;
            }

            return null;
        }
    }
}