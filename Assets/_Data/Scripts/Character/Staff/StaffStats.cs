using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang.AI
{
    public class StaffStats : ObjectStats
    {
        [Header("ItemStats")]
        [SerializeField] StaffData _staffData;
        [SerializeField] Staff _staff;

        protected override void Start()
        {
            _staff = GetComponent<Staff>();
        }

        public virtual StaffData GetData()
        {
            SaveData();
            return _staffData;
        }

        protected override void SaveData()
        {
            ItemData itemHolder = null;

            _staffData = new StaffData(
                _staff._ID,
                _staff._typeID,
                _staff._name,
                itemHolder,
                _staff.transform.position);
        }

        public override void LoadData<T>(T data)
        {
            _staffData = data as StaffData;
            if (StaffPooler.Instance.IsContentID(_staffData._id)) return;

            _staff = GetComponent<Staff>();
            _staff.SetProperties(_staffData);
        }
    }

}