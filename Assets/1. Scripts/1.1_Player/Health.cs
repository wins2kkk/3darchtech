using UnityEngine;
using TEGOF.UI;
using TEGOF.Core; // 👈 thêm dòng này để dùng CharacterAnimator

namespace TEGOF.Logic
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _maxHp = 100f;
        private float _currentHp;

        private UIBattleHUD _hud;
        [SerializeField] private bool _isPlayer;

        [Header("Animation Reference")]
        [SerializeField] private CharacterAnimator _animator; // thêm dòng này

        public GameObject _destroyTarget; //detroy
        private float _lifeTime = 2f;

        private void Awake()
        {
            _currentHp = _maxHp;
        }

        private void Start()
        {
            _hud = FindObjectOfType<UIBattleHUD>();
            UpdateHud();
        }

        public void TakeDamage(int amount)
        {
            _currentHp -= amount;
            if (_currentHp < 0) _currentHp = 0;

            UpdateHud();//animation Hit
            if (_isPlayer && _animator != null)
            {
                _animator.PlayHit();
            }
            if (_currentHp <= 0 )
            {
                Destroy(_destroyTarget, _lifeTime);
            }
        }

        private void UpdateHud()
        {
            if (_hud == null) return;

            float ratio = _currentHp / _maxHp;
            if (_isPlayer)
                _hud.UpdatePlayerHp(ratio);
            else
                _hud.UpdateEnemyHp(ratio);
        }
    }
}
