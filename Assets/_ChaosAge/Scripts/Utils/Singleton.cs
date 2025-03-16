namespace DatSystem.utils
{
    using UnityEngine;

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                // Nếu chưa có instance, tìm trong scene
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    // Nếu không tìm thấy, tạo mới một GameObject với component này
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        _instance = singletonObject.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        // Hàm này được gọi khi object được tạo
        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }

        // Optional: Đảm bảo instance bị xóa khi ứng dụng thoát
        protected virtual void OnApplicationQuit()
        {
            _instance = null;
        }
    }
}