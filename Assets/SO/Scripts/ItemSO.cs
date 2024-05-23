using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObjects/ItemSO")]
    public class ItemSO : ObjectSO
    {
        public float _price;
        public float _priceMarketMin, _priceMarketMax;
        public string _currency;
        public Transform _thisPrefabs;
        public List<ItemSO> _items; /// <summary> Items sẽ được nạp vào objectPlant theo trình tự nên không được để rỗng </summary>
    }
}