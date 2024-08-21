using System.Collections.Generic;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang
{
    public class StaffPoolerStats : ObjectStats
    {
        [Header("CUSTOMER POOLER STATS")]
        [SerializeField] StaffPooler _staffPooler;
        public List<StaffData> _staffs;

        protected override void Start()
        {
            base.Start();
            _staffPooler = GetComponent<StaffPooler>();
        }

        /// <summary> Tái sử dụng hoặc tạo ra item mới từ item có sẵn </summary>
        protected override void LoadData(GameData gameData)
        {
            base.LoadData(gameData);

            _staffs = gameData._staffs;

            if (_staffs.Count > 0)
            {

            }
        }

        protected override void SaveData()
        {
            _staffs.Clear();

            foreach (var staff in _staffPooler.GetPoolItem)
            { 
                StaffData staffData = staff._staffStats.GetStaffData();
                _staffs.Add(staffData);
            }

            GetGameData()._staffs = _staffs;
        }
    }
}