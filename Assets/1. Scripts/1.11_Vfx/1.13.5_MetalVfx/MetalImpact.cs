using UnityEngine;
using TEGOF.Logic;

namespace TEGOF.Core
{
    public class MetalImpact : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _lightningVfx; // tia sét chính

        [Header("Timing")]
        [SerializeField] private float _delayBeforeImpact = 0.5f; // thời gian trễ trước khi sét đánh
        [SerializeField] private float _lifeTime = 3f;            // thời gian tồn tại

        [Header("Damage Settings")]
        [SerializeField] private float _damageRadius = 2f;        // phạm vi gây damage
        [SerializeField] private float _damage = 60f;             // lượng sát thương

        private Transform _target;
        private bool _hasDealtDamage = false;

        private void Start()
        {
            if (_lightningVfx != null)
                _lightningVfx.SetActive(false); // ẩn tia sét trước

            Invoke(nameof(TriggerImpact), _delayBeforeImpact);
            Destroy(gameObject, _lifeTime);
        }

        private void TriggerImpact()
        {
            if (_lightningVfx != null)
                _lightningVfx.SetActive(true); // hiển thị tia sét

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
                    }
                }
            }
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _damageRadius);
        }
    }
}
