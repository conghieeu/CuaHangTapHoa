using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPlantSO", menuName = "ScriptableObjects/ObjectPlant")]
public class ObjectPlantSO : ScriptableObject
{
    public string _name;
    public float _price;
    public string _currency;
    public List<ObjectSellSO> _listItem;
}

