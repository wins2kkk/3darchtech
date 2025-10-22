using System;
using System.Collections;
using System.Collections.Generic;
using ARGame2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenesisInventory : MonoBehaviour
{
    public static Action OnCloseInventory;
    public static Action<ARGenesisBall> OnGenesisInventoryChange;

    [SerializeField] private Transform _content;
    [SerializeField] private List<ARGenesisBall> aRGenesisBall;

    [SerializeField] private TextMeshProUGUI _textFes;
    [SerializeField] private TextMeshProUGUI _textID;
    [SerializeField] private TextMeshProUGUI _genesisName;

    [SerializeField] private EnergyBar _energyBar;

    [SerializeField] private Button _buttonClose;
    
    [SerializeField] private InfiniteScroll _infiniteScroll;
    [SerializeField] private GenesisFoodMgr _genesisFoodMgr;
    [SerializeField] private FlexibleUIMgr _flexibleUIMgr;

    [SerializeField] private GameObject _inventory3D;
    [SerializeField] private GameObject _modelRotation;
    [SerializeField] private GameObject _panelInventory; 

    private GenesisInventoryItem[] _listGenesisItem;

    private ARGenesisBall _currentGenesisSelect;

    private void Awake() 
    {
        _listGenesisItem = _content.GetComponentsInChildren<GenesisInventoryItem>();
    }

    private void Start() 
    {
        InitItem();    
    }

    private void InitItem()
    {
        foreach(var item in _listGenesisItem)
        {
            item.Init(this);
        }
    }

    private void OnEnable() 
    {
        _infiniteScroll.OnSelectMiddleItem.AddListener(OnSelectItem);   
        _infiniteScroll.OnUnSelectMiddleItem.AddListener(OnUnSelectItem);   
        _buttonClose.onClick.AddListener(OnClickButtonClose);
    }

    private void OnDisable()
    {
        _infiniteScroll.OnSelectMiddleItem.RemoveListener(OnSelectItem);   
        _infiniteScroll.OnUnSelectMiddleItem.RemoveListener(OnUnSelectItem); 
        _buttonClose.onClick.RemoveListener(OnClickButtonClose);
    }

    private void OnSelectItem(GameObject item)
    {
        if(item == null) 
            return;
        GenesisInventoryItem giitem = item.GetComponent<GenesisInventoryItem>();

        giitem.ScaleUp();

        ShowARGenesis(giitem.eGenesisType);
    }

    private void OnUnSelectItem(GameObject item)
    {
        if(item == null) 
            return;
        GenesisInventoryItem giitem = item.GetComponent<GenesisInventoryItem>();

        giitem.ScaleDown();
    }

    private void ShowARGenesis(EGenesisType eGenesisType)
    {
        foreach(var arBall in aRGenesisBall)
        {
            if(arBall.eGenesisType == eGenesisType)
            {
                _currentGenesisSelect?.OnGenesisEatEnergy.RemoveListener(OnEnergyCurrnetGenesisChange);
                _currentGenesisSelect = arBall;
                _currentGenesisSelect.OnGenesisEatEnergy.AddListener(OnEnergyCurrnetGenesisChange);

                _currentGenesisSelect.gameObject.SetActive(true);
                UpdateInfo(_currentGenesisSelect);
                UpdateFood(arBall);

                _flexibleUIMgr.UpdateAllFlexibleUI(_currentGenesisSelect.eElementType);

                OnGenesisInventoryChange?.Invoke(_currentGenesisSelect);
            }
            else
            {
                arBall.gameObject.SetActive(false);
            }
        }
    }

    public void ScrollToItem(GameObject item)
    {
        _infiniteScroll.ScrollToItem(item);
    }

    private void UpdateInfo(ARGenesisBall aRGenesisBall)
    {
        _textFes.text = $"{aRGenesisBall.fesValue} FES";
        _textID.text = $"ID: {aRGenesisBall.id}";
        _genesisName.text = aRGenesisBall.nameGenesis;
        _energyBar.UpdateEnergyBarSmooth(aRGenesisBall.energy);
    }

    private void UpdateFood(ARGenesisBall aRGenesisBall)
    {
        _genesisFoodMgr.SelectFoodForCurrentGenesis(aRGenesisBall);
    }

    private void OnEnergyCurrnetGenesisChange()
    {
        UpdateInfo(_currentGenesisSelect);
    }
 
    private void OnClickButtonClose()
    {
        gameObject.SetActive(false);
        _inventory3D.SetActive(false);
        _modelRotation.SetActive(false);
        _panelInventory.SetActive(true);
        OnCloseInventory?.Invoke();
    }
}
