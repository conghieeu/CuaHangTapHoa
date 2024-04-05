using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPlantSO", menuName = "ScriptableObjects/ObjectPlant")]
public class ObjectPlantSO : ScriptableObject
{
    public string _name;
    public string _nameID;
    public float _price;
    public string _currency;
    public List<ObjectSellSO> _listItem;
}

[CreateAssetMenu(fileName = "ObjectSellSO", menuName = "ScriptableObjects/ObjectSell")]
public class ObjectSellSO : ScriptableObject
{
    public string _name;
    public string _nameID;
    public float _price;
    public string _currency;
    public Transform _itemPrefabs; 
}