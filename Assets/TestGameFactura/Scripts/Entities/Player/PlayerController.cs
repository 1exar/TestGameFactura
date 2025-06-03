using System;
using System.Threading.Tasks;
using DG.Tweening;
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
        
        public Action OnDeath;
        
        private float _moveSpeed;
        private bool _isMoving;
        private int _health;
        private int _maxHealth;

        private Vector3 _startPosition;
        private Tween _sideMoveTween;
        private Tween _tiltTween;

        [Inject]
        public void Construct(GameConfig config)
        {
            _moveSpeed = config.PlayerSpeed;
            _health = config.PlayerMaxHealth;
            _maxHealth = config.PlayerMaxHealth;
            
            healthSlider.Init(config.PlayerMaxHealth);
        }

        private void Start()
        {
            _startPosition = transform.position;
        }

        public void MoveToStartPosition() => transform.position = _startPosition;

        public void StartMoving()
        {
            _isMoving = true;

            Sequence motionSequence = DOTween.Sequence();

            motionSequence.Append(
                transform.DOLocalMoveX(_startPosition.x + 0.25f, 2.2f).SetEase(Ease.InOutSine)
            );
            motionSequence.Join(
                transform.DORotate(transform.rotation.eulerAngles + Vector3.up * 6f, 2.2f).SetEase(Ease.InOutSine)
            );

            motionSequence.Append(
                transform.DOLocalMoveX(_startPosition.x, 1.6f).SetEase(Ease.InOutSine)
            );
            motionSequence.Join(
                transform.DORotate(transform.rotation.eulerAngles, 1.6f).SetEase(Ease.InOutSine)
            );

            motionSequence.Append(
                transform.DOLocalMoveX(_startPosition.x - 0.22f, 2.6f).SetEase(Ease.InOutSine)
            );
            motionSequence.Join(
                transform.DORotate(transform.rotation.eulerAngles + Vector3.up * -7f, 2.6f).SetEase(Ease.InOutSine)
            );

            motionSequence.Append(
                transform.DOLocalMoveX(_startPosition.x, 1.8f).SetEase(Ease.InOutSine)
            );
            motionSequence.Join(
                transform.DORotate(transform.rotation.eulerAngles, 1.8f).SetEase(Ease.InOutSine)
            );

            motionSequence.SetLoops(-1, LoopType.Restart);

            _tiltTween = motionSequence;
        }




        public void StopMoving()
        {
            _isMoving = false;

            _sideMoveTween?.Kill();
            _tiltTween?.Kill();
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
        }

        public void Restore()
        {
            _health = _maxHealth;
            healthSlider.SetValue(_health);
        }
    }
}
