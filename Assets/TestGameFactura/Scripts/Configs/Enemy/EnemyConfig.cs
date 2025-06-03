using UnityEngine;

namespace TestGameFactura.Scripts.Configs.Enemy
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private int damage;
        [SerializeField] private float aggroDistance;
        [SerializeField] private int maxHealth;
        [SerializeField] private float stoppingDinstance;
        
        public float MoveSpeed => movementSpeed;
        public int Damage => damage;
        public float AggroDistance => aggroDistance;
        public int MaxHealth => maxHealth;
        public float StoppingDinstance => stoppingDinstance;

    }
}