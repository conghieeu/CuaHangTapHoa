using CuaHang.AI;
using UnityEngine;

public class CustomerStats : ObjectStats
{
    [Header("ItemStats")]
    [SerializeField] CustomerData _customerData;
    [SerializeField] Customer _customer;

    protected override void Start()
    {
        base.Start();
        _customer = GetComponent<Customer>();
    }

    public void LoadData(CustomerData customerData)
    {
        _customerData = customerData;
    }

    public CustomerData GetCustomerData()
    {
        _customerData = new CustomerData(
            _customer._ID, 
            _customer._name, 
            _customer._totalPay, 
            _customer._isNotNeedBuy, 
            _customer._playerConfirmPay, 
            _customer._isPay, 
            _customer.transform.position);

        return _customerData;
    }

    protected override void SaveData() { }
}
