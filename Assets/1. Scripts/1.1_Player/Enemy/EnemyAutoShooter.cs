using TEGOF.Core;
using TEGOF.Logic;
using UnityEngine;

namespace TEGOF.Logic
{
    public class EnemyAutoShooter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _firePoint;     // vị trí đầu nòng súng
        [SerializeField] private GameObject _bulletPrefab; // prefab viên đạn
        [SerializeField] private Transform _playerTarget;  // player cần bắn
        [SerializeField] private float _fireRate = 3f;     // thời gian bắn (3s/lần)

        private float _fireTimer;

        private void Update()
        {
            if (_playerTarget == null || _bulletPrefab == null || _firePoint == null)
                return;

            _fireTimer -= Time.deltaTime;
            if (_fireTimer <= 0f)
            {
                Shoot();
                _fireTimer = _fireRate;
            }
        }

        private void Shoot()
        {
            // Tạo viên đạn tại vị trí bắn
            GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);

            // Xoay về hướng Player
            Vector3 dir = (_playerTarget.position - _firePoint.position).normalized;
            bullet.transform.forward = dir;

            // Gọi hàm Launch để bay
            var bulletScript = bullet.GetComponent<EnemyBullet>();
            if (bulletScript != null)
            {
                bulletScript.Launch(dir);
            }
        }
    }
}
