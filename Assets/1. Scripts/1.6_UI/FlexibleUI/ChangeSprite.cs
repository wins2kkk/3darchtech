using ARGame2;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : FlexibleUI
{
    [SerializeField] protected ChangeSpriteScriptable changeSpriteScriptable;

    private Image image;

    private void Awake() 
    {
        image = GetComponent<Image>();    
    }

    private void Start() 
    {
        FlexibleByType(eElementType);
    }

    public override void FlexibleByType(EElementType eElementType)
    {
        base.FlexibleByType(eElementType);
        image.sprite = changeSpriteScriptable.GetSprite(eElementType);
    }
}
