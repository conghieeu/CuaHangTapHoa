using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CuaHang
{
    public class MayTinh : MonoBehaviour
    {
        [Header("MayTinh")]
        public ObjectPlantSO _objectPlantSO;
        public Transform _objectPlantPrefabs;
        public Transform _objectPlantHolder;
        public Transform _spawnTrans;
        public List<Transform> _slotsQueue;
        public List<Transform> _slotsCustomer;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player đã chạm máy tính: Tạo 1 vật phẩm");
                CreateObjectPlant();
            }
        }

        /// <summary> đăng ký slot hàng đợi </summary>
        public void RegisterSlot(Transform customer)
        {
            _slotsCustomer.Add(customer.transform);
        }

        /// <summary> Rời slot hàng đợi </summary>
        public void CancelRegisterSlot(Transform customer)
        {
            _slotsCustomer.Remove(customer);
        }

        /// <summary> Lấy hàng đợi </summary>
        public Transform GetCustomerSlot(Transform customer)
        {
            for (int i = 0; i < _slotsCustomer.Count; i++)
            {
                if (customer == _slotsCustomer[i])
                {
                    return _slotsQueue[i].transform;
                }
            }
            return null;
        }

        // tạo vật thể với SO mới trùng vs SO mẫu nào đó
        [ContextMenu("CreateObjectPlant")]
        void CreateObjectPlant()
        {
            Transform objectPlant = Instantiate(_objectPlantPrefabs, _objectPlantHolder);
            objectPlant.position = _spawnTrans.position;
            objectPlant.rotation = _spawnTrans.rotation;
            objectPlant.GetComponent<ObjectPlant>()._SO = _objectPlantSO;
        }

        // // Lưu lại những thay đổi của vật thể đó
        // public void SaveSO()
        // {
        //     // Tạo AssetBundle
        //     var assetBundle = BuildPipeline.BuildAssetBundle(
        //         ScriptableObject.CreateInstance<ObjectPlantSO>(),
        //         "MyAssetBundle",
        //         BuildAssetBundleOptions.None,
        //         BuildTarget.StandaloneWindows64
        //     );

        //     // Thiết lập vị trí lưu trữ
        //     var outputPath = Path.Combine(Application.dataPath, "AssetBundles");
        //     assetBundle.SaveToFile(outputPath);

        //     // Giải phóng AssetBundle
        //     assetBundle.Unload(true);
        // }

        // // Load lại thông tin của object plant với SO đó
        // public void LoadSO()
        // {
        //     // Tải AssetBundle từ vị trí lưu trữ
        //     var assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "AssetBundles", "MyAssetBundle"));

        //     // Truy cập ScriptableObject từ AssetBundle
        //     var myScriptableObject = assetBundle.LoadAsset<MyScriptableObject>();

        //     // Sử dụng ScriptableObject
        //     Debug.Log(myScriptableObject.myVariable);

        //     // Giải phóng AssetBundle
        //     assetBundle.Unload(true);
        // }

    }
}