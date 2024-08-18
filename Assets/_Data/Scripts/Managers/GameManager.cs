using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace CuaHang
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] protected static float _coin;

        protected override void Awake()
        {
            base.Awake();
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