using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace CuaHang
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] protected float _coin;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance) Destroy(this); else { Instance = this; }
        }

        public float _Coin
        {
            get => _coin;
            set
            {
                if (value > 0 && value < 9999999) _coin = value;
            }
        }
        
        public void AddCoin(float value) => _Coin += value;
    }

}