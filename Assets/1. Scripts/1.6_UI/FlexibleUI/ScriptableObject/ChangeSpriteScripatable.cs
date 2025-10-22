using System.Collections.Generic;
using ARGame2;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeSpriteScriptable", menuName = "ScriptableObjects/ChangeSpriteScriptable", order = 1)]
public class ChangeSpriteScriptable : ScriptableObject
{
    public List<SpriteConfig> spriteConfigs;

    public Sprite GetSprite(EElementType eElementType)
    {
        foreach(var cf in spriteConfigs)
        {
            if(cf.eElementType == eElementType)
            {
                return cf.sprite;
            }
        }

        return null;
    }
}

[System.Serializable]
public class SpriteConfig
{
    public Sprite sprite;
    public EElementType eElementType;
}