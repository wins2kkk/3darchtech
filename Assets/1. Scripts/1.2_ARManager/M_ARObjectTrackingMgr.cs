using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ARGame2
{
    public class M_ARObjectTrackingMgr : MonoBehaviour
    {
        [SerializeField] private ARSession _aRSession;
        [SerializeField] private ARTrackedImageManager aRTrackedImageManager;
        [SerializeField] private List<ARObjectData> _listARObject;
        [SerializeField] private Transform _camera;

        private Dictionary<string, TrackedVirtualObject> _dictTrackedObject;

        private bool _isFix = false; //giữ AR Object cố định trong 3D World, không update theo image tracking
        private bool _isPause; //dừng cập nhập hình ảnh
        private bool _isShowInfo;

        private TrackedVirtualObject _currentObjectTracked;

        public void SetCurrentObjectTracking(TrackedVirtualObject trackedVirtualObject)
        {
            _currentObjectTracked = trackedVirtualObject;
        }

        private void Awake() 
        {
            _dictTrackedObject = new Dictionary<string, TrackedVirtualObject>();
        }

        private void OnEnable() 
        {
            aRTrackedImageManager.trackedImagesChanged += OnImageChange;       
            UIFuncButton.ShowInfoARGenesis += ShowInfoAR;
            UIFuncButton.OnOpenInventory += PauseTracking;
            GenesisInventory.OnCloseInventory += ResumeTracking;
        }

        private void OnDisable() 
        {
            aRTrackedImageManager.trackedImagesChanged -= OnImageChange;
            UIFuncButton.ShowInfoARGenesis -= ShowInfoAR;
            UIFuncButton.OnOpenInventory -= PauseTracking;
            GenesisInventory.OnCloseInventory -= ResumeTracking;
        }

        private void OnImageChange(ARTrackedImagesChangedEventArgs args)
        {
            if(_isPause) return;

            foreach(var image in args.added)
            {
                CreateARObject(image);
            }   

            foreach(var image in args.updated)
            {
                // if(_isFix) return;
                string key = image.referenceImage.name;
                if(key == null) continue; // có những trường hợp chạy trên editor ảnh từ hư không
                if (_dictTrackedObject.ContainsKey(key))
                {
                    if(_currentObjectTracked == null 
                        || _currentObjectTracked._aRGenesisBall.eGenesisType == _dictTrackedObject[key]._aRGenesisBall.eGenesisType)
                    {
                        Debug.Log(key);
                        _dictTrackedObject[key].TryToUpdate(image);
                    }
                }
                else 
                {
                    CreateARObject(image);
                }
            }

            foreach(var image in args.removed)
            {
                Debug.Log(image.referenceImage.name + " remove");
            }
        }

        private void CreateARObject(ARTrackedImage aRTrackedImage)
        {
            Debug.Log("CREATE " + aRTrackedImage.referenceImage.name + "_" + _dictTrackedObject.Count);
            if(aRTrackedImage.referenceImage.name == null) return;
            ARGenesisBall newPrefab = Instantiate(GetGameObject(aRTrackedImage.referenceImage.name), Vector3.zero, Quaternion.identity);
            newPrefab.ShowInfo(_isShowInfo);
            var newTrackedVirtualObject = new TrackedVirtualObject();
            newTrackedVirtualObject.Add(aRTrackedImage, newPrefab.gameObject, newPrefab, this);
            _dictTrackedObject.Add(aRTrackedImage.referenceImage.name, newTrackedVirtualObject);
        }

        public void FixedVirtualObject(int value)
        {
            _isFix = value == 0 ? false : true;

            if(_isFix == false)
            {
                DeActiveAll();
            }
        }

        public void ShowInfoAR(bool isShow)
        {
            _isShowInfo = isShow;

            foreach(var ar in _dictTrackedObject)
            {
                ar.Value._aRGenesisBall.ShowInfo(isShow);
            }
        }

        public void PauseTracking()
        {
            _isPause = true;

            DeActiveAll();
        }

        public void ResumeTracking()
        {
            _isPause = false;
        }

        private void DeActiveAll()
        {
            foreach(var ob in _dictTrackedObject.Values)
            {
                ob.Deactivate();
            }
        }

        public ARGenesisBall GetGameObject(string imageName)
        {
            foreach(var dt in _listARObject)
            {
                if(dt.imageName == imageName)
                {
                    return dt.aRGenesisBall;
                } 
            }

            return null;
        }
    }

    [Serializable]
    public class ARObjectData
    {
        public string imageName;
        public ARGenesisBall aRGenesisBall;
    }
}

