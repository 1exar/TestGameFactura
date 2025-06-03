using System;
using System.Threading.Tasks;
using DG.Tweening;
using TestGameFactura.Scripts.Configs.Game;
using TestGameFactura.Scripts.Entities.Interfaces.Health;
using TestGameFactura.Scripts.Entities.Player.Turret;
using TestGameFactura.Scripts.Managers.SoundManager;
using TestGameFactura.Scripts.UI.Slider;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Entities.Player
{
    public class PlayerController : MonoBehaviour, IHealth
    {
        [SerializeField] private CustomSlider healthSlider;
        [SerializeField] private PlayerTurretController playerTurret;

        [Header("Animation")] 
        [SerializeField] private float duration = 1.6f;
        [SerializeField] private float cren = .25f;
        [SerializeField] private float rotationCren = 6f;
        
        public Action OnDeath;
        
        private float _moveSpeed;
        private bool _isMoving;
        private int _health;
        private int _maxHealth;

        private ISoundManager _soundManager;
        
        private Vector3 _startPosition;
        private Tween _sideMoveTween;
        private Tween _tiltTween;
        
        public int Health => _health;
        
        [Inject]
        public void Construct(GameConfig config, ISoundManager soundManager)
        {
            _moveSpeed = config.PlayerSpeed;
            _health = config.PlayerMaxHealth;
            _maxHealth = config.PlayerMaxHealth;
            
            healthSlider.Init(config.PlayerMaxHealth);
            _soundManager = soundManager;
        }

        private void Start()
        {
            _startPosition = transform.position;
        }

        public void MoveToStartPosition() => transform.position = _startPosition;

        public void StartMoving()
        {
            _isMoving = true;

            playerTurret.SetShootAvaiblity(true);
            
            Sequence motionSequence = DOTween.Sequence();

            motionSequence.Append(
                transform.DOLocalMoveX(_startPosition.x + cren, duration).SetEase(Ease.InOutSine)
            );
            motionSequence.Join(
                transform.DORotate(transform.rotation.eulerAngles + Vector3.up * rotationCren, duration).SetEase(Ease.InOutSine)
            );

            motionSequence.Append(
                transform.DOLocalMoveX(_startPosition.x, duration).SetEase(Ease.InOutSine)
            );
            motionSequence.Join(
                transform.DORotate(transform.rotation.eulerAngles, duration).SetEase(Ease.InOutSine)
            );

            motionSequence.Append(
                transform.DOLocalMoveX(_startPosition.x - cren, duration).SetEase(Ease.InOutSine)
            );
            motionSequence.Join(
                transform.DORotate(transform.rotation.eulerAngles + Vector3.up * -rotationCren, duration).SetEase(Ease.InOutSine)
            );

            motionSequence.Append(
                transform.DOLocalMoveX(_startPosition.x, duration).SetEase(Ease.InOutSine)
            );
            motionSequence.Join(
                transform.DORotate(transform.rotation.eulerAngles, duration).SetEase(Ease.InOutSine)
            );

            motionSequence.SetLoops(-1, LoopType.Restart);

            _tiltTween = motionSequence;
        }

        public void StopMoving()
        {
            _isMoving = false;
            
            playerTurret.SetShootAvaiblity(false);
            
            _sideMoveTween?.Kill();
            _tiltTween?.Kill();
            
            transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
        }

        private void Update()
        {
            if (_isMoving)
            {
                transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime, Space.World);
            }
        }

        public async Task TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                OnDeath?.Invoke();
                healthSlider.SetValue(0);
            }
            else
            {
                healthSlider.SetValue(_health);
            }
            
            Sequence damageTween = DOTween.Sequence();
            damageTween.Append(transform.DOScale(new Vector3(0.99f,1f,0.99f), .1f));
            damageTween.Append(transform.DOScale(Vector3.one, .1f));

            damageTween.Play();
            
            _soundManager.PlayCarDamageSound();
        }

        public void Restore()
        {
            _health = _maxHealth;
            healthSlider.SetValue(_health);
        }

    }
}
