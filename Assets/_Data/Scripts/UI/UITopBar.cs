using UnityEngine;
using UnityEngine.UI;

namespace CuaHang.UI
{
    public class UITopBar : MonoBehaviour
    {
        [SerializeField] protected Text _textCoin;
    
        private void FixedUpdate()
        {
            _textCoin.text = $"Tổng tiền: {GameManager._Coin.ToString("F1")}";
        }
    }

}