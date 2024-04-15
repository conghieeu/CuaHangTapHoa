using System.Collections;
using System.Collections.Generic;
using CuaHang.StaffAI;
using UnityEngine;

public class ListStaff : MonoBehaviour
{
    public List<StaffAI> _listStaffAI;

    public static ListStaff Instance;

    private void Awake() {
        if (Instance) Destroy(this); else { Instance = this; }
    }

    public void CallListStaffAIUpdateArrivesTarget()
    {
        foreach (var staff in _listStaffAI)
        {
            staff.OnUpdateArrivesTarget();
        }
    }
}
