using Cysharp.Threading.Tasks;
using TestGameFactura.Scripts.Pools.Sound;
using UnityEngine;

namespace TestGameFactura.Scripts.Managers.SoundManager
{
    public class Sound : MonoBehaviour
    {

        [SerializeField] private AudioSource source;
        private SoundPool _soundPool;
        
        public void Init(AudioClip clip, bool randomizePitch, SoundPool soundPool)
        {
            source.clip = clip;
            source.pitch = randomizePitch ? Random.Range(0.8f, 1.1f) : 1f;
            _soundPool = soundPool;
            
            source.Play();
            
            WaitBeforeRelease();
        }

        private async void WaitBeforeRelease()
        {
            await UniTask.WaitForSeconds(source.clip.length);
            _soundPool.Release(this);
        }
        
    }
}