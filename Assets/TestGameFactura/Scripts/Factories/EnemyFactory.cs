using TestGameFactura.Scripts.Configs.Enemy;
using TestGameFactura.Scripts.Entities.Player;
using TestGameFactura.Scripts.Pools;
using UnityEngine;

namespace TestGameFactura.Scripts.Factories
{
    public class EnemyFactory
    {
        private readonly PlayerController _player;
        private readonly EnemyConfig _enemyConfig;
        private readonly EnemiesPool _enemiesPool;

        public EnemyFactory(PlayerController player, EnemyConfig config, EnemiesPool pool)
        {
            _player = player;
            _enemyConfig = config;
            _enemiesPool = pool;
            
            pool.ClearPool();
        }

        public void Create(Vector3 position)
        {
            var enemy = _enemiesPool.Get();
            
            enemy.transform.position = position;
            
            enemy.Init(_player.transform, _enemyConfig, _enemiesPool);
        }
    }
}