using UnityEngine;

namespace CuaHang
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObjects/ItemSO")]
    public class ItemSO : ObjectSO
    {
        public float _priceDefault;
        public float _priceMarketMin, _priceMarketMax;
        public bool _isCanSell; // có thể bán hay không
        public bool _isBlockPrice;
    }
}