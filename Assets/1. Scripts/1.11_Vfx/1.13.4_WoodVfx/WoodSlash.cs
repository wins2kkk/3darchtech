using UnityEngine;
using TEGOF.Logic;

namespace TEGOF.Core
{
    public class WoodSlash : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _slashVfx;
        [SerializeField] private GameObject _impactVfx;

        [Header("Settings")]
        [SerializeField] private float _damage = 50f;
        [SerializeField] private float _speed = 15f;
        [SerializeField] private float _lifeTime = 2f;
        [SerializeField] private float _hitRadius = 1.2f;

        private bool _hasHit = false;

        private void Start()
        {
            if (_slashVfx != null)
                _slashVfx.SetActive(true);

            Destroy(gameObject, _lifeTime);
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasHit) return;

            if (other.CompareTag("Enemy"))
            {
                _hasHit = true;

                // Gây damage
                var health = other.GetComponent<Health>();
                if (health != null)
                    health.TakeDamage((int)_damage);

                // Gọi hiệu ứng trúng
                if (_impactVfx != null)
                    Instantiate(_impactVfx, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }
}
