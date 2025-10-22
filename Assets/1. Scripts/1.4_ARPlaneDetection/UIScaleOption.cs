using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class UIScaleOption : MonoBehaviour
{
    public int ratio;

    [SerializeField] private Button _mainButton;
    [SerializeField] private GameObject _backgroundHighlight;

    private UIScaleMenuOptions uIScaleMenuOptions;

    private void OnEnable() 
    {
        _mainButton.onClick.AddListener(OnClickButton);    
    }

    private void OnDisable() 
    {
        _mainButton.onClick.RemoveListener(OnClickButton);    
    }

    public void Init(UIScaleMenuOptions uIScaleMenuOptions)
    {
        this.uIScaleMenuOptions = uIScaleMenuOptions; 
    }

    private void OnClickButton()
    {
        uIScaleMenuOptions.OnSelectScale(this);
    }

    public void Select(bool isSelect)
    {
        _backgroundHighlight.gameObject.SetActive(isSelect);
    }
}
