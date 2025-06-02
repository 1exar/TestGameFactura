using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestGameFactura.Scripts.Configs.Levels
{
    
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public List<LevelStage> Stages = new();
    }

    [Serializable]
    public class LevelStage
    {
        public int enemiesCount;
        public int enemiesMaxHp;
    }
}