using System;
using System.Collections;
using HieuDev;
using UnityEngine;

namespace CuaHang
{
    public class PlayerStats : HieuBehavior
    {
        public static event Action _OnDataChange;

        [Header("PlayerStats")]
        public PlayerData _playerData;

        private void Start()
        {
            StartCoroutine(UpdatePlayer());
        }

        private void OnEnable()
        {
            SerializationAndEncryption._OnDataLoaded += LoadPlayerData;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SavePlayerData();
            }
        }

        private void LoadPlayerData(GameData gameData)
        {
            _playerData = gameData._playerData;
            _OnDataChange?.Invoke();
        }

        IEnumerator UpdatePlayer()
        {
            yield return new WaitForSeconds(0.1f);
            transform.position = _playerData._position;
        }

        private void SavePlayerData()
        {
            SerializationAndEncryption.Instance.GameData._playerData = new PlayerData(
                _playerData._name, transform.position, _playerData._money);

            SerializationAndEncryption.Instance.SaveGameData();
        }
    }
}
