using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    [CreateAssetMenu(fileName = "ObjectPlantSO", menuName = "ScriptableObjects/ObjectPlant")]
    public class ObjectPlantSO : ObjectSO
    {
        public string _name;
        public string _type;
        public float _price;
        public string _currency;
        public List<ObjectSellSO> _items; /// <summary> Items sẽ được nạp vào objectPlant theo trình tự nên không được để rỗng </summary>
    }
}