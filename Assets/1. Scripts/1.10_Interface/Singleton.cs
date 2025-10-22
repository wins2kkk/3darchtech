using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    // Để khóa đồng bộ trong các trường hợp đa luồng (nếu cần)
    private static readonly object _lock = new object();

    public static T Instance
    {
        get
        {
            // Nếu đã có instance, trả về luôn
            if (_instance != null) return _instance;

            // Kiểm tra và đảm bảo không có xung đột
            lock (_lock)
            {
                // Tìm đối tượng đã tồn tại
                _instance = FindObjectOfType<T>();
                if (_instance != null) return _instance;

                // Nếu chưa có, tạo mới
                var singletonObject = new GameObject(typeof(T).Name);
                _instance = singletonObject.AddComponent<T>();

                // Đảm bảo đối tượng không bị phá hủy khi chuyển scene
                DontDestroyOnLoad(singletonObject);
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
