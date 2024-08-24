using HieuDev;
using UnityEngine;

namespace CuaHang
{
    public class PlayerStats : ObjectStats
    {
        [Header("PlayerStats")]
        public PlayerData _playerData;

        public override void LoadData<T>(T data)
        {
            throw new System.NotImplementedException();
        }

        protected override void SaveData()
        {
            throw new System.NotImplementedException();
        }
    }
}
