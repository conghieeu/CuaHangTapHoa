using UnityEngine;

namespace CuaHang.AI
{
    public class StaffStats : ObjectStats
    {

        [Header("ItemStats")]
        [SerializeField] Staff _staff;
        [SerializeField] StaffData _staffData;

        protected override void Start()
        {
            base.Start();
            _staff = GetComponent<Staff>();
        }

        public void LoadData(StaffData staffData)
        {
            _staffData = staffData;
        }

        public StaffData GetStaffData()
        {
            ItemData itemHolder = null;

            if (_staff._parcelHold)
            {
                itemHolder = _staff._parcelHold._itemStats._itemData;
            }

            _staffData = new StaffData(
                _staff._ID,
                _staff._name,
                itemHolder,
                _staff.transform.position);

            return _staffData;
        }



        protected override void SaveData() { }
    }

}