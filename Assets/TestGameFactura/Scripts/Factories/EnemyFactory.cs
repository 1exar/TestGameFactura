using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Factories
{
    public class EnemyFactory
    {
        private readonly DiContainer _container;
        private readonly GameObject _enemyPrefab;

        public EnemyFactory(DiContainer container, GameObject enemyPrefab)
        {
            _container = container;
            _enemyPrefab = enemyPrefab;
        }

        public void Create(Vector3 position, int hp)
        {
            var enemyGO = _container.InstantiatePrefab(_enemyPrefab, position, _enemyPrefab.transform.rotation, null);
            //var enemy = enemyGO.GetComponent<EnemyController>();
           // enemy.Init(hp);
        }
    }

}