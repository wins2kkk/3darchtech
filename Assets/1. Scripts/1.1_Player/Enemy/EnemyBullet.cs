using UnityEngine;
using TEGOF.Logic;

namespace TEGOF.Core
{
    public class EnemyBullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _damage = 20f;
        [SerializeField] private float _lifeTime = 5f;
        [SerializeField] private GameObject _impactVfx;

        private Vector3 _direction;
        private bool _launched = false;

        public void Launch(Vector3 direction)
        {
            _direction = direction.normalized;
            _launched = true;
            Destroy(gameObject, _lifeTime);
        }

        private void Update()
        {
            if (_launched)
                transform.position += _direction * _speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var health = other.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage((int)_damage);
                }

                if (_impactVfx != null)
                    Instantiate(_impactVfx, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }
}
