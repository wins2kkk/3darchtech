using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVFXController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _waterFallVFX;
    [SerializeField] private ParticleSystem _waterPuddkleVFX;
    [SerializeField] private ParticleSystem _waterPuddlePlaneVFX;
    [SerializeField] private GameObject _waterPlaneObject;
    [SerializeField] private GameObject _waterBall;




    [SerializeField] private float _timeToEnablePuddelVFX;
    private float _timeToEnablePuddlePlaneVFX = 0.2f;

    private void OnEnable()
    {
        _waterFallVFX?.Clear();
        _waterPuddlePlaneVFX?.Clear();
        _waterPuddkleVFX?.Clear();
        _waterPlaneObject.gameObject.SetActive(false);
        _waterBall?.gameObject.SetActive(false);

        EnableWaterFalling();
    }
    private void EnableWaterFalling()
    {
        _waterFallVFX.Play();
        StartCoroutine(EnablePuddleVFX());
    }

    private IEnumerator EnablePuddleVFX()
    {
        yield return new WaitForSeconds(_timeToEnablePuddelVFX);
        _waterPuddkleVFX.Play();

        StartCoroutine( EnablePuddlePlaneVFX());
    }

   
    private IEnumerator EnablePuddlePlaneVFX()
    {
        yield return new WaitForSeconds(_timeToEnablePuddlePlaneVFX);
        _waterPuddlePlaneVFX.Play();
        //  _WaterPlaneObject.gameObject.SetActive(true);
        StartCoroutine(EnableBallEffect());
    }

    private IEnumerator EnableBallEffect()
    {
        yield return new WaitForSeconds(0.9f);
        _waterBall.SetActive(true);
        _waterPuddlePlaneVFX.gameObject.SetActive(false);
    }


}
