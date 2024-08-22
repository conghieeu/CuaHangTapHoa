using UnityEngine;

namespace CuaHang.AI
{
    public class StaffStats : ObjectStats
    {
        [Header("ItemStats")]
        [SerializeField] Staff _staff;
        [SerializeField] StaffData _data;

        private void Awake()
        {

            _staff = GetComponent<Staff>();
        }

        protected override void Start()
        {
            base.Start();
        }

        public void LoadData(StaffData staffData)
        {
            _data = staffData;
            _staff.SetProperties(staffData);
        }

        public virtual StaffData GetData()
        {
            ItemData itemHolder = null;

            if (_staff._parcelHold)
            {
                itemHolder = _staff._parcelHold._itemStats._data;
            }

            _data = new StaffData(
                _staff._ID,
                _staff._typeID,
                _staff._name,
                itemHolder,
                _staff.transform.position);

            return _data;
        }

        protected override void SaveData() { }
    }

}