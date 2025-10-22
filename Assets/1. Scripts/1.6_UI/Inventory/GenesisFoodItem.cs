using System.Collections;
using System.Collections.Generic;
using ARGame2;
using UnityEngine;
using UnityEngine.UI;

public class GenesisFoodItem : MonoBehaviour
{
    public EElementType eElementType;

    private GenesisFoodMgr _genesisFoodMgr;
    private Button _mainButton;

    private bool _isSelect;

    public void Init(GenesisFoodMgr mgr)
    {
        _genesisFoodMgr = mgr;
    }

    private void Awake() 
    {
        _mainButton = GetComponent<Button>();
    }

    private void OnEnable() 
    {
        _mainButton.onClick.AddListener(OnClickFoodItem);
    }

    private void OnDisable()
    {
        _mainButton.onClick.RemoveListener(OnClickFoodItem);
    }

    private void OnClickFoodItem()
    {
        if(_isSelect)
        {
            _genesisFoodMgr.FeedGenesis();
        }
        else 
        {

        }
    }

    public void Select()
    {
        _isSelect = true;
        transform.localScale = Vector3.one * 2f;
    }

    public void UnSelect()
    {
        _isSelect = false;
        transform.localScale = Vector3.one;
    }
}
