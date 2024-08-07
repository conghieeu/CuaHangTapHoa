using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CuaHang.UI
{
    public class UpdateValues : MonoBehaviour
    {
        [SerializeField] protected GameManager _gameManager;
        [SerializeField] protected Text _textCoin;

        private void Start()
        {
            _gameManager = GameManager.Instance;
        }

        private void FixedUpdate()
        {
            _textCoin.text = _gameManager._Coin.ToString();
        }
    }

}