using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthVFXController : MonoBehaviour
{
    [SerializeField] private GameObject _startEarthVFX;
    [SerializeField] private GameObject _endEarthVFX;

    [SerializeField] private float _timeToEnablEndVFX;


    private void OnEnable()
    {
        _startEarthVFX.gameObject.SetActive(true);
        _endEarthVFX.gameObject.SetActive(false);

        EnableStartVFX();
    }

    private void OnDisable()
    {
        _startEarthVFX.gameObject?.SetActive(false);
        _endEarthVFX?.gameObject?.SetActive(false);
    }



    private void EnableStartVFX()
    {
        _startEarthVFX.gameObject.SetActive(true);

        StartCoroutine(EnableEndVFX());
        
    }
    private IEnumerator EnableEndVFX()
    {

        yield return new WaitForSeconds(_timeToEnablEndVFX);
        _startEarthVFX.gameObject.SetActive(false);
        _endEarthVFX.gameObject.SetActive(true) ;


        StartCoroutine(DIsableEndVFX());
    }



    private IEnumerator DIsableEndVFX()
    {
        yield return new WaitForSeconds(4f);
        _endEarthVFX.gameObject.SetActive(false);
    }
}
