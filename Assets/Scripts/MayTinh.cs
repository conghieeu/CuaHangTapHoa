using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CuaHang
{
    public class MayTinh : MonoBehaviour
    {
        public ObjectPlantSO _objectPlantSO;
        public Transform _objectPlantPrefabs;
        public Transform _objectPlantHolder;
        public Transform _spawnTrans;
        string _pathSaveSO = Path.Combine(Application.dataPath, "SO");

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player đã chạm máy tính: Tạo 1 vật phẩm");
                CreateObjectPlant();
            }
        }

        // tạo vật thể với SO mới trùng vs SO mẫu nào đó
        [ContextMenu("CreateObjectPlant")]
        public void CreateObjectPlant()
        {
            Transform objectPlant = Instantiate(_objectPlantPrefabs, _objectPlantHolder);
            objectPlant.position = _spawnTrans.position;
            objectPlant.rotation = _spawnTrans.rotation;
            objectPlant.GetComponent<ObjectPlant>()._objPlantSO = _objectPlantSO;
            // Tạo So mới ở vị trí file cần lưu
            Debug.Log(Application.dataPath + "/../AssetBundles");
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