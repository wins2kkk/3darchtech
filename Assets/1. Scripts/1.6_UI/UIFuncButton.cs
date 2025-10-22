using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFuncButton : MonoBehaviour
{
    public static event Action<bool> ShowInfoARGenesis;
    public static event Action OnOpenInventory;

    [SerializeField] private Button _buttonShowInfo;
    [SerializeField] private Button _buttonCamera;
    [SerializeField] private Button _buttonInventory;

    [SerializeField] private GameObject _highLightButtonShowInfo;

    //Inventory
    [SerializeField] private GameObject Inventory2D;
    [SerializeField] private GameObject Inventory3D;
    [SerializeField] private GameObject ModelRotation;

    private bool _isInfoVisible;

    private void Start() 
    {
        OnClickButtonShowInfo();
    }

    private void OnEnable() 
    {
        _buttonShowInfo.onClick.AddListener(OnClickButtonShowInfo);
        _buttonInventory.onClick.AddListener(OnClickButtonInventory);
    }

    private void OnDisable() 
    {
        _buttonShowInfo.onClick.RemoveListener(OnClickButtonShowInfo);
        _buttonInventory.onClick.RemoveListener(OnClickButtonInventory); 
    }

    private void OnClickButtonShowInfo()
    {
        _isInfoVisible = !_isInfoVisible;
        _highLightButtonShowInfo.SetActive(_isInfoVisible);
        ShowInfoARGenesis?.Invoke(_isInfoVisible);
    }

    private void OnClickButtonInventory()
    {
        Inventory2D.SetActive(true);
        Inventory3D.SetActive(true);
        ModelRotation.SetActive(true);
        gameObject.SetActive(false);
        OnOpenInventory?.Invoke();
    }

}
