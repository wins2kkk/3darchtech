using System.Collections;
using System.Collections.Generic;
using ARGame2;
using UnityEngine;

public class GenesisInventory3D : MonoBehaviour
{
    [SerializeField] private FlexibleUIMgr flexibleUIMgr;

    private void OnEnable() 
    {
        GenesisInventory.OnGenesisInventoryChange += OnGenesisInventoryChange;
    }

    private void OnDisable()
    {
        GenesisInventory.OnGenesisInventoryChange -= OnGenesisInventoryChange;
    }

    private void OnGenesisInventoryChange(ARGenesisBall aRGenesisBall)
    {
        flexibleUIMgr.UpdateAllFlexibleUI(aRGenesisBall.eElementType);
    }
}
