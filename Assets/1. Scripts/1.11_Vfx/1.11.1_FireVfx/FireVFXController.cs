using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FireVFXController : MonoBehaviour
{
    [SerializeField] private GameObject _fire_Ball_apear_vfx;
    [SerializeField] private GameObject _explosion_vfx;
    [SerializeField] private GameObject _fire_ball_after_explouse_vfx;


    [SerializeField] private float _timeBeforeExploude;
    [SerializeField] private float _timeDisplayBallAfterExploude = 0.2f;

    private void OnEnable()
    {
        _fire_Ball_apear_vfx.SetActive(true);
        _explosion_vfx.SetActive(false);
        _fire_ball_after_explouse_vfx.SetActive(false);
        StartCoroutine(DisplayEffectCouroutine());
    }

    //private void OnDisable()
    //{
    //    _fire_Ball_apear_vfx.SetActive(true);
    //    _explosion_vfx.SetActive(false);

    //}

    private IEnumerator DisplayEffectCouroutine()
    {

        yield return new WaitForSeconds(_timeBeforeExploude);
        _explosion_vfx.SetActive(true);
        _fire_Ball_apear_vfx.SetActive(false);

        StartCoroutine(DisPlayBallAfterExplouseCourountine());

    }

    private IEnumerator DisPlayBallAfterExplouseCourountine()
    {
        yield return new WaitForSeconds(_timeDisplayBallAfterExploude);
        _fire_ball_after_explouse_vfx.SetActive(true);
    }
}
