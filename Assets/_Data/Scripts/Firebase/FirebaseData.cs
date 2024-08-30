using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;

public class FirebaseData : MonoBehaviour
{
    private DatabaseReference connectedRef;
    public bool isConnectedToFirebase;

    void Start()
    {
        // Trỏ đến nút đặc biệt `.info/connected`
        //  connectedRef = FirebaseDatabase.DefaultInstance.GetReference(".info/connected");

        // Lắng nghe thay đổi trạng thái kết nối với Firebase
        // connectedRef.ValueChanged += HandleFirebaseConnectionChanged;
    }

    void Update()
    {
        // // Bạn có thể dùng isConnectedToInternet và isConnectedToFirebase để kiểm tra cả hai trạng thái
        // if (GameSystem._IsConnected && isConnectedToFirebase)
        // {
        //     Debug.Log("Có kết nối Internet và đã kết nối đến Firebase.");
        // }
        // else if (!GameSystem._IsConnected)
        // {
        //     Debug.LogWarning("Không có kết nối Internet.");
        // }
        // else if (!isConnectedToFirebase)
        // {
        //     Debug.LogWarning("Không kết nối được đến Firebase.");
        // }
    }

    private void HandleFirebaseConnectionChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError("Lỗi khi kiểm tra kết nối Firebase: " + args.DatabaseError.Message);
            return;
        }

        isConnectedToFirebase = (bool)args.Snapshot.Value;

        if (isConnectedToFirebase)
        {
            Debug.Log("Đã kết nối đến Firebase.");
        }
        else
        {
            Debug.LogWarning("Mất kết nối đến Firebase!");
        }
    }

    
}

