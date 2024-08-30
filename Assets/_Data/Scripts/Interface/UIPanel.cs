using UnityEngine;

namespace CuaHang.UI
{
    public class UIPanel : HieuBehavior
    {
        [Header("UI PANEL")]
        public RectTransform _panelContent;
        public virtual void ShowContents(bool value)
        {
            if (_panelContent)
            {
                _panelContent.gameObject.SetActive(value);
            }
        }
    }
}