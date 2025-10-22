using System;
using System.IO;
using UnityEngine;

public class ARCameraCapture : Singleton<ARCameraCapture>
{
    public static Action<Texture2D> OnCaptureScreen;

    public Camera arCamera;

    private Texture2D screenShotJustTaken;

    public void CaptureCamera()
    {
        // Tạo một RenderTexture để lưu ảnh
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        arCamera.targetTexture = renderTexture;

        // Destroy(screenShotJustTaken);

        // Render camera vào RenderTexture
        screenShotJustTaken = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        arCamera.Render();
        RenderTexture.active = renderTexture;
        screenShotJustTaken.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShotJustTaken.Apply();

        // Reset lại targetTexture
        arCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        OnCaptureScreen?.Invoke(screenShotJustTaken);

        // Lưu ảnh vào thư mục (hoặc xử lý ảnh)
        // string filePath = Path.Combine(Application.persistentDataPath, "AR_Capture.png");
        // File.WriteAllBytes(filePath, screenshot.EncodeToPNG());

        // Debug.Log($"Image saved to: {filePath}");
    }

    public void SaveImageJustTaken()
    {
        if(screenShotJustTaken == null)
        {
            Debug.Log("ảnh chụp null");
        }
        else 
        {
            Debug.Log("ảnh chụp không null");
        }
        NativeGallery.SaveImageToGallery(screenShotJustTaken, "GalleryTest", "Image.png", ( success, path ) => Debug.Log( "Media save result: " + success + " " + path ) );
    }
}
