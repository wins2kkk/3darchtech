using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace ARGame2
{
    public class ARGenesisCanvas : MonoBehaviour
    {

        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private TextMeshProUGUI _textFes;
        [SerializeField] private TextMeshProUGUI _textID;
        [SerializeField] private TextMeshProUGUI _textGenesisName;

        [SerializeField] private EnergyBar _energyBar;
        
        [SerializeField] private FlexibleUIMgr _flexibleUIMgr;

        private ARGenesisBall _aRGenesisBall;
        private Transform _cameraTransform;

        private void Awake() 
        {
            _cameraTransform = Camera.main.transform;
        }

        private async void OnEnable() 
        {
            await Task.Delay(100);
            UpdateUI();   
        }

        private void LateUpdate() 
        {
            FollowCamera();
        }

        private void FollowCamera()
        {
            Vector3 direction = transform.position - _cameraTransform.position;

            // Tránh lỗi do vector direction bằng zero
            if (direction.sqrMagnitude > 0.01f) // Kiểm tra khoảng cách bình phương
            {
                // Quaternion lookCamera = Quaternion.LookRotation(direction, Vector3.up);
                // // lookCamera.y = transform.rotation.y;
                // transform.rotation = lookCamera;

                // Vector3 euler = transform.rotation.eulerAngles;
                // Vector3 euler3 = Camera.main.transform.rotation.eulerAngles;
                // Vector3 euler2 = new Vector3(euler.x, euler3.y, euler.z);
                // transform.rotation = Quaternion.Euler(euler2);

                transform.rotation = Camera.main.transform.rotation;

                // Quaternion lookCamera2 = Camera.main.transform.rotation;
                // lookCamera2.x = transform.localRotation.x;
                // lookCamera2.z = transform.localRotation.z;
                // transform.localRotation = lookCamera2;
            }
        }

        public void AttachGenesis(ARGenesisBall aRGenesisBall)
        {
            _aRGenesisBall = aRGenesisBall;
        }

        private void UpdateUI()
        {
            _textFes.text = $"{_aRGenesisBall.fesValue} FES";
            _textID.text = $"ID: {_aRGenesisBall.id}";
            _textGenesisName.text = _aRGenesisBall.nameGenesis;
            _energyBar.UpdateEnergyBarSmooth(_aRGenesisBall.energy);
            _flexibleUIMgr.UpdateAllFlexibleUI(_aRGenesisBall.eElementType);
        }

        public void ShowInfo(bool isShow)
        {
            _infoPanel.SetActive(isShow);
        }
    }
}