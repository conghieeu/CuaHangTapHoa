using System.Collections;
using System.Collections.Generic;
using CuaHang;
using UnityEngine;

public class CollDragItem : MonoBehaviour
{
    public Item _item;

    private void Awake()
    {
        _item = GetComponentInParent<Item>();
    } 
}
