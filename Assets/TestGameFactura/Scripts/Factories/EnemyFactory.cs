using TestGameFactura.Scripts.Configs.Enemy;
using TestGameFactura.Scripts.Entities.Enemy;
using TestGameFactura.Scripts.Entities.Player;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Factories
{
    public class EnemyFactory
    {
        private readonly DiContainer _container;
        private readonly GameObject _enemyPrefab;
        private readonly PlayerController _player;
        private readonly EnemyConfig _enemyConfig;

        public EnemyFactory(DiContainer container, GameObject enemyPrefab, PlayerController player, EnemyConfig config)
        {
            _container = container;
            _enemyPrefab = enemyPrefab;
            _player = player;
            _enemyConfig = config;
        }

        public void Create(Vector3 position)
        {
            var enemy = _container.InstantiatePrefab(_enemyPrefab, position, _enemyPrefab.transform.rotation, null)
                .GetComponent<EnemyController>();
            enemy.Init(_player.transform, _enemyConfig);
        }
    }
}