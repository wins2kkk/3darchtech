using System.Collections;
using System.Collections.Generic;
using ARGame2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLinearTextMeshPro : FlexibleUI
{
    [SerializeField] private ChangeTMPScriptable changeTMPScriptable;
    private TextMeshProUGUI textMeshProUGUI;

    private void Awake() 
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();    
    }

    private void Start() 
    {
        FlexibleByType(eElementType);
    }

    public override void FlexibleByType(EElementType eElementType)
    {
        base.FlexibleByType(eElementType);

        TMPConfig tMPConfig = changeTMPScriptable.GetTMPConfig(eElementType);
        VertexGradient gradient = new VertexGradient(
                tMPConfig.topLeft,
                tMPConfig.topRight,
                tMPConfig.bottomLeft,
                tMPConfig.bottomRight
            );

        textMeshProUGUI.colorGradient = gradient;
    }
}
