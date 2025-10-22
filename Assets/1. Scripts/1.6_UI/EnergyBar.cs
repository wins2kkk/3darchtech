using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private RectTransform _bounnd;
    [SerializeField] private RectTransform _energyBar;

    [SerializeField] private TextMeshProUGUI _textPercent;

    private Coroutine _currentCoroutine;

    private void OnEnable() 
    {

    }

    private void OnDisable() 
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
    }

    public void UpdateEnergyBarSmooth(float targetPercent, float duration = 1)
    {
        // Nếu có một coroutine đang chạy, dừng nó trước
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        // Bắt đầu một coroutine mới
        _currentCoroutine = StartCoroutine(SmoothUpdate(targetPercent, duration));
    }

    private IEnumerator SmoothUpdate(float targetPercent, float duration)
    {
        // Clamp targetPercent để đảm bảo nó nằm trong khoảng 0 - 100
        targetPercent = Mathf.Clamp(targetPercent, 0, 100);

        // Lấy giá trị hiện tại của chiều rộng thanh năng lượng và phần trăm hiện tại
        float currentWidth = _energyBar.rect.width;
        float targetWidth = (_bounnd.rect.width * targetPercent) / 100f;

        float currentPercent = (currentWidth / _bounnd.rect.width) * 100f;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Tính toán giá trị hiện tại giữa current và target
            float newWidth = Mathf.Lerp(currentWidth, targetWidth, elapsedTime / duration);
            float newPercent = Mathf.Lerp(currentPercent, targetPercent, elapsedTime / duration);

            // Cập nhật chiều rộng và phần trăm hiển thị
            _energyBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
            _textPercent.text = $"{Mathf.RoundToInt(newPercent)}%";

            yield return null; // Đợi frame tiếp theo
        }

        // Đảm bảo giá trị cuối cùng là chính xác
        _energyBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
        _textPercent.text = $"{Mathf.RoundToInt(targetPercent)}%";
    }

}
