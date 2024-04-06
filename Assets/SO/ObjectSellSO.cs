using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSellSO", menuName = "ScriptableObjects/ObjectSell")]
public class ObjectSellSO : ScriptableObject
{
    public string _name;
    public string _nameID;
    public float _price;
    public string _currency;
    public Transform _itemPrefabs; 
}