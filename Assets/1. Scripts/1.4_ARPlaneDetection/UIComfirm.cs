using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class UIComfirm : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _buttonCancel;
    [SerializeField] private Button _buttonComfirm;

    [SerializeField] private ObjectSpawner objectSpawner;

    private GameObject _currentARObject;

    private void OnEnable() 
    {
        objectSpawner.objectSpawned += ShowButton;
        ARPlaneObject.OnDeleteARObject += ClearAndClose;

        _buttonCancel.onClick.AddListener(OnClickCancel);
        _buttonComfirm.onClick.AddListener(OnClickComfirm);
    }

    private void OnDisable()
    {
        ARPlaneObject.OnDeleteARObject -= ClearAndClose;

        _buttonCancel.onClick.RemoveListener(OnClickCancel);
        _buttonComfirm.onClick.RemoveListener(OnClickComfirm);
    }

    private void ShowButton(GameObject gameObject)
    {
        gameObject.GetComponent<ARPlaneObject>().Visible(EVisibleState.transparent);
        _currentARObject = gameObject;
        _panel.SetActive(true);
    }

    private void OnClickCancel()
    {
        Destroy(_currentARObject);
        _panel.SetActive(false);
    }

    private void OnClickComfirm()
    {
        _currentARObject.GetComponent<ARPlaneObject>().Visible(EVisibleState.visible);
        ClearAndClose();
    }

    private void ClearAndClose()
    {
        _currentARObject = null;
        _panel.SetActive(false);
    }
}
