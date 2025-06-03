using TestGameFactura.Scripts.Configs.Enemy;
using TestGameFactura.Scripts.Entities.Interfaces.Health;
using TestGameFactura.Scripts.UI.Slider;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace TestGameFactura.Scripts.Entities.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour, IHealth
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private CustomSlider healthSlider;
        [Inject] private IHealth _playerHealth;
        
        private Transform _target;
        private EnemyConfig _config;

        private int _damage;
        
        private int _currentHp;

        private float _lastAttackTime;

        public void Init(Transform playerTransform, EnemyConfig config)
        {
            _target = playerTransform;
            _config = config;
            _currentHp = config.MaxHealth;
            _damage = config.Damage;
            
            agent.speed = _config.MoveSpeed;
            
            agent.stoppingDistance = _config.StoppingDinstance;

            healthSlider.Init(_currentHp);
            healthSlider.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_target == null || _config == null || agent == null) return;

            float distanceToPlayer = Vector3.Distance(transform.position, _target.position - Vector3.forward);
            if (distanceToPlayer > _config.AggroDistance)
                return;

            if (agent.remainingDistance >= agent.stoppingDistance)
            {
                if (distanceToPlayer < 1f)
                {
                    _playerHealth.TakeDamage(_damage);
                    Die();
                }
                else
                {
                    agent.SetDestination(_target.position - Vector3.forward);
                }
            }
            else
            {
                agent.SetDestination(_target.position - Vector3.forward);
            }
        }
        

        public void TakeDamage(int dmg)
        {
            healthSlider.gameObject.SetActive(true);
            _currentHp -= dmg;
            healthSlider.SetValue(_currentHp);
            if (_currentHp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}