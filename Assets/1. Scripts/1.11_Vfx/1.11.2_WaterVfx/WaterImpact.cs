using UnityEngine;
using TEGOF.Logic;

namespace TEGOF.Core
{
    public class WaterImpact : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _circleVfx;
        [SerializeField] private GameObject _waterColumnVfx;
        [SerializeField] private GameObject _splashVfx;

        [Header("Timing")]
        [SerializeField] private float _delayBeforeImpact = 0.7f;
        [SerializeField] private float _lifeTime = 3f;

        [Header("Damage Settings")]
        [SerializeField] private float _damageRadius = 1.5f;
        [SerializeField] private float _damage = 50f;

        private Transform _target; 
        private bool _hasDealtDamage = false;

        private void Start()
        {
            if (_circleVfx != null)
                _circleVfx.SetActive(true);

            if (_waterColumnVfx != null)
                _waterColumnVfx.SetActive(false);

            Invoke(nameof(TriggerImpact), _delayBeforeImpact);
            Destroy(gameObject, _lifeTime);
        }


        private void TriggerImpact()
        {
            if (_waterColumnVfx != null)
                _waterColumnVfx.SetActive(true);

            DealDamage();
        }

        private void DealDamage()
        {
            if (_hasDealtDamage) return;
            _hasDealtDamage = true;

            Collider[] hits = Physics.OverlapSphere(transform.position, _damageRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    var health = hit.GetComponent<Health>();
                    if (health != null)
                    {
                        health.TakeDamage((int)_damage);

                        if (_splashVfx != null)
                            Instantiate(_splashVfx, hit.transform.position, Quaternion.identity);
                    }
                }
            }
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
    }
}
