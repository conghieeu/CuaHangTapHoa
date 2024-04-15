using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang
{
    [Serializable]
    public class ParcelTrash
    {
        public float _time = 0;
        public Transform _parcel;
    }

    public class Trash : StorageRoom
    {
        [Header("           Trash")]
        public float _timeDelete; // thời gian để xoá đi đối tượng bênh trong kho
        public List<ParcelTrash> _listTrash; // thời gian để xoá đi đối tượng bênh trong kho

        private void FixedUpdate()
        {
            CountDownRemove();
        }

        private void CountDownRemove()
        {
            for (int i = 0; i < _listTrash.Count; i++)
            {
                if (_listTrash[i]._time > 0f)
                {
                    _listTrash[i]._time -= Time.fixedDeltaTime;
                }

                if (_listTrash[i]._time <= 0f && _listTrash[i]._parcel)
                {
                    Destroy(_listTrash[i]._parcel.gameObject);
                    ListStaff.Instance.CallListStaffAIUpdateArrivesTarget();
                }
            }
        }

        public void AddDeleteItem(Transform objectPlant)
        {
            foreach (var trash in _listTrash)
            {
                if (trash._parcel == null)
                {
                    trash._time = _timeDelete;
                    trash._parcel = objectPlant;
                    break;
                }
            }
        }

    }

}