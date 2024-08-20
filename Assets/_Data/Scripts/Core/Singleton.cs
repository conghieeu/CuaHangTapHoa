using CuaHang;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public abstract class Singleton<T> : HieuBehavior where T : MonoBehaviour
{
    private static T instance;

    [SerializeField] private bool _dontDestroyOnLoad;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    GameObject gameObject = new GameObject("Controller");
                    instance = gameObject.AddComponent<T>();
                    Debug.LogWarning("Lỗi singleton: intance không tồn tại, controller được khởi tạo", gameObject);
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDontDestroyOnLoad(bool value)
    {
        _dontDestroyOnLoad = value;
        if (_dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
