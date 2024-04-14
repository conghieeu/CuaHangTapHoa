using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CuaHang
{
    public class PlayerPlanting : MonoBehaviour
    {
        PlayerCtrl _ctrl;
        // khi dragging object temp thì thằng này sẽ hiện model của object đang drag nớ ra

        private void Awake()
        {
            _ctrl = GetComponent<PlayerCtrl>();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.T))
            {
                SenderItems();
            }
        }

        // kiện hàng trên tay người chơi còn chạm vào kệ trống người chơi có thể truyền item parcel sang kệ đó
        void SenderItems()
        {
            if (_ctrl._boxSensor._hits.Count == 0) return;

            ObjectPlant objPlantHit = null;

            foreach (var hit in _ctrl._boxSensor._hits)
            {
                if (hit.GetComponent<ObjectPlant>())
                    objPlantHit = hit.GetComponent<ObjectPlant>();
            }

            ObjectPlant objPlantHolding = _ctrl._modelTempHolding.GetComponentInChildren<ObjectPlant>();

            // Nếu vật thể đã chạm được tới thực thể cần tới 
            if (objPlantHit && objPlantHolding)
            {
                // thực hiện việc truyền đơn hàng
                Debug.Log("Người chơi truyền Item" + objPlantHolding + " -> " + objPlantHit);

                ObjectPlantSO parcelSO = objPlantHolding._objPlantSO;
                ObjectPlantSO tableSO = objPlantHit._objPlantSO;

                // chuyển item
                for (int i = parcelSO._listItem.Count - 1; i >= 0; i--)
                {
                    if (parcelSO._listItem[i] == null) continue;

                    for (int j = 0; j < tableSO._listItem.Count; j++)
                    {
                        if (tableSO._listItem[j] == null)
                        {
                            tableSO._listItem[j] = parcelSO._listItem[i];
                            parcelSO._listItem[i] = null;
                        }
                    }
                }

                // Load lại các item hiển thị
                objPlantHit.LoadItemsSlot();
                objPlantHolding.GetComponent<ObjectPlant>().LoadItemsSlot();
            }
        }

    }
}
