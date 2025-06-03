using TestGameFactura.Scripts.Configs.Player;
using TestGameFactura.Scripts.Entities.Player.Turret;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Factories
{
    public class BulletFactory
    {
        private readonly GameObject _bulletPrefab;
        private readonly DiContainer _container;
        
        private readonly PlayerTurretConfig _config;

        public BulletFactory(DiContainer container, PlayerTurretConfig config)
        {
            _bulletPrefab = config.BulletPrefab;
            _container = container;
            _config = config;
        }

        public void Create(Vector3 spawnPosition, Vector3 direction)
        {
            var bullet = _container.InstantiatePrefab(_bulletPrefab, spawnPosition, _bulletPrefab.transform.rotation, null).GetComponent<Bullet>();
            bullet.Init(direction, _config);

        }
    }
}