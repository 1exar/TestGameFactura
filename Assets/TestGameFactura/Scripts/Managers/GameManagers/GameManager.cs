using System;
using Cysharp.Threading.Tasks;
using TestGameFactura.Scripts.Entities.Enemy;
using TestGameFactura.Scripts.Entities.Interfaces.Health;
using TestGameFactura.Scripts.Entities.Player;
using TestGameFactura.Scripts.Managers.UIManager;
using TestGameFactura.Scripts.Pools;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Managers.GameManagers
{
    public class GameManager : IInitializable, IDisposable
    {
        private readonly PlayerController _player;
        private readonly EnemiesPool _enemiesPool;
        private readonly IUIManager _uiManager;
        private readonly LevelManager.LevelManager _levelManager;
        private readonly IHealth _playerHealth;
        
        [Inject]
        public GameManager(PlayerController player, EnemiesPool enemiesPool, IUIManager uiManager, LevelManager.LevelManager levelManager, IHealth playerHealth)
        {
            _player = player;
            _enemiesPool = enemiesPool;

            _uiManager = uiManager;
            _uiManager.OnClickRestart += RestartGame;
            
            _levelManager = levelManager;
            
            enemiesPool.OnObjectReleased += CheckLiveEnemies;
            
            _playerHealth = playerHealth;
        }
        
        private void CheckLiveEnemies()
        {
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

        private async void RestartGame()
        {
            _uiManager.ShowTransition(false);
            await UniTask.Delay(TimeSpan.FromSeconds(3));
            _player.MoveToStartPosition();
            _levelManager.InitializeAsync();

            EnemyController[] temp = new EnemyController[_enemiesPool.ActiveBehaviours.Count];
            
            _enemiesPool.ActiveBehaviours.CopyTo(temp);

            foreach (var activeEnemie in temp)
            {
                _enemiesPool.Release(activeEnemie, true);
            }
            
            _playerHealth.Restore();
            
            WaitForStart().Forget();
        }
        
        private async UniTaskVoid WaitForStart()
        {
            await UniTask.WaitUntil(() => Input.GetMouseButton(0));

            StartGame();
        }

        private void StartGame()
        {
            _player.StartMoving();
        }

        private void OnLose()
        {
            _player.StopMoving();
            _uiManager.ShowEndGamePanel(false);
        }

        private void OnWin()
        {
            _player.StopMoving();
            _uiManager.ShowEndGamePanel(true);
        }

        public void Dispose()
        {
            _uiManager.OnClickRestart -= RestartGame;
            _enemiesPool.OnObjectReleased -= CheckLiveEnemies;
            _player.OnDeath -= OnLose;
        }
    }
}