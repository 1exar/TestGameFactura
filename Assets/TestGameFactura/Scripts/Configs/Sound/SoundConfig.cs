using UnityEngine;

namespace TestGameFactura.Scripts.Configs.Sound
{
    [CreateAssetMenu(fileName = "Sound Config", menuName = "Configs/Sound")]
    public class SoundConfig : ScriptableObject
    {

        [SerializeField] private AudioClip shootSound;
        [SerializeField] private AudioClip hitSound;
        [SerializeField] private AudioClip deathSound;
        [SerializeField] private AudioClip enemyDeathSound;
        [SerializeField] private AudioClip carDamageSound;

        public AudioClip ShootSound => shootSound;
        public AudioClip HitSound => hitSound;
        public AudioClip DeathSound => deathSound;
        public AudioClip EnemyDeathSound => enemyDeathSound;
        public AudioClip CarDamageSound => carDamageSound;

    }
}