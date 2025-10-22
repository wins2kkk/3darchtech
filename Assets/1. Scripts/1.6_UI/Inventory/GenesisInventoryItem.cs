using ARGame2;
using UnityEngine;
using UnityEngine.UI;

public class GenesisInventoryItem : MonoBehaviour 
{
    public EGenesisType eGenesisType;

    private Button _mainButton;
    private GenesisInventory _genesisInventory;

    public void Init(GenesisInventory mgr)
    {
        _genesisInventory = mgr;
    }

    private void Awake() 
    {
        _mainButton = GetComponent<Button>();    
    }

    private void OnEnable() 
    {
        _mainButton.onClick.AddListener(OnClickItem);
    }

    private void OnDisable() 
    {
        _mainButton.onClick.RemoveListener(OnClickItem);
    }

    private void OnClickItem()
    {
        _genesisInventory.ScrollToItem(gameObject); //scroll to self item
    }

    public void ScaleUp()
    {
        transform.localScale = Vector3.one * 1.6f;
    }

    public void ScaleDown()
    {
        transform.localScale = Vector3.one;
    }
}