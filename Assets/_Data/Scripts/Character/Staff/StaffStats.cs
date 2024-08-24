using UnityEngine;

namespace CuaHang.AI
{
    public class StaffStats : ObjectStats
    {
        [Header("ItemStats")]
        public StaffData _data;
        [SerializeField] Staff _staff;

        private void Awake()
        {

            _staff = GetComponent<Staff>();
        }
 

        /// <summary> Được gọi từ cha </summary>
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
                // itemHolder = _staff._parcelHold._itemStats._data;
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

        public override void LoadData<T>(T data)
        {
            throw new System.NotImplementedException();
        }
    }

}