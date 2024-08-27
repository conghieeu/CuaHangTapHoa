using UnityEngine;

namespace CuaHang.UI
{
    public abstract class UIPanel : HieuBehavior
    {
        [Header("UI PANEL")]
        public RectTransform _contentPanel;
        public abstract void Show();
        public abstract void Hide();
    }


}