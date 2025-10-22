using System.Collections;
using System.Collections.Generic;
using ARGame2;
using UnityEngine;

public class FlexibleUIMgr : MonoBehaviour
{
    private FlexibleUI[] flexibleUIs;
    
    private void Awake() 
    {
        flexibleUIs = GetComponentsInChildren<FlexibleUI>(); 
    } 

    public void UpdateAllFlexibleUI(EElementType type)
    {
        foreach(var flexui in flexibleUIs)
        {
            flexui.FlexibleByType(type);
        }
    }
}
