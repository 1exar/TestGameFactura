using System;
using Cysharp.Threading.Tasks;
using TestGameFactura.Scripts.Entities.Player;
using TestGameFactura.Scripts.Pools;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Managers.GameManagers
{
    public class GameManager : IInitializable, IDisposable
    {
        private readonly PlayerController _player;
        private readonly EnemiesPool _enemiesPool;
        private bool _gameStarted = false;

        [Inject]
        public GameManager(PlayerController player, EnemiesPool enemiesPool)
        {
            _player = player;
            _enemiesPool = enemiesPool;

            enemiesPool.OnObjectReleased += CheckLiveEnemies;
        }

        private void CheckLiveEnemies()
        {
            Debug.LogError(_enemiesPool.CurrentSize);
            if (_enemiesPool.CurrentSize == 0)
            {
                OnWin();
            }
        }

        public void Initialize()
        {
             _player.OnDeath += OnLose;

            WaitForStart().Forget();
        }

        private async UniTaskVoid WaitForStart()
        {
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));

            StartGame();
        }

        private void StartGame()
        {
            _gameStarted = true;
            _player.StartMoving();
        }

        private void OnLose()
        {
            _gameStarted = false;
            _player.StopMoving();
            Debug.LogError("Player lose");
        }

        private void OnWin()
        {
            _player.StopMoving();
            Debug.LogError("Player win");
        }

        public void Dispose()
        {
            _enemiesPool.OnObjectReleased -= CheckLiveEnemies;
            _player.OnDeath -= OnLose;
        }
    }
}