using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TestGameFactura.Scripts.Configs.Player;
using TestGameFactura.Scripts.Entities.Interfaces.Health;
using TestGameFactura.Scripts.Pools.Bullet;
using UnityEngine;

namespace TestGameFactura.Scripts.Entities.Player.Turret
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trailRenderer;
        
        private int _speed;
        private int _damage;
        private Vector3 _direction;
        private BulletPool _bulletPool;
        
        public void Init(Vector3 direction, PlayerTurretConfig config, BulletPool bulletPool)
        {
            _trailRenderer.Clear();
            
            _direction = transform.position - direction;
            _damage = config.Damage;    
            _speed = config.BulletSpeed;
            _bulletPool = bulletPool;
            
            WaitBeforeRelease(config.BulletLifeTime);
        }

        private async Task WaitBeforeRelease(float lifetime)
        {
            await UniTask.WaitForSeconds(lifetime);
            _bulletPool.Release(this);
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