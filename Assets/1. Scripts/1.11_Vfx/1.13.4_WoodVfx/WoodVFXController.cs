using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodVFXController : MonoBehaviour
{
    [SerializeField] private GameObject _woodApear_VFX;
    [SerializeField] private GameObject _WoodDisapear_VFX;

    [SerializeField] private float _timeToEnableDisapearVFX;//  default 2


    [SerializeField] private float _timeToTurnOffApearVFX;
    private void OnEnable()
    {
        _woodApear_VFX.gameObject.SetActive(true);
        _WoodDisapear_VFX.gameObject.SetActive(false);


        StartCoroutine(EnableDisapearVFX());

    }


    private void OnDisable()
    {
        
    }

    private IEnumerator EnableDisapearVFX()
    {
        yield return new WaitForSeconds(_timeToEnableDisapearVFX);

        StartCoroutine(TurnOffApearVFX());
        _WoodDisapear_VFX.gameObject.SetActive(true);
    }
    private IEnumerator TurnOffApearVFX()
    {
        yield return new WaitForSeconds(_timeToTurnOffApearVFX);
        _woodApear_VFX.gameObject.SetActive(false);
    }
}
