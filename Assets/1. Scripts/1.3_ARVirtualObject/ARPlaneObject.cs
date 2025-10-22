using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVisibleState
{
    visible,
    invisible,
    transparent,
}

public class ARPlaneObject : MonoBehaviour
{
    public static event Action OnDeleteARObject;
    public int defaultRatio => _defaultRatio;
    public Vector3 minScale => _minScale;

    [SerializeField] private int _defaultRatio;
    [SerializeField] private Transform _model;

    private Vector3 _minScale;
    private MeshRenderer[] _listMesh;

    private void Awake()
    {
        _minScale = transform.localScale / 1000;
        _listMesh = _model.GetComponentsInChildren<MeshRenderer>();
    }

    public void Visible(EVisibleState eVisibleState)
    {
        switch(eVisibleState)
        {
            case EVisibleState.visible:
                StartCoroutine(Opaque());
            break;
            case EVisibleState.invisible:

            break;
            case EVisibleState.transparent:
                StartCoroutine(Transparent());
            break;
        }
    }

    private IEnumerator Transparent()
    {
        foreach(var mesh in _listMesh)
        {
            mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, 0.4f);
            ChangeTransparency(true, mesh.material);
        }
        yield return null;
    }

    private IEnumerator Opaque()
    {
        foreach(var mesh in _listMesh)
        {
            mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, 1f);
            ChangeTransparency(false, mesh.material);
        }
        yield return null;
    }

    private void OnDestroy() 
    {
        OnDeleteARObject?.Invoke();
    }

#region ChangeMaterialTransparent
    public enum SurfaceType
    {
        Opaque,
        Transparent
    }

    public enum BlendMode
    {
        Alpha,
        Premultiply,
        Additive,
        Multiply
    }

    public void ChangeTransparency(bool transparent, Material material)
    {
        if (transparent)
        {
            material.SetFloat("_Surface", (float)SurfaceType.Transparent);
            material.SetFloat("_Blend", (float)BlendMode.Alpha);
        }
        else
        {
            material.SetFloat("_Surface", (float)SurfaceType.Opaque);
        }
        SetupMaterialBlendMode(material);
    }

    void SetupMaterialBlendMode(Material material)
    {
        if (material == null)
            throw new ArgumentNullException("material");

        bool alphaClip = material.GetFloat("_AlphaClip") == 1;
        if (alphaClip)
            material.EnableKeyword("_ALPHATEST_ON");
        else
            material.DisableKeyword("_ALPHATEST_ON");

        SurfaceType surfaceType = (SurfaceType)material.GetFloat("_Surface");
        if (surfaceType == 0)
        {
            material.SetOverrideTag("RenderType", "");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
            material.SetShaderPassEnabled("ShadowCaster", true);
        }
        else
        {
            BlendMode blendMode = (BlendMode)material.GetFloat("_Blend");
            switch (blendMode)
            {
                case BlendMode.Alpha:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    material.SetShaderPassEnabled("ShadowCaster", false);
                    break;
                case BlendMode.Premultiply:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    material.SetShaderPassEnabled("ShadowCaster", false);
                    break;
                case BlendMode.Additive:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    material.SetShaderPassEnabled("ShadowCaster", false);
                    break;
                case BlendMode.Multiply:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.DstColor);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    material.SetShaderPassEnabled("ShadowCaster", false);
                    break;
            }
        }
    }
#endregion ChangeMaterialTransparent
}
