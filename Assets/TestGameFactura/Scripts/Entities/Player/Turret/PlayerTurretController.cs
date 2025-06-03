using TestGameFactura.Scripts.Configs.Player;
using TestGameFactura.Scripts.Factories;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Entities.Player.Turret
{
    public class PlayerTurretController : MonoBehaviour
    {
        [SerializeField] private Transform barrelPivot;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Transform bulletSpawnPoint;

        [SerializeField] private LineRenderer aimLine;
        
        private PlayerTurretConfig _config;
        private float _currentYaw;
        private float _lastFireTime;

        private BulletFactory _bulletFactory;
        
        [Inject]
        public void Construct(PlayerTurretConfig config, BulletFactory bulletFactory)
        {
            _config = config;
            _bulletFactory = bulletFactory;
        }

        private void Update()
        {
            aimLine.SetPosition(0, bulletSpawnPoint.position);
            aimLine.SetPosition(1, firePoint.position);
            
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
            _bulletFactory.Create(bulletSpawnPoint.position,firePoint.position);
        }

        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(bulletSpawnPoint.position, firePoint.position);
        }
        #endif
    }
}