namespace TestGameFactura.Scripts.Managers.SoundManager
{
    public interface ISoundManager
    {
        public void PlayShootSound();
        public void PlayHitSound();
        public void PlayDeathSound();
        public void PlayEnemyDeathSound();
        public void PlayCarDamageSound();

    }
}