using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ARGame2
{
    public enum EElementType
    {
        Metal,
        Wood,
        Water,
        Fire,
        Earth,
    }

    public enum EGenesisType
    {
        bulby,
        crocub,
        shepra,
        spikuff,
        sylvine,
        verfawn,
        taily,
        mossy,
        hercub,
        bregon,
        Seafa,
        Rumbu,
        Shardon,
        Zetooth,
        Celuna,
        Drizzy,
        Luea,
        Marvi,
        Misty,
        Nympha,
        Sylvi
    }

    public class ARGenesisBall : MonoBehaviour
    {
        public EGenesisType eGenesisType;
        //tạm thời
        public string number;
        public string nameGenesis;
        public string fesValue;
        public EElementType eElementType;
        public string id;
        public int energy = 30;

        [Range(0, 1000)] public int hp;
        [Range(0, 100)] public int hpregen;
        [Range(0, 100)] public int attack;
        [Range(0, 200)] public int critAttack;
        [Range(0, 100)] public int speed;
        [Range(0, 100)] public int defense;

        [SerializeField] private Transform _model;
        [SerializeField] private ARGenesisCanvas _aRGenesisCanvas;
        [SerializeField] private bool _isARObject;

        [Header("Apear Effect")]
        [SerializeField] private GameObject _fireEffect;
        [SerializeField] private GameObject _waterEffect;
        [SerializeField] private GameObject _earthEffect;
        [SerializeField] private GameObject _metalEffect;
        [SerializeField] private GameObject _woodEffect;

        private bool _isDisPlayEffect;
        private GameObject _effectObject;

        private MeshRenderer _modelMeshRenderer;
        private bool _isActive = true;
        private float _currentValue;
        private Coroutine _effectDissolveRoutine;

        public UnityEvent OnGenesisEatEnergy;




        private void OnEnable()
        {
            _model.localRotation = Quaternion.Euler(0, 180, 0);
            _aRGenesisCanvas?.AttachGenesis(this);
        }

        private void OnDisable()
        {
            Destroy(_effectObject);
            _isDisPlayEffect = false;
            _modelMeshRenderer.material.SetFloat("_CutoffHeight", -1);
        }

        private void Awake()
        {
            id = eElementType.ToString() + number + nameGenesis;
            _modelMeshRenderer = _model.GetComponentInChildren<MeshRenderer>();
            if (_isARObject)
            {
                _modelMeshRenderer.material.SetFloat("_CutoffHeight", -1.1f);
            }
            else
            {
                _modelMeshRenderer.material.SetFloat("_CutoffHeight", 100f);
            }
        }

        public void ShowInfo(bool isShow)
        {
            _aRGenesisCanvas?.gameObject.SetActive(isShow);
        }

        public void EatEnergy()
        {
            energy += 10;
            OnGenesisEatEnergy?.Invoke();
        }

        public void ActiveWithEffect()
        {
            if (_isActive) return;
            _isActive = true;

            StopEffectRoutine();

            gameObject.SetActive(true);
            _currentValue = -1.1f;
            //_effectDissolveRoutine = StartCoroutine(IEActiveWithEffect(-1.1f, 1.5f, 1.5f, () =>
            //{
            //    // gameObject.SetActive(true);
            //}));

            TurnOnAppearEffect(eElementType);
            StartCoroutine(DisplayARGBall());
        }

        private IEnumerator DisplayARGBall()
        {

            //  metal 1
            float timeToDisplay = 2f;
            switch (eElementType)
            {
                case EElementType.Fire:
                    timeToDisplay = 1.5f;
                    break;
                case EElementType.Water:
                    timeToDisplay = 2f;
                    break;
                case EElementType.Earth:
                    timeToDisplay = 2.8f;
                    break;
                case EElementType.Metal:
                    timeToDisplay = 1.5f;
                    break;
                case EElementType.Wood:
                    timeToDisplay = 0.45f;
                    break;
                default:

                    break;
            }

            yield return new WaitForSeconds(timeToDisplay);
            _currentValue = -1.1f;
            _effectDissolveRoutine = StartCoroutine(IEActiveWithEffect(-1.1f, 1.5f, 1.5f, () =>
            {
                // gameObject.SetActive(true);
            }));
        }
        public void DeActiveWithEffect()
        {
            if (_isActive == false) return;
            _isActive = false;
            StopEffectRoutine();

            gameObject.SetActive(false);

            // _effectDissolveRoutine = StartCoroutine(IEActiveWithEffect(1.5f, -1.1f, 1.5f, () =>
            // {
            //     gameObject.SetActive(false);
            // }));
        }

        private IEnumerator IEActiveWithEffect(float startValue, float endValue, float duration, Action OnFinish)
        {
            float elapsedTime = 0f;
            float initialValue = _currentValue; // Lưu giá trị hiện tại

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                _currentValue = Mathf.Lerp(initialValue, endValue, t); // Lerp từ giá trị hiện tại (_currentValue) tới endValue
                _modelMeshRenderer.material.SetFloat("_CutoffHeight", _currentValue);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _currentValue = endValue; // Đảm bảo giá trị cuối cùng là endValue
            _modelMeshRenderer.material.SetFloat("_CutoffHeight", _currentValue);

            OnFinish?.Invoke();
        }

        private void StopEffectRoutine()
        {
            if (_effectDissolveRoutine != null)
            {
                StopCoroutine(_effectDissolveRoutine);
            }
        }


        private void TurnOnAppearEffect(EElementType _effectType)
        {
            if (_isDisPlayEffect) return;

            _isDisPlayEffect = true;

            if (_effectType == EElementType.Wood)
            {
                _effectObject = Instantiate(_woodEffect, _model.GetComponentInChildren<MeshRenderer>().GetComponent<Transform>());

                _effectObject.transform.rotation = Quaternion.identity;
            }


            if (_effectType == EElementType.Metal)
            {
                _effectObject = Instantiate(_metalEffect, _model.GetComponentInChildren<MeshRenderer>().GetComponent<Transform>());

                _effectObject.transform.rotation = Quaternion.identity;
            }


            if (_effectType == EElementType.Fire)
            {
                _effectObject = Instantiate(_fireEffect, this.transform);

                _effectObject.transform.rotation = Quaternion.identity;

            }
            if (_effectType == EElementType.Earth)
            {
                _effectObject = Instantiate(_earthEffect, _model.GetComponentInChildren<MeshRenderer>().GetComponent<Transform>());

                _effectObject.transform.rotation = Quaternion.identity;

            }
            if (_effectType == EElementType.Water)
            {
                _effectObject = Instantiate(_waterEffect, _model.GetComponentInChildren<MeshRenderer>().GetComponent<Transform>());

                _effectObject.transform.rotation = Quaternion.identity;

            }


            if (_model && _effectObject)
                _effectObject.transform.rotation = _model.transform.rotation;
        }
    }
}
