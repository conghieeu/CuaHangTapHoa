using UnityEngine;

namespace CuaHang.UI
{
    public abstract class UIPanel : HieuBehavior
    {
        [Header("UI PANEL")]
        public RectTransform _contentPanel;
        public virtual void ShowContents(bool value)
        {
            _contentPanel.gameObject.SetActive(value);
        }
    }


}