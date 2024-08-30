using UnityEngine;
using UnityEngine.UI;

namespace CuaHang.UI
{
    public class BtnSwitch : MonoBehaviour
    {
        [Header("Open new panels")]
        public UIPanel _panelOpen;
        public UIPanel _panelClose;

        Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(_OpenNewPanel);
        }

        public void _OpenNewPanel()
        {
            if (_panelClose)
            {
                _panelClose.ShowContents(false);
            }

            if (_panelOpen)
            {
                _panelOpen.ShowContents(true);
            }

        }
    }

}
