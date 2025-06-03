using System;
using Cysharp.Threading.Tasks;
using TestGameFactura.Scripts.Entities.Player;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Managers.GameManagers
{
    public class GameManager : IInitializable, IDisposable
    {
        private readonly PlayerController _player;

        private bool _gameStarted = false;

        [Inject]
        public GameManager(PlayerController player)
        {
            _player = player;
        }

        public void Initialize()
        {
           // _player.OnDeath += OnLose;

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

        }
        

        public void Dispose()
        {
           // _player.OnDeath -= OnLose;
        }
    }
}