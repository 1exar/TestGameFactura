using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TestGameFactura.Scripts.Configs.Enemy;
using TestGameFactura.Scripts.Entities.Interfaces.Health;
using TestGameFactura.Scripts.Managers.SoundManager;
using TestGameFactura.Scripts.Pools;
using TestGameFactura.Scripts.Pools.Enemy;
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
        [SerializeField] private Collider collider;
        [SerializeField] private SkinnedMeshRenderer renderer;
        [SerializeField] private Material damageMaterial;
        [SerializeField] private GameObject damageEffect;
        
        [Inject] private IHealth _playerHealth;
        [Inject] private ISoundManager _soundManager;
        
        public int Health => _currentHp;

        private Material _defaultMaterial;
        
        private Transform _target;
        
        private EnemyConfig _config;

        private int _damage;
        
        private int _currentHp;

        private float _lastAttackTime;
        
        private bool _isDead;
        
        private EnemiesPool _pool;

        private EnemyState _currentState;
        
        public void Init(Transform playerTransform, EnemyConfig config, EnemiesPool pool)
        {
            collider.enabled = true;
            _isDead = false;
            damageEffect.SetActive(false);

            _defaultMaterial = renderer.material;
            
            _target = playerTransform;
            _config = config;
            _currentHp = config.MaxHealth;
            _damage = config.Damage;
            
            agent.speed = _config.MoveSpeed;
            
            agent.stoppingDistance = _config.StoppingDinstance;

            healthSlider.Init(_currentHp);
            healthSlider.gameObject.SetActive(false);
            
            _pool = pool;
            
            SetState(EnemyState.Idle);
        }

        private void SetState(EnemyState newState)
        {
            if (_currentState == newState) return;

            _currentState = newState;

            switch (newState)
            {
                case EnemyState.Idle:
                    animator.SetTrigger("Idle");
                    break;
                case EnemyState.Run:
                    animator.SetTrigger("Run");
                    break;
                case EnemyState.Attack:
                    animator.SetTrigger("Attack");
                    break;
                case EnemyState.Death:
                    animator.SetTrigger("Death");
                    break;
            }
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
                        SetState(EnemyState.Run);
                    agent.SetDestination(_target.position - Vector3.forward);
                }
            }
            else
            {
                if(_currentHp > 0)
                    SetState(EnemyState.Run);
                agent.SetDestination(_target.position - Vector3.forward);
            }
        }
        

        public async void TakeDamage(int dmg)
        {
            if(_isDead) return;
            healthSlider.gameObject.SetActive(true);
            _currentHp -= dmg;
            if (_currentHp <= 0)
            {
                healthSlider.SetValue(0);
                Die(true);
            }
            else
            {
                healthSlider.SetValue(_currentHp);
            }
            _soundManager.PlayHitSound();

            renderer.material = damageMaterial;
            await UniTask.WaitForSeconds(.15f);
            renderer.material = _defaultMaterial;
        }

        public void Restore()
        {
        }
        
        private async void Die(bool useAnimation = false)
        {
            agent.isStopped = true;
            _isDead = true;

            collider.enabled = false;
            
            if (useAnimation)
            {
                SetState(EnemyState.Death);         
                damageEffect.SetActive(true);
                await UniTask.WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length + 1);
            }
            
            _pool.Release(this);
        }
    }
}