using System.Collections;
using System.Collections.Generic;
using CuaHang.StaffAI;
using UnityEngine;

public class BoolingStaffs : BoolingObjects
{
    public static BoolingStaffs Instance;

    private void Awake()
    {
        if (Instance) Destroy(this); else { Instance = this; }
    }

}
