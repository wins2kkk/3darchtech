using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private LayerMask _objTouchableLayerMask;
    [SerializeField] private float _maxDistance = 100f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, _maxDistance, layerMask: _objTouchableLayerMask))
                {
                    if(hit.transform.TryGetComponent(out IPlayerCanTouchable obj))
                    {
                        obj.OnPlayerTouch();
                    }
                }
            }
        }
    }
}
