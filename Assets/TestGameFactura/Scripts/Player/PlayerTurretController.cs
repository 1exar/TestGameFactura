using TestGameFactura.Scripts.Configs.Player;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Player
{
    public class PlayerTurretController : MonoBehaviour
    {
        [SerializeField] private Transform barrelPivot;
        [SerializeField] private Transform firePoint;

        private PlayerTurretConfig _config;
        private float _currentYaw;
        private float _lastFireTime;

        [Inject]
        public void Construct(PlayerTurretConfig config)
        {
            _config = config;
        }

        private void Update()
        {
            HandleRotation();
            HandleShooting();
        }

        private void HandleRotation()
        {
            if (Input.GetMouseButton(0))
            {
                float mouseX = Input.GetAxis("Mouse X");
                _currentYaw += mouseX * _config.RotationSpeed * Time.deltaTime;
                _currentYaw = Mathf.Clamp(_currentYaw, -_config.YawLimit, _config.YawLimit);

                barrelPivot.localRotation = Quaternion.Euler(-90, _currentYaw, 180);
            }
        }

        private void HandleShooting()
        {
            if (Input.GetMouseButton(0) && Time.time >= _lastFireTime + _config.FireCooldown)
            {
                Shoot();
                _lastFireTime = Time.time;
            }
        }

        private void Shoot()
        {
            Instantiate(_config.BulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}