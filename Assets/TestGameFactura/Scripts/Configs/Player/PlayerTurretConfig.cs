using UnityEngine;

namespace TestGameFactura.Scripts.Configs.Player
{
    [CreateAssetMenu(fileName = "PlayerTurretConfig", menuName = "Configs/PlayerTurret")]
    public class PlayerTurretConfig : ScriptableObject
    {
        public GameObject BulletPrefab;
        public float YawLimit;
        public float RotationSpeed;
        public float FireCooldown;
    }
}
