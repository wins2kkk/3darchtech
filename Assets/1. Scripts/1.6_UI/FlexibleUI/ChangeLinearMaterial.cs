using System.Collections;
using System.Collections.Generic;
using ARGame2;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLinearMaterial : FlexibleUI
{
    [SerializeField] private ChangeLinearMaterialScriptable _changeLinearMaterialScriptable;

    private Image _image;

    private void Awake() 
    {
        _image = GetComponent<Image>();
        _image.material = Instantiate(_changeLinearMaterialScriptable.originMaterial);
    }

    public override void FlexibleByType(EElementType eElementType)
    {
        base.FlexibleByType(eElementType);

        var config = _changeLinearMaterialScriptable.GetColor(eElementType);

        _image.material.SetColor("_Color", config.startColor);
        _image.material.SetColor("_Color2", config.endColor);
    }
}
