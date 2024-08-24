using CuaHang.AI;
using UnityEngine;

public class CustomerStats : ObjectStats
{
    [Header("ItemStats")]
    public CustomerData _data;
    [SerializeField] Customer _customer;

    protected void Start()
    { 
        _customer = GetComponent<Customer>();
    }

    public void LoadData(CustomerData data)
    {
        _data = data;
        _customer.SetProperties(data);
    }

    public CustomerData GetData()
    {
        _data = new CustomerData(
            _customer._ID,
            _customer._typeID,
            _customer._name,
            _customer._totalPay,
            _customer._isNotNeedBuy,
            _customer._playerConfirmPay,
            _customer._isPay,
            _customer.transform.position,
            _customer.transform.rotation);

        return _data;
    }

    protected override void SaveData()
    {
        
    }

    public override void LoadData<T>(T data)
    {
        
    }
}
