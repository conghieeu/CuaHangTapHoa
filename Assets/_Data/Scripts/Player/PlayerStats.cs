using Core;
using UnityEngine;

namespace CuaHang
{
    public class PlayerStats : ObjectStats
    {
        [Header("PlayerStats")]
        public PlayerData _playerData;

        protected override void Start()
        {
            base.Start();
        }

        public override void LoadData<T>(T data)
        {
            _playerData = (data as GameData)._playerData;

            // set properties
            transform.position = _playerData._position;
            transform.rotation = _playerData._rotation;
        }

        protected override void SaveData()
        { 
            // save value
            GetGameData()._playerData = new PlayerData(
                _playerData._name, _playerData._money, transform.rotation, transform.position);
        }
    }
}