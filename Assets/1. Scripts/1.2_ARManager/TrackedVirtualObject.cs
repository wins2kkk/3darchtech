using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Mathematics;

namespace ARGame2
{
    public class TrackedVirtualObject
    {
        private GameObject _virtualObject;
        public GameObject virtualObject => _virtualObject;

        public ARGenesisBall _aRGenesisBall;

        private M_ARObjectTrackingMgr m_ARObjectTrackingMgr;

        private bool _isUpdateRot;

        private float _delayUpdatingRotTime;
        private float _stopUpdatingRotTime;

        private const float K_Max_Delta_Angle_Update_Rot = 3f;
        private const float K_Deplay_Update_Rot_Time = 0.2f;
        private const float K_Stop_Update_Rot_Time = 0.5f;
        private const float K_Possition_Update = 0.1f;
        private static bool _checkTimeIdle = true;

        public bool CheckTimeIdle => _checkTimeIdle;

        public bool IsTracking;

        
        public void Add(ARTrackedImage trackedImage, GameObject instantiatedPref, ARGenesisBall aRGenesisBall, M_ARObjectTrackingMgr m_ARObjectTrackingMgr)
        {
            _virtualObject = instantiatedPref;
            
            _aRGenesisBall = aRGenesisBall;

            _virtualObject.transform.SetPositionAndRotation(trackedImage.transform.position,
                                                               trackedImage.transform.rotation);

            this.m_ARObjectTrackingMgr = m_ARObjectTrackingMgr;

            Deactivate();

        }

        public void TryToUpdate(ARTrackedImage trackedImage)
        {
            Debug.Log("AR DEBUG: " + trackedImage.referenceImage.name + " " + trackedImage.trackingState);
            #if UNITY_IOS
                TryToUpdateForIOS(trackedImage);
            #elif UNITY_ANDROID
                TryToUpdateForAndroid(trackedImage);
            #endif

            IsTracking = trackedImage.trackingState == TrackingState.Tracking;
            if(IsTracking)
            {
                m_ARObjectTrackingMgr.SetCurrentObjectTracking(this);
            }
            else 
            {
                m_ARObjectTrackingMgr.SetCurrentObjectTracking(null);
            }
        }

        private void TryToUpdateForIOS(ARTrackedImage trackedImage)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                _virtualObject.transform.SetLocalPositionAndRotation(trackedImage.transform.position,
                                                                trackedImage.transform.rotation);
                // _virtualObject.transform.SetPositionAndRotation(trackedImage.transform.position,
                //                                                 trackedImage.transform.rotation);
                Activate();
            }
            else
            {
                Deactivate();
            }

        }

        private void TryToUpdateForAndroid(ARTrackedImage trackedImage)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                //--------------------------------------------
                //try to reduce rotation jitter for Android
                //--------------------------------------------
                if (_delayUpdatingRotTime > 0)
                {
                    _delayUpdatingRotTime -= Time.deltaTime;

                    _virtualObject.transform.SetPositionAndRotation(trackedImage.transform.position,
                    trackedImage.transform.rotation);
                }
                else
                {
                    //update position
                    if(DetectChange(trackedImage.transform.position, _virtualObject.transform.position))
                    {
                        _virtualObject.transform.position = trackedImage.transform.position;
                    }
                    
                    //update rotation
                    Vector3 currentRot = trackedImage.transform.rotation.eulerAngles;
                    Vector3 lastRot = _virtualObject.transform.rotation.eulerAngles;
                    float angleChange = Vector3.Angle(lastRot, currentRot);

                    if (angleChange < K_Max_Delta_Angle_Update_Rot)
                    {
                        if (_isUpdateRot == true)
                        {
                            if (_stopUpdatingRotTime > 0)
                                _stopUpdatingRotTime -= Time.deltaTime;
                            else
                                _isUpdateRot = false;

                            _virtualObject.transform.rotation = trackedImage.transform.rotation;
                        }

                    }
                    else
                    {
                        if (_isUpdateRot)
                        {
                            _virtualObject.transform.rotation = trackedImage.transform.rotation;
                        }
                    }

                    Activate();
                }
            }
            else
            {

                Deactivate();
            }
        }

        public void Activate()
        {
            _checkTimeIdle = false;
            // if (_virtualObject.activeSelf == true) { return; }

            _aRGenesisBall.ActiveWithEffect();
        }

        public void Deactivate()
        {
            _checkTimeIdle = true;
            // if(_virtualObject.activeSelf == false) { return; }

            _aRGenesisBall.DeActiveWithEffect();
            Reset();
        }

        private void Reset()
        {
            _isUpdateRot = true;
            _delayUpdatingRotTime = K_Deplay_Update_Rot_Time;
            _stopUpdatingRotTime = K_Stop_Update_Rot_Time;
        }

        private bool DetectChange(Vector3 v1, Vector3 v2)
        {
            return (Math.Abs(v1.x - v2.x) > K_Possition_Update) || (Math.Abs(v1.y - v2.y) > K_Possition_Update) || (Math.Abs(v1.z - v2.z) > K_Possition_Update);
        }
    }
}
