using UnityEngine;

public enum Type
{
    Shelf = 0,
    Parcel = 1,
    Computer = 2,
    Storage = 3,
    Trash = 4,
    ItemSell = 5,
}

public enum TypeID
{
    computer_1,
    customer_1,
    customer_2,
    shelf_1,
    table_1,
    parcel_1,
    trash_1,
    storage_1,
    apple_1,
    milk_1,
    banana_1,

}

[CreateAssetMenu(fileName = "ObjectSO", menuName = "ObjectSO", order = 0)]
public class ObjectSO : ScriptableObject
{
    public string _name; // Tên của đối tượng và có thể thay đổi vì người dùng có thể dặt tên lại
    public TypeID _typeID; // đây như kiểu product type id của đối tượng, không thể thay đổi 
    public Type _type; // Kiểu của đối tượng và không thể thay đổi
}