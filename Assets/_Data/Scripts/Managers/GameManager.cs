using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace CuaHang
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] protected static float _coin;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }

            _coin = 500;
        }

        public static float _Coin
        {
            get => _coin;
            set
            {
                if (value > 0 && value < 999999) _coin = value;
            }
        }
        
        public static void AddCoin(float value) => _Coin += value;
    }

}