using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestGameFactura.Scripts.Configs.Levels
{
    
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private List<LevelStage> stages = new();
        [SerializeField] private GameObject levelStagePrefab;
        [SerializeField] private float stageLength;
        [SerializeField] private Vector2 enemySpawnRange;
        
        public List<LevelStage> Stages => stages;
        public GameObject LevelStagePrefab => levelStagePrefab;
        public float StageLength => stageLength;
        public Vector2 EnemySpawnRange => enemySpawnRange;
    }

    [Serializable]
    public class LevelStage
    {
        public int enemiesCount;
    }
}