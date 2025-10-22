using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.PlayerLoop;
using System;

public class ARRaycastHitFollower : MonoBehaviour
{
    public static event Action OnTouchInIndicator;

    public GameObject _indicator;
    public ARRaycastManager arRaycastManager;
    public ObjectSpawner objectSpawner; 
    public LayerMask _indicatorLayerMask;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private ARRaycastHit _currenrtARRaycastHit; 

    private bool _isShowIndicator = true;

    private void OnEnable() 
    {
        ARPlaneObject.OnDeleteARObject += OnDeleteObject;
        objectSpawner.objectSpawned += OnCreateARObject;
    }

    private void OnDisable()
    {
        ARPlaneObject.OnDeleteARObject -= OnDeleteObject;
        objectSpawner.objectSpawned -= OnCreateARObject;
    }

    void Update()
    {
        if(_isShowIndicator == false) return;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (arRaycastManager.Raycast(ray, hits, TrackableType.PlaneWithinBounds))
        {
            _currenrtARRaycastHit = hits[0];

            Pose hitPose = _currenrtARRaycastHit.pose;

            if (!(_currenrtARRaycastHit.trackable is ARPlane arPlane))
                    return;

            _indicator.transform.position = hitPose.position;
            _indicator.SetActive(true);
        }
        else
        {
            _indicator.SetActive(false);
        }

        TouchInIndicator();
    }

    private void TouchInIndicator()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, _indicatorLayerMask))
            {
                OnTouchInIndicator?.Invoke();
            }
        }
    }

    public bool TryToGetCurrentRaycastHitPoint(out ARRaycastHit aRRaycastHit)
    {
        if(_indicator.activeSelf)
        {
            aRRaycastHit = _currenrtARRaycastHit;
            return true;
        }
        else 
        {
            aRRaycastHit = default;
            return false;
        }
    }

    private void OnCreateARObject(GameObject GameObject)
    {
        _isShowIndicator = false;
        _indicator.SetActive(false);
    }

    private void OnDeleteObject()
    {
        _isShowIndicator = true;
        _indicator.SetActive(true);
    }
}
