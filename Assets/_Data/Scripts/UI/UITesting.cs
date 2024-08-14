using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CuaHang
{
    public class UITesting : MonoBehaviour
    {
        public Text _txtSnap;
        private void FixedUpdate() {

            _txtSnap.text = $"Snapping: {SingleModuleManager.Instance._raycastCursor._enableSnapping}";
        }
    }
}
