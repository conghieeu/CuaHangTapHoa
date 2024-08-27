using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class GameSystem : Singleton<GameSystem>
{
    // [Header("GAME SYSTEM")]
    public static bool _IsConnected;
    public static event Action<bool> _OnCheckConnect;

    void Start()
    {
        StartCoroutine(CheckInternetConnection()); 
    }

    IEnumerator CheckInternetConnection()
    { 
        _IsConnected = Application.internetReachability != NetworkReachability.NotReachable;

        if (!_IsConnected)
        {
            Debug.LogWarning("Mất kết nối Internet!"); 
        }
        
        _OnCheckConnect?.Invoke(_IsConnected);

        // Kiểm tra lại sau mỗi 5 giây
        yield return new WaitForSecondsRealtime(5);
        StartCoroutine(CheckInternetConnection());
    }

}

