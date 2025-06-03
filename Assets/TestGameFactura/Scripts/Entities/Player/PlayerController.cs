using TestGameFactura.Scripts.Configs.Game;
using TestGameFactura.Scripts.Entities.Interfaces.Health;
using TestGameFactura.Scripts.UI.Slider;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Entities.Player
{
    public class PlayerController : MonoBehaviour, IHealth
    {
        
        [SerializeField] private CustomSlider healthSlider;
        private float _moveSpeed;
        private bool _isMoving;
        private int _health;

        [Inject]
        public void Construct(GameConfig config)
        {
            _moveSpeed = config.PlayerSpeed;
            _health = config.PlayerMaxHealth;
            
            healthSlider.Init(config.PlayerMaxHealth);
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

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Debug.LogError("Player Dead");
                StopMoving();
                healthSlider.SetValue(0);
            }
            else
            {
                healthSlider.SetValue(_health);
            }
        }
    }
}
