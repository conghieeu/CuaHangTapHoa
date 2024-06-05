using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectSO", menuName = "ObjectSO", order = 0)]
public class ObjectSO : ScriptableObject
{
    public string _typeID; // đây như kiểu product type id của đối tượng, không thể thay đổi 
    public string _name; // Tên của đối tượng và có thể thay đổi vì người dùng có thể dặt tên lại
    public string _type; // Kiểu của đối tượng và không thể thay đổi
}