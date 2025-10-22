using UnityEngine;
using TEGOF.Logic;

namespace TEGOF.Core
{
    public class EarthVfx : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ParticleSystem _earthEffect; // Hiệu ứng đá trồi lên

        [Header("Timing")]
        [SerializeField] private float _delayBeforeImpact = 0.5f; // Thời gian chờ trước khi trồi lên
        [SerializeField] private float _damageDelay = 0.3f;       // Độ trễ trước khi gây damage
        [SerializeField] private float _lifeTime = 3f;            // Thời gian tồn tại trước khi bị hủy

        [Header("Damage Settings")]
        [SerializeField] private float _damageRadius = 2f;        // Bán kính gây damage
        [SerializeField] private float _damage = 50f;             // Lượng sát thương gây ra

        private bool _hasDealtDamage = false;

        private void Start()
        {
            if (_earthEffect != null)
                _earthEffect.Stop();

            // Chờ cho đá trồi lên rồi gây damage
            Invoke(nameof(TriggerImpact), _delayBeforeImpact);
            Invoke(nameof(DealDamage), _delayBeforeImpact + _damageDelay);

            Destroy(gameObject, _lifeTime);
        }

        private void TriggerImpact()
        {
            if (_earthEffect != null)
                _earthEffect.Play();
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

        private void OnDisable()
        {
            // Ngăn ParticleSystem phát sinh lỗi khi object bị hủy sớm
            if (_earthEffect != null && _earthEffect.isPlaying)
                _earthEffect.Stop();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.5f, 0.3f, 0.1f);
            Gizmos.DrawWireSphere(transform.position, _damageRadius);
        }
    }
}
