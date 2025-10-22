using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderToggle : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _mainButton;

    public UnityEvent<int> onValueChange;

    private void OnEnable() 
    {
        _mainButton.onClick.AddListener(OnClickButton);    
    }

    private void OnDisable()
    {
        _mainButton.onClick.RemoveListener(OnClickButton);     
    }

    private void OnClickButton()
    {
        float cur = _slider.value;
        _slider.value = cur == 0 ? 1 : 0;
        onValueChange?.Invoke((int)_slider.value);
    }
}
