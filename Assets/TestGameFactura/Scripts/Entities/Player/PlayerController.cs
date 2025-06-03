using TestGameFactura.Scripts.Configs.Game;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Entities.Player
{
    public class PlayerController : MonoBehaviour
    {
        private float _moveSpeed;
        private bool _isMoving;

        [Inject]
        public void Construct(GameConfig config)
        {
            _moveSpeed = config.PlayerSpeed;
        }
        
        public void StartMoving()
        {
            _isMoving = true;
        }

        public void StopMoving()
        {
            _isMoving = false;
        }

        private void Update()
        {
            if (_isMoving)
            {
                transform.Translate(Vector3.back * _moveSpeed * Time.deltaTime);
            }

        }
    }
}
