using UnityEngine;
using TEGOF.Logic;

namespace TEGOF.Core
{
    public class ProjectileFireball : MonoBehaviour
    {
        [SerializeField] private float _damage = 50f;
        [SerializeField] private float _speed = 15f;
        [SerializeField] private float _lifeTime = 5f;
        [SerializeField] private GameObject _impactVfx; //thêm VFX

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.useGravity = false;
        }

        private void Start()
        {
            Destroy(gameObject, _lifeTime);
        }

        public void Launch(Vector3 direction)
        {
            _rb.velocity = direction.normalized * _speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                // ✅ Gây damage
                var health = other.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage((int)_damage);
                }

                // ✅ Tạo hiệu ứng nổ tại vị trí va chạm
                if (_impactVfx != null)
                {
                    Instantiate(_impactVfx, transform.position, Quaternion.identity);
                }

                // ✅ Hủy đạn sau khi trúng
                Destroy(gameObject);
            }
        }

        // Cho phép truyền VFX từ SkillData
        public void SetImpactVfx(GameObject vfx)
        {
            _impactVfx = vfx;
        }
    }
}
