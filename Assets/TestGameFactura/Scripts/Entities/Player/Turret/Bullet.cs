using TestGameFactura.Scripts.Configs.Player;
using TestGameFactura.Scripts.Entities.Interfaces.Health;
using UnityEngine;

namespace TestGameFactura.Scripts.Entities.Player.Turret
{
    public class Bullet : MonoBehaviour
    {
        private int _speed;
        private int _damage;
        private Vector3 _direction;

        public void Init(Vector3 direction, PlayerTurretConfig config)
        {
            _direction = transform.position - direction;
            _damage = config.Damage;    
            _speed = config.BulletSpeed;
            Destroy(gameObject, config.BulletLifeTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent(out IHealth health))
            {
                health.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            transform.position -= _direction * _speed * Time.deltaTime;
        }
    }
}