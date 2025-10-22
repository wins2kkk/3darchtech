using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class UIScaleMenuOptions : MonoBehaviour
{
    private List<UIScaleOption> uIScaleOptions;
    private Vector3 _minScale;
    private float _defaultHeightChild;

    [SerializeField] private ObjectSpawner objectSpawner;
    [SerializeField] private GameObject layout;
    
    public static event Action<int> OnSelectScaleRatio;

    private void OnEnable() 
    {
        objectSpawner.objectSpawned += OnObjectSpawn;   
        ARPlaneObject.OnDeleteARObject += OnObjectDelete;
    }

    private void OnDisable()
    {
        objectSpawner.objectSpawned -= OnObjectSpawn;   
        ARPlaneObject.OnDeleteARObject -= OnObjectDelete;
    }

    private void Awake() 
    {
        layout.SetActive(false);
        
        uIScaleOptions = layout.GetComponentsInChildren<UIScaleOption>().ToList();    

        foreach(var op in uIScaleOptions)
        {
            op.Init(this);
        }
    }

    public void OnSelectScale(UIScaleOption uIScaleOption)
    {
        foreach(var op in uIScaleOptions)
        {
            if(op == uIScaleOption)
            {
                op.Select(true);
            }
            else 
            {
                op.Select(false);
            }
        }
        ScaleObject(uIScaleOption.ratio);
        OnSelectScaleRatio?.Invoke(uIScaleOption.ratio);
    }

    private void SelectRatio(int ratio)
    {
        foreach(var op in uIScaleOptions)
        {
            if(op.ratio == ratio)
            {
                op.Select(true);
            }
            else 
            {
                op.Select(false);
            }
        }
    }

    private void OnObjectSpawn(GameObject arObject)
    {
        ARPlaneObject aRPlaneObject = arObject.GetComponent<ARPlaneObject>();
        SelectRatio(aRPlaneObject.defaultRatio);
        _minScale = aRPlaneObject.minScale;

        _defaultHeightChild = arObject.transform.GetChild(0).transform.position.y;
        layout.SetActive(true);
    }

    private void OnObjectDelete()
    {
        layout.SetActive(false);
    }

    private void ScaleObject(int scaleRatio)
    {
        // Vector3 meshPosition = objectSpawner.currentARObject.transform.GetChild(0).position;
        // if(scaleRatio == 1)
        // {
        //     float y = objectSpawner.currentARObject.transform.position.y;
        //     objectSpawner.currentARObject.transform.GetChild(0).transform.localPosition = new Vector3(meshPosition.x, y-objectSpawner.currentARObject.transform.position.y, meshPosition.y);
        // }
        // else 
        // {
        //     objectSpawner.currentARObject.transform.GetChild(0).transform.localPosition = new Vector3(meshPosition.x, _defaultHeightChild, meshPosition.y);
        // }
        objectSpawner.currentARObject.transform.localScale = 1000/scaleRatio * _minScale;
    }
}
