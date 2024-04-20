using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSellSO", menuName = "ScriptableObjects/ObjectSell")]
public class ObjectSellSO : ScriptableObject
{
    public string _name;
    public string _type;
    public float _price;
    public float _priceMarketMin, _priceMarketMax;
    public string _currency;
    public Transform _itemPrefabs;
}