using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    // Thanh tiến trình (Progress Bar) để hiển thị tiến trình tải, nếu cần
    public static Slider progressBar;

    // Hàm để tải cảnh mới theo tên một cách không đồng bộ
    public static async void LoadSceneByNameAsync(string sceneName)
    {
        await LoadSceneAsyncInternal(sceneName);
    }

    // Hàm để tải cảnh mới theo index một cách không đồng bộ
    public static async void LoadSceneByIndexAsync(int sceneIndex)
    {
        await LoadSceneAsyncInternal(sceneIndex);
    }

    // Phương thức nội bộ sử dụng cho cả tên cảnh và index
    private static async Task LoadSceneAsyncInternal(object sceneIdentifier)
    {
        // Kiểm tra nếu Progress Bar tồn tại và đặt giá trị ban đầu
        if (progressBar != null)
        {
            progressBar.value = 0;
            progressBar.gameObject.SetActive(true); // Hiển thị thanh tiến trình
        }

        // Tạo tác vụ tải cảnh không đồng bộ
        AsyncOperation asyncLoad;
        if (sceneIdentifier is string sceneName)
            asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        else if (sceneIdentifier is int sceneIndex)
            asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        else
            throw new System.ArgumentException("sceneIdentifier phải là tên cảnh hoặc index cảnh.");

        asyncLoad.allowSceneActivation = false; // Tắt tự động chuyển cảnh

        // Vòng lặp cập nhật tiến trình tải
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Lấy tiến trình tải (0.9 là tối đa)
            if (progressBar != null)
                progressBar.value = progress; // Cập nhật thanh tiến trình

            // Kiểm tra nếu tải xong (gần 90%) thì có thể cho phép chuyển cảnh
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true; // Cho phép chuyển cảnh
            }

            await Task.Yield(); // Đợi một frame trước khi tiếp tục
        }

        // Ẩn Progress Bar khi hoàn tất
        if (progressBar != null)
            progressBar.gameObject.SetActive(false);
    }
}
