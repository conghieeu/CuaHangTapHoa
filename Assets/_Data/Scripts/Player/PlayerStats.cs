using HieuDev;
using UnityEngine;

namespace CuaHang
{
    public class PlayerStats : ObjectStats
    {
        [Header("PlayerStats")]
        public PlayerData _playerData;

        protected override void LoadData(GameData gameData)
        {
            base.LoadData(gameData);

            _playerData = gameData._playerData;

            if (_playerData != null)
            {
                transform.position = _playerData._position;
            }
        }

        protected override void SaveData()
        { 
            GetGameData()._playerData = new PlayerData(_playerData._name, transform.position, _playerData._money);
        }
    }
}
