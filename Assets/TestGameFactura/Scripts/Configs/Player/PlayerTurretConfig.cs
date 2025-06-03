using UnityEngine;

namespace TestGameFactura.Scripts.Configs.Player
{
    [CreateAssetMenu(fileName = "PlayerTurretConfig", menuName = "Configs/PlayerTurret")]
    public class PlayerTurretConfig : ScriptableObject
    {
        [SerializeField] private float yawLimit;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float fireCooldown;

        [SerializeField] private int damage;
        [SerializeField] private int bulletSpeed;
        [SerializeField] private int bulletLifeTime;
        
        public float YawLimit => yawLimit;
        public float RotationSpeed => rotationSpeed;
        public float FireCooldown => fireCooldown;
        public int Damage => damage;
        public int BulletSpeed => bulletSpeed;
        public int BulletLifeTime => bulletLifeTime;
    }
}
