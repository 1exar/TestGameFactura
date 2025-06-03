using System;
using Cysharp.Threading.Tasks;
using TestGameFactura.Scripts.Entities.Enemy;
using TestGameFactura.Scripts.Entities.Interfaces.Health;
using TestGameFactura.Scripts.Entities.Player;
using TestGameFactura.Scripts.Managers.UIManager;
using TestGameFactura.Scripts.Pools;
using TestGameFactura.Scripts.Pools.Enemy;
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
        private readonly CameraManager.CameraManager _cameraManager;
        private readonly IHealth _playerHealth;


        private bool _isGameRun;
        
        [Inject]
        public GameManager(PlayerController player, 
            EnemiesPool enemiesPool, 
            IUIManager uiManager, 
            LevelManager.LevelManager levelManager, 
            IHealth playerHealth,
            CameraManager.CameraManager cameraManager)
        {
            _player = player;
            
            _enemiesPool = enemiesPool;

            _uiManager = uiManager;

            _levelManager = levelManager;
            
            
            _playerHealth = playerHealth;
            
            _cameraManager = cameraManager;
            
            _uiManager.OnClickRestart += RestartGame;
            enemiesPool.OnObjectReleased += CheckLiveEnemies;
        }
        
        private void CheckLiveEnemies()
        {
            if (_enemiesPool.CurrentSize == 0)
            {
                if (_playerHealth.Health > 0)
                {
                    OnWin();
                }
                else
                {
                    OnLose();
                }
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
            await UniTask.WaitForSeconds(3);
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
            _cameraManager.AnimateCamera();
            StartGame();
        }

        private void StartGame()
        {
            _isGameRun = true;
            _player.StartMoving();
        }

        private async void OnLose()
        {
            if(_isGameRun == false) return;
            _isGameRun = false;
            _player.StopMoving();
            await UniTask.WaitForSeconds(1);
            _uiManager.ShowEndGamePanel(false);
        }

        private void OnWin()
        {
            if(_isGameRun == false) return;
            _isGameRun = false;
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