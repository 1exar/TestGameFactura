using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TestGameFactura.Scripts.Configs.Enemy;
using TestGameFactura.Scripts.Entities.Interfaces.Health;
using TestGameFactura.Scripts.Pools;
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
        [SerializeField] private Animator animator;
        [Inject] private IHealth _playerHealth;
        
        private Transform _target;
        private EnemyConfig _config;

        private int _damage;
        
        private int _currentHp;

        private float _lastAttackTime;
        
        private bool _isDead;
        
        private EnemiesPool _pool;

        public void Init(Transform playerTransform, EnemyConfig config, EnemiesPool pool)
        {
            _target = playerTransform;
            _config = config;
            _currentHp = config.MaxHealth;
            _damage = config.Damage;
            
            agent.speed = _config.MoveSpeed;
            
            agent.stoppingDistance = _config.StoppingDinstance;

            healthSlider.Init(_currentHp);
            healthSlider.gameObject.SetActive(false);
            
            _pool = pool;
        }

        private void Update()
        {
            if (_target == null || _config == null || agent == null || _isDead) return;

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
                    if(_currentHp > 0)
                        animator.SetTrigger("Run");
                    agent.SetDestination(_target.position - Vector3.forward);
                }
            }
            else
            {
                if(_currentHp > 0)
                    animator.SetTrigger("Run");
                agent.SetDestination(_target.position - Vector3.forward);
            }
        }
        

        public void TakeDamage(int dmg)
        {
            if(_isDead) return;
            healthSlider.gameObject.SetActive(true);
            _currentHp -= dmg;
            healthSlider.SetValue(_currentHp);
            if (_currentHp <= 0)
            {
                Die(true);
            }
        }

        private async Task Die(bool useAnimation = false)
        {
            _isDead = true;

            if (useAnimation)
            {
                animator.SetTrigger("Death");
                await UniTask.WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);

            }
            _pool.Release(this);
        }
    }
}