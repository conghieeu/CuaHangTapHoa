using System.Collections;
using UnityEngine;

public class GameSystem : Singleton<GameSystem>
{
    // [Header("GAME SYSTEM")]
    public static bool _IsConnected { get; private set; }

    void Start()
    {
        StartCoroutine(CheckInternetConnection());
    }

    IEnumerator CheckInternetConnection()
    {
        while (true)
        {
            _IsConnected = Application.internetReachability != NetworkReachability.NotReachable;

            if (!_IsConnected)
            {
                Debug.LogWarning("Mất kết nối Internet!");
                // Thực hiện các hành động cần thiết khi mất kết nối
            }

            // Kiểm tra lại sau mỗi 5 giây
            yield return new WaitForSeconds(5);
        }
    }

}

