using System.Threading.Tasks;

namespace TestGameFactura.Scripts.Entities.Interfaces.Health
{
    public interface IHealth
    {
        public Task TakeDamage(int damage);
        public void Restore();
        public int Health { get; }
    }
}