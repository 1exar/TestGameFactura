using TestGameFactura.Scripts.Configs.Player;
using TestGameFactura.Scripts.Entities.Player.Turret;
using TestGameFactura.Scripts.Pools.Bullet;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Factories
{
    public class BulletFactory
    {
        private readonly PlayerTurretConfig _config;
        
        private readonly BulletPool _bulletPool;

        public BulletFactory(PlayerTurretConfig config, BulletPool bulletPool)
        {
            _config = config;
            _bulletPool = bulletPool;
        }

        public void Create(Vector3 spawnPosition, Vector3 direction)
        {
            var bullet = _bulletPool.Get();
            bullet.transform.position = spawnPosition;
            bullet.Init(direction, _config, _bulletPool);
        }
    }
}