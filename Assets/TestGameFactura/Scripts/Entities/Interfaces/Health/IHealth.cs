namespace TestGameFactura.Scripts.Entities.Interfaces.Health
{
    public interface IHealth
    {
        public void TakeDamage(int damage);
        public void Restore();
        public int Health { get; }
    }
}