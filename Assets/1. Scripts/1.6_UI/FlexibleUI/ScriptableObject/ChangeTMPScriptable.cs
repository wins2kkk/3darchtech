using System.Collections.Generic;
using ARGame2;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeTMPScriptable", menuName = "ScriptableObjects/ChangeTMPScriptable", order = 1)]
public class ChangeTMPScriptable : ScriptableObject
{
    public List<TMPConfig> spriteConfigs;

    public TMPConfig GetTMPConfig(EElementType eElementType)
    {
        foreach(var cf in spriteConfigs)
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
public class TMPConfig
{
    public Color topLeft;
    public Color topRight;
    public Color bottomLeft;
    public Color bottomRight;
    public EElementType eElementType;
}