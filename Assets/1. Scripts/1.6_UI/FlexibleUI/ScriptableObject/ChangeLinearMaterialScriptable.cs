using System.Collections.Generic;
using ARGame2;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeLinearMaterialScriptable", menuName = "ScriptableObjects/ChangeLinearMaterialScriptable", order = 1)]
public class ChangeLinearMaterialScriptable : ScriptableObject
{
    public Material originMaterial;
    public List<LinearMaterialConfig> linearMaterialConfigs;

    public LinearMaterialConfig GetColor(EElementType eElementType)
    {
        foreach(var cf in linearMaterialConfigs)
        {
            if(cf.eElementType == eElementType)
            {
                return cf;
            }
        }

        return null;
    }
}

[System.Serializable]
public class LinearMaterialConfig
{
    public Color startColor;
    public Color endColor;
    public EElementType eElementType;
}