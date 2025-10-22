using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation.VisualScripting;

public class UICaptureImageMgr : MonoBehaviour
{
    [Header("Max Image")]
    [SerializeField] private GameObject _maxImageCapture;
    [SerializeField] private RectTransform _maxImageBorder;
    [SerializeField] private Image _maxImage;
    [SerializeField] private Button _buttonSaveImage;
    [SerializeField] private Button _buttonCloseImage;

    [Header("Mini Image")]
    [SerializeField] private RectTransform _miniImageBorder;
    [SerializeField] private Image _miniImage;
    [SerializeField] private Button _miniImageButton;

    private Sprite _spriteImageCapture;

    private const float k_MiniImageWidthRatio = 0.2145f;
    private const float k_MaxImageRatio = 0.6f;

    private void OnEnable() 
    {
        ARCameraCapture.OnCaptureScreen += ShowImageMini;
        _buttonSaveImage.onClick.AddListener(SaveImage);
        _miniImageButton.onClick.AddListener(OpenMaximumImage);
        _buttonCloseImage.onClick.AddListener(OnClickClose);
    }

    private void OnDisable()
    {
        ARCameraCapture.OnCaptureScreen -= ShowImageMini;
        _buttonSaveImage.onClick.RemoveListener(SaveImage);
        _miniImageButton.onClick.RemoveListener(OpenMaximumImage);
        _buttonCloseImage.onClick.RemoveListener(OnClickClose);
    }

    private void ShowImageMini(Texture2D texture2D)
    {
        ClearSprite();

        _spriteImageCapture = Sprite.Create(
            texture2D, 
            new Rect(0, 0, texture2D.width, texture2D.height), 
            new Vector2(0.5f, 0.5f)
        );

        _miniImage.sprite = _spriteImageCapture;

        _miniImageBorder.sizeDelta = new Vector2(Screen.width * k_MiniImageWidthRatio, Screen.height * k_MiniImageWidthRatio);

        _miniImageBorder.gameObject.SetActive(true);
    }

    private void OpenMaximumImage()
    {
        _maxImageBorder.sizeDelta = new Vector2(Screen.width * k_MaxImageRatio, Screen.height * k_MaxImageRatio);

        _miniImageBorder.gameObject.SetActive(false);
        _maxImageCapture.gameObject.SetActive(true);

        _maxImage.sprite = _spriteImageCapture;
    }

    private void SaveImage()
    {
        ARCameraCapture.Instance.SaveImageJustTaken();
        OnClickClose();
    }

    private void OnClickClose()
    {
        _maxImageCapture.SetActive(false);

        ClearSprite();
    }

    private void ClearSprite()
    {
        if(_spriteImageCapture?.texture != null)
        {
            Destroy(_spriteImageCapture.texture);
        }
    }
}
