using System;
using System.Collections;
using HieuDev;
using UnityEditorInternal;
using UnityEngine;

namespace CuaHang
{
    public class PlayerStats : ObjectSave
    {
        [Header("PlayerStats")]
        public PlayerData _playerData;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SavePlayerData();
            }
        }

        protected override void LoadPlayerData(GameData gameData)
        {
            base.LoadPlayerData(gameData);

            _playerData = gameData._playerData;

            if (_playerData != null)
            {
                transform.position = _playerData._position;
            }
        }

        protected override void SavePlayerData()
        {
            _serializationAndEncryption.GameData._playerData = new PlayerData(
                _playerData._name,
                transform.position,
                _playerData._money);

            base.SavePlayerData();
        }
    }
}
