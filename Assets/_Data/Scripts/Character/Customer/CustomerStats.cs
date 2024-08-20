using UnityEngine;

public class CustomerStats : ObjectStats
{
    [Header("ItemStats")]
    [SerializeField] CustomerData _customerData;

    public void LoadData(CustomerData customerData)
    {
        _customerData = customerData;

    }

    protected override void SaveData()
    {
        // set up save file

    }
}
