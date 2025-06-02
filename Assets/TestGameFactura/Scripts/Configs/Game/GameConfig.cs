using System;
using TestGameFactura.Scripts.Configs.Levels;
using TestGameFactura.Scripts.Configs.Player;
using UnityEngine;

namespace TestGameFactura.Scripts.Configs.Game
{
    [Serializable]
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
    public class GameConfig : ScriptableObject
    {
        
        [Header("Level Settings")]
        [SerializeField] private LevelConfig levelConfig;
        [SerializeField] private GameObject levelStagePrefab;
        [SerializeField] private float stageLength;
        [SerializeField] private Vector2 enemySpawnRange;

        [Header("Player Settings")]
        [SerializeField] private PlayerTurretConfig playerTurretConfig;

        [Header("Enemy Settings")]
        [SerializeField] private GameObject enemyPrefab;

        public LevelConfig LevelConfig => levelConfig;
        public GameObject LevelStagePrefab => levelStagePrefab;
        public float StageLength => stageLength;
        public Vector2 EnemySpawnRange => enemySpawnRange;

        public PlayerTurretConfig PlayerTurretConfig => playerTurretConfig;

        public GameObject EnemyPrefab => enemyPrefab;

    }
    
}