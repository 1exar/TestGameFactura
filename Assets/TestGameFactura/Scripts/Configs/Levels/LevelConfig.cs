using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestGameFactura.Scripts.Configs.Levels
{
    
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private List<LevelStage> stages = new();
        public List<LevelStage> Stages => stages;
    }

    [Serializable]
    public class LevelStage
    {
        public int enemiesCount;
    }
}