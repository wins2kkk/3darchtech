using TEGOF.Core;
using UnityEngine;

namespace TEGOF.Logic
{
    public class OrbCombatController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private SkillData _skill;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private GameObject _target;

        private bool _isAttacking = false;

        private void Awake()
        {
            if (_firePoint == null)
                _firePoint = transform;
        }

        public void SetTarget(GameObject target) => _target = target;
        public void SetSkill(SkillData skill) => _skill = skill;

        public void OnAttackButtonPressed()
        {
            if (_isAttacking) return;
            _isAttacking = true;

            if (_animator != null)
                _animator.PlayAttack();
        }

        // Gọi từ Animation Event
        public void SpawnProjectile()
        {

            if (_skill == null)
            {
                return;
            }
            if (_skill.ProjectilePrefab == null)
            {
                return;
            }
            if (_target == null)
            {
                return;
            }

            //hệ Nước
            if (_skill.Id == "Water")
            {
                Vector3 spawnPos = _target.transform.position + Vector3.up * 0.1f;
                GameObject waterEffect = Instantiate(_skill.ProjectilePrefab, spawnPos, Quaternion.identity);

                var impact = waterEffect.GetComponent<WaterImpact>();
                if (impact != null)
                    impact.SetTarget(_target.transform);

                Invoke(nameof(EndAttack), 0.3f);
                return;
            }
            // Hệ Kim ⚡
            if (_skill.Id == "Metal")
            {
                Vector3 spawnPos = _target.transform.position + Vector3.up * 0.1f;
                GameObject metalEffect = Instantiate(_skill.ProjectilePrefab, spawnPos, Quaternion.identity);

                var impact = metalEffect.GetComponent<MetalImpact>();
                if (impact != null)
                    impact.SetTarget(_target.transform);

                Invoke(nameof(EndAttack), 0.3f);
                return;
            }
            // Hệ Thổ 🪨
            if (_skill.Id == "Earth")
            {
                Vector3 spawnPos = _target.transform.position;
                GameObject earthEffect = Instantiate(_skill.ProjectilePrefab, spawnPos, Quaternion.identity);

                var impact = earthEffect.GetComponent<EarthVfx>();
                if (impact != null)
                {
                    // EarthVfx tự xử lý hiệu ứng & damage
                }

                Invoke(nameof(EndAttack), 0.3f);
                return;
            }
            // Hệ Mộc 🌿
            if (_skill.Id == "Wood")
            {
                Quaternion spawnRotation = _firePoint.rotation;

                // 👉 Nếu effect nằm ngang theo trục X thì xoay thêm 90 độ quanh trục X
                spawnRotation *= Quaternion.Euler(90f, 0f, 0f);

                GameObject woodSlash = Instantiate(_skill.ProjectilePrefab, _firePoint.position, spawnRotation);

                var slash = woodSlash.GetComponent<WoodSlash>();
                if (slash != null)
                {
                    // Có thể set thêm target hoặc damage sau này
                }

                Invoke(nameof(EndAttack), 0.3f);
                return;
            }

            //hệ bay có Rigidbody (Fire, Earth, v.v.)
            GameObject projectile = Instantiate(_skill.ProjectilePrefab, _firePoint.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 dir = (_target.transform.position - _firePoint.position).normalized;
                rb.velocity = dir * _skill.Speed;
            }

            Invoke(nameof(EndAttack), 0.3f);
        }

        
        public void EndAttack() => _isAttacking = false;
    }
}
