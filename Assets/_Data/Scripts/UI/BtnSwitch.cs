using UnityEngine;

namespace CuaHang.UI
{
    public class BtnSwitch : MonoBehaviour
    {
        public RectTransform _panelObsever;

        public void OnClickBtnSwitch(bool trigger)
        {
            _panelObsever.gameObject.SetActive(trigger);
        }
    }

}
