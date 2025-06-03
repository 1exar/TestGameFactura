using System;
using TestGameFactura.Scripts.Configs.Sound;
using TestGameFactura.Scripts.Pools.Sound;
using UnityEngine;
using Zenject;

namespace TestGameFactura.Scripts.Managers.SoundManager
{
    public class SoundManager : MonoBehaviour, ISoundManager
    {

        [Inject] private SoundConfig _config;    
        [Inject] private SoundPool _pool;

        private void Start()
        {
            _pool.ClearPool();
        }

        private void PlaySound(AudioClip clip, bool randomPitch)
        {
            var sound = _pool.Get();
            sound.Init(clip, randomPitch, _pool);
        }
        
        public void PlayShootSound()
        {
            PlaySound(_config.ShootSound, true);
        }

        public void PlayHitSound()
        {
            PlaySound(_config.HitSound, true);
        }

        public void PlayDeathSound()
        {
            PlaySound(_config.DeathSound, true);
        }

        public void PlayEnemyDeathSound()
        {
            PlaySound(_config.EnemyDeathSound, true);
        }

        public void PlayCarDamageSound()
        {
            PlaySound(_config.CarDamageSound, true);
        }
    }
}