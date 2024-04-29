using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolingObjects : MonoBehaviour
{
    [Header("BoolingObjects")]
    [SerializeField] protected List<Transform> _objectsPrefab;

    /// <summary> Xoá object trong hồ </summary>
    public virtual void RemoveObject(Transform objectR)
    {
        objectR.gameObject.SetActive(false);
    }
}
