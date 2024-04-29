using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolingCustomers : BoolingObjects
{
    public static BoolingCustomers Instance;

    private void Awake()
    {
        if (Instance) Destroy(this); else { Instance = this; }
    }


}
